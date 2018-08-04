using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Sample.Shared.Interfaces;

namespace AoLibs.Sample.Shared.BL
{
    public class FancyTurtlesProvider : ISomeFancyProvider
    {
        public string SomethingFancy { get; } = "💣💣 TURTLE 💣💣";
    }
}
