using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;
using AoLibs.Adapters.Core.Interfaces;
using Newtonsoft.Json;

namespace AoLibs.Adapters.Core
{
    public abstract class AppVariablesBase
    {
        private readonly ISyncStorage _syncStorage;
        private readonly IAsyncStorage _asyncStorage;

        public IReadOnlyCollection<VariableAttribute> Attributes { get; internal set; }

        /// <summary>
        /// Attribute that marks property of type <see cref="Holder{T}"/> as variable for persistent data storage.
        /// Marked property is required to have both setter and getter. 
        /// Properties of type different than <see cref="Holder{T}"/> will be ignored.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class VariableAttribute : Attribute
        {
            /// <summary>
            /// Don't store in persistent cache.
            /// </summary>
            public bool MemoryOnly { get; set; }

            /// <summary>
            /// Store variable using specified key, by default property name will be used.
            /// </summary>
            public string CustomKey { get; set; }

            /// <summary>
            /// Time in seconds describing how long data is valid since last write.
            /// By default only supported in async calls when calling <see cref="AppVariablesBase"/> 
            /// contructor with <see cref="IDataCache"/>. Provide custom <see cref="ISyncStorage"/> to consume this attribute.
            /// </summary>
            public int ExpirationTime { get; set; } = -1;
        }

        public interface IAsyncStorage
        {
            Task SetAsync<T>(T data, string key, VariableAttribute attr);
            Task<T> GetAsync<T>(string key, VariableAttribute attr);
            Task Reset(string key, VariableAttribute attr);
        }

        public interface ISyncStorage
        {
            void SetValue<T>(T data, string key, VariableAttribute attr);
            void GetValue<T>(ref T data, string key, VariableAttribute attr);
        }

        private class DefaultSyncStorage : ISyncStorage
        {
            private readonly ISettingsProvider _settingsProvider;

            public DefaultSyncStorage(ISettingsProvider settingsProvider)
            {
                _settingsProvider = settingsProvider;
            }

            public void GetValue<T>(ref T local, string prop, VariableAttribute attr)
            {
                var cached = _settingsProvider.GetString(prop);
                if (cached == null)
                    return;

                local = JsonConvert.DeserializeObject<T>(cached);
            }

            public void SetValue<T>(T value, string prop, VariableAttribute attr)
            {
                _settingsProvider.SetString(prop,
                    value == null ? null : JsonConvert.SerializeObject(value));
            }
        }

        private class DefaultAsyncStorage : IAsyncStorage
        {
            private readonly IDataCache _dataCache;

            class TimestampWrapper<T>
            {
                public TimestampWrapper(T data)
                {
                    Value = data;
                }

                public DateTime WriteTime { get; set; }
                public T Value { get; set; }
            }

            public DefaultAsyncStorage(IDataCache dataCache)
            {
                _dataCache = dataCache;
            }

            public Task SetAsync<T>(T data, string key, VariableAttribute attr)
            {
                return _dataCache.SaveDataAsync(key, data);
            }

            public async Task<T> GetAsync<T>(string key, VariableAttribute attr)
            {
                try
                {
                    return await _dataCache.RetrieveData<T>(key,
                        attr.ExpirationTime > 0 ? (TimeSpan?) TimeSpan.FromSeconds(attr.ExpirationTime) : null);
                }
                catch (DataExpiredException)
                {
                    return default(T);
                }
            }

            public Task Reset(string key, VariableAttribute attr)
            {
                return _dataCache.Clear(key);
            }
        }

        [Preserve(AllMembers = true)]
        public class HolderBase
        {
            internal static AppVariablesBase _parent;
        }

        /// <summary>
        /// Class that holds stored data. Cannot be inherited. 
        /// If not instantinated it will be automatically created by underlying mechanisms.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Preserve(AllMembers = true)]
        public sealed class Holder<T> : HolderBase where T : class
        {
            public event EventHandler<T> ValueChanged;

            private readonly VariableAttribute _attribute;
            private readonly string _propName;
            private T _value;
            private T _defaultValue;

            public Holder(T defaultValue)
            {
                _value = defaultValue;
            }

