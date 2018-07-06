// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using log4net.Core;

namespace Editor.ConfigProperties.Base
{
    public abstract class LevelPropertyBase : PropertyBase
    {
        protected LevelPropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, string name, bool prependEmpty = false)
            : base(container, rowHeight)
        {
            Name = name;
            Levels = prependEmpty ? new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys) : Log4NetUtilities.LevelsByName.Keys;
            SelectedLevel = Levels.First();
        }

        protected void TryLoadSelectedLevel(string level)
        {
            if (Log4NetUtilities.TryParseLevel(level, out Level match))
            {
                SelectedLevel = match.Name;
            }
        }

        public string Name { get; }

        public IEnumerable<string> Levels { get; }

        public string SelectedLevel { get; set; }
    }
}
