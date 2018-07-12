// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class StringMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;

        public StringMatch(ReadOnlyCollection<IProperty> container, Func<bool> validate)
            : base(container, "String to Match:", "stringToMatch")
        {
            mValidate = validate;
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }
    }
}
