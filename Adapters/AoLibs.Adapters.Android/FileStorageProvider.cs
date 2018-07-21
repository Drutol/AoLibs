using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using AoLibs.Adapters.Core.Interfaces;
using Java.IO;
using Console = System.Console;
using File = Java.IO.File;

namespace AoLibs.Adapters.Android
{
    public class FileStorageProvider : IFileStorageProvider
    {
        private readonly string _rootPath;

        public FileStorageProvider()
        {
            _rootPath = Application.Context.GetExternalFilesDir(null).Path;
        }

        private string ResolvePath(string path)
        {
            var finalPath = _rootPath;
            foreach (var pathPiece in path.Split('/').ToArray())          
                finalPath = Path.Combine(finalPath, pathPiece);

            if (!finalPath.Split('/').Last().Contains('.'))
                finalPath += ".dat";

            return finalPath;
        }

        public async Task<string> ReadTextAsync(string path)
        {
            try
            {
                var file = new File(ResolvePath(path));
                var text = new StringBuilder();

                using (BufferedReader br = new BufferedReader(new FileReader(file)))
                {
                    string line;
                    while ((line = await br.ReadLineAsync()) != null)
                    {
                        text.Append(line);
                    }
                }
                return text.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]> ReadBytesAsync(string path)
        {
            try
            {
                using (var streamReader = new StreamReader(ResolvePath(path)))
                {
                    using (var memstream = new MemoryStream())
                    {
                        await streamReader.BaseStream.CopyToAsync(memstream);
                        return memstream.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        public void WriteText(string path, string text)
        {
            var file = EnsureFileCreated(path);

            using (var writer = new FileWriter(file))
            {
                writer.Write(text);
                writer.Close();
            }
        }

        public async Task WriteTextAsync(string path, string text)
        {
            var file = EnsureFileCreated(path);

            using (var writer = new FileWriter(file))
            {
                await writer.WriteAsync(text);
                writer.Close();
            }
        }

        public void WriteBytes(string path, byte[] bytes)
        {
            var file = EnsureFileCreated(path);

            using (var streamWriter = new StreamWriter(file.Path))
            {
                streamWriter.BaseStream.Write(bytes, 0, bytes.Length);
            }
        }

        public async Task WriteBytesAsync(string path, byte[] bytes)
        {
            var file = EnsureFileCreated(path);

            using (var streamWriter = new StreamWriter(file.Path))
            {
                await streamWriter.BaseStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public void RemoveFile(string path)
        {
            var file = new File(ResolvePath(path));
            file.Delete();
        }

        private File EnsureFileCreated(string path)
        {
            var file = new File(ResolvePath(path));

            if (!file.ParentFile.Exists())
                file.ParentFile.Mkdirs();
            file.CreateNewFile();

            return file;
        }
    }
}