            public Holder(string propName, VariableAttribute attribute)
            {
                _attribute = attribute;
                _propName = attribute.CustomKey ?? propName;
            }

            /// <summary>
            /// Clears both memory storage and <see cref="ISyncStorage"/>
            /// </summary>
            public void Reset()
            {
                _value = default(T);
                if (!_attribute.MemoryOnly)
                    _parent._syncStorage.SetValue(default(T), _propName, _attribute);
            }

            /// <summary>
            /// Clears both memory storage and <see cref="IAsyncStorage"/>
            /// </summary>
            public async Task ResetAsync()
            {
                _value = default(T);
                if (!_attribute.MemoryOnly)
                    await _parent._asyncStorage.Reset(_propName, _attribute);
            }

            public T Value
            {
                get
                {
                    if (_value != null)
                        return _value;
                    if (!_attribute.MemoryOnly)
                        _parent._syncStorage.GetValue(ref _value, _propName, _attribute);
                    return _value;
                }
                set
                {
                    _value = value;
                    if (!_attribute.MemoryOnly)
                        _parent._syncStorage.SetValue(value, _propName, _attribute);
                    ValueChanged?.Invoke(this, _value);
                }
            }

            public Task<T> GetAsync()
            {
                if (_parent._asyncStorage == null)
                    throw new InvalidOperationException(
                        "You can call async methods only after providing async storage interface in AppVariablesBase.");
                if (_attribute.MemoryOnly)
                    return Task.FromResult(_value);

                return _parent._asyncStorage.GetAsync<T>(_propName, _attribute);
            }

            public async Task SetAsync(T data)
            {
                if (_parent._asyncStorage == null)
                    throw new InvalidOperationException(
                        "You can call async methods only after providing async storage interface in AppVariablesBase.");
                if (_attribute.MemoryOnly)
                {
                    _value = data;
                    return;
                }

                await _parent._asyncStorage.SetAsync(data, _propName, _attribute);
                ValueChanged?.Invoke(this, _value);
            }
        }

        private AppVariablesBase()
        {
            HolderBase._parent = this;

            var props = this.GetType().GetRuntimeProperties()
                .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(Holder<>));
            var attributes = new List<VariableAttribute>();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<VariableAttribute>();

                if (attr != null)
                {
                    attributes.Add(attr);
                    var holder = prop.GetValue(this);
                    if (holder == null) //default value not provided, we have to create instance
                    {
                        prop.SetValue(this, Activator.CreateInstance(prop.PropertyType,
                            new object[] {prop.Name, attr}));
                    }
                    else //we have value but we have to fill missing data
                    {
                        var typeInfo = holder.GetType().GetTypeInfo();
                        typeInfo.GetDeclaredField("_propName")
                            .SetValue(holder, attr.CustomKey ?? prop.Name);

                        typeInfo.GetDeclaredField("_attribute")
                            .SetValue(holder, attr);
                    }
                }

                Attributes = attributes;
            }
        }

        /// <summary>
        /// Initialize with default <see cref="ISyncStorage"/> where <see cref="ISettingsProvider"/> is underlaying storage layer.
        /// Async methods of <see cref="Holder{T}"/> will be unavailable and throw <see cref="InvalidOperationException"/>
        /// </summary>
        /// <param name="settingsProvider"></param>
        /// <param name="dataCache"></param>
        protected AppVariablesBase(ISettingsProvider settingsProvider, IDataCache dataCache = null) : this()
        {
            if (dataCache != null)
                _asyncStorage = new DefaultAsyncStorage(dataCache);
            _syncStorage = new DefaultSyncStorage(settingsProvider);
        }

        /// <summary>
        /// Initialize with custom implementations of <see cref="ISyncStorage"/> and optionally <see cref="IAsyncStorage"/>.
        /// Not providing <see cref="IAsyncStorage"/> will result in <see cref="InvalidOperationException"/> when accessing async methods of <see cref="Holder{T}"/>
        /// </summary>
        /// <param name="syncStorage"></param>
        /// <param name="asyncStorage"></param>
        protected AppVariablesBase(ISyncStorage syncStorage, IAsyncStorage asyncStorage = null)
        {
            _syncStorage = syncStorage;
            _asyncStorage = asyncStorage;
        }
    }
}

