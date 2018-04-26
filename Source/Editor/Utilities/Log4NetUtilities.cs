// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using log4net.Core;

namespace Editor.Utilities
{
    public class Log4NetUtilities
    {
        public static readonly IReadOnlyDictionary<string, Level> LevelsByName = new Dictionary<string, Level>
        {
            { Level.All.Name, Level.All },
            { Level.Finest.Name, Level.Finest },
            { Level.Verbose.Name, Level.Verbose },
            { Level.Finer.Name, Level.Finer },
            { Level.Trace.Name, Level.Trace },
            { Level.Fine.Name, Level.Fine },
            { Level.Debug.Name, Level.Debug },
            { Level.Info.Name, Level.Info },
            { Level.Notice.Name, Level.Notice },
            { Level.Warn.Name, Level.Warn },
            { Level.Error.Name, Level.Error },
            { Level.Severe.Name, Level.Severe },
            { Level.Critical.Name, Level.Critical },
            { Level.Alert.Name, Level.Alert },
            { Level.Fatal.Name, Level.Fatal },
            { Level.Emergency.Name, Level.Emergency },
            { Level.Off.Name, Level.Off }
        };
    }
}
