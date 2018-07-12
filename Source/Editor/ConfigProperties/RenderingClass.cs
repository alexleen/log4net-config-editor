// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class RenderingClass : StringValueProperty
    {
        private const string RenderingClassName = "renderingClass";

        internal RenderingClass(ReadOnlyCollection<IProperty> container)
            : base(container, "Rendering Class:", null)
        {
            IsFocused = true;
            ToolTip = "Value must be the type name for this renderer. " +
                      "If the type is not defined in the log4net assembly this type name must be fully assembly qualified. " +
                      "This is the type of the object that will take responsibility for rendering the 'Rendered Class'.";
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes[RenderingClassName]?.Value);
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

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, RenderingClassName, Value);
        }
    }
}
