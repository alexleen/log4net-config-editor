// Copyright © 2018 Alex Leendertsen

using System;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class StringMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;

        public StringMatch(Func<bool> validate)
            : base("String to Match:", "stringToMatch")
        {
            mValidate = validate;
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }
    }
}
