// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using log4net.Core;

namespace Editor.ConfigProperties.Base
{
    internal abstract class LevelPropertyBase : MultiValuePropertyBase<string>
    {
        protected LevelPropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, string name, bool prependEmpty = false)
            : base(container, rowHeight, name, prependEmpty ? new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys) : Log4NetUtilities.LevelsByName.Keys, 100)
        {
            SelectedValue = Values.First();
        }

        protected void TryLoadSelectedLevel(string level)
        {
            if (Log4NetUtilities.TryParseLevel(level, out Level match))
            {
                SelectedValue = match.Name;
            }
        }
    }
}
