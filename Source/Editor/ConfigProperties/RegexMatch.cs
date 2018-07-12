// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class RegexMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;

        public RegexMatch(ReadOnlyCollection<IProperty> container, Func<bool> validate)
            : base(container, "Regex to Match:", "regexToMatch")
        {
            mValidate = validate;
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }
    }
}
