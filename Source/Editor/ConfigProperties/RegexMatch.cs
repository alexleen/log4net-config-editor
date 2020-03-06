// Copyright © 2018 Alex Leendertsen

using System;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class RegexMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;

        public RegexMatch(Func<bool> validate)
            : base("Regex to Match:", "regexToMatch")
        {
            mValidate = validate;
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }
    }
}
