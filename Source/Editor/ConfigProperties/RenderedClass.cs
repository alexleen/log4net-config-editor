// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class RenderedClass : StringValueProperty
    {
        private const string RenderedClassName = "renderedClass";

        internal RenderedClass()
            : base("Rendered Class:", null)
        {
            ToolTip = "Value must be the type name for the target type for this renderer. " +
                      "If the type is not defined in the log4net assembly this type name must be fully assembly qualified. " +
                      "This is the name of the type that this renderer will render.";
        }

        public override void Load(IElementConfiguration config)
        {
            config.Load(RenderedClassName, out IValueResult result);
            SetValueIfNotNullOrEmpty(result.AttributeValue);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A valid rendered class must be assigned.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(IElementConfiguration config)
        {
            config.Save(RenderedClassName, Value);
        }
    }
}
