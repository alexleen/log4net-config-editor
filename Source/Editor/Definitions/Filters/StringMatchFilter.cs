// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class StringMatchFilter : FilterSkeleton
    {
        private readonly RegexMatch mRegexMatch;
        private readonly StringMatch mStringMatch;

        public StringMatchFilter(bool isFocused = true)
        {
            mStringMatch = new StringMatch(Validate) { IsFocused = isFocused };
            mRegexMatch = new RegexMatch(Validate);
        }

        public override string Name => "String Match Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.String;

        public override void Initialize()
        {
            AddProperty(mStringMatch);
            AddProperty(mRegexMatch);
            base.Initialize();
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(mStringMatch.Value) && string.IsNullOrEmpty(mRegexMatch.Value))
            {
                MessageBoxService.ShowError("Either 'String to Match' or 'Regex to Match' must be specified.");
                return false;
            }

            return true;
        }
    }
}
