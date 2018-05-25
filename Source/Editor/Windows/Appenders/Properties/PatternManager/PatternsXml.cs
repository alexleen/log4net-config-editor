// Copyright © 2018 Alex Leendertsen

using System;

namespace Editor.Windows.Appenders.Properties.PatternManager
{
    [Serializable]
    public class PatternsXml
    {
        public StoredPattern[] Patterns { get; set; }
    }
}
