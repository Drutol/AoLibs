using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Sample.Shared.Interfaces;

namespace AoLibs.Sample.Shared.BL
{
    public class FancyTrainsProvider : ISomeFancyProvider
    {
        public string SomethingFancy { get; } = "🎉🎉 TRAINS 🎉🎉"; // who doesn't like trains right? :D
    }
}
