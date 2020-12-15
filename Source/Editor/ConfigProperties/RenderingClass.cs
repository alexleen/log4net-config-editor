// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    /// <summary>
    /// Represents the rendering class attribute of a renderer.
    /// </summary>
    internal class RenderingClass : StringValueProperty
    {
        private const string RenderingClassName = "renderingClass";

        internal RenderingClass()
            : base("Rendering Class:", null)
        {
            IsFocused = true;
            ToolTip = "Value must be the type name for this renderer. " +
                      "If the type is not defined in the log4net assembly this type name must be fully assembly qualified. " +
                      "This is the type of the object that will take responsibility for rendering the 'Rendered Class'.";
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(RenderingClassName, out IValueResult result))
            {
                Value = result.AttributeValue;
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A valid rendering class must be assigned.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(IElementConfiguration config)
        {
            config.Save(RenderingClassName, Value);
        }
    }
}
