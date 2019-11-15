using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    //TODO use this inplace of others that do the same thing
    internal class RequiredStringProperty : StringValueProperty
    {
        public RequiredStringProperty(ReadOnlyCollection<IProperty> container, string name, string elementName)
            : base(container, name, elementName)
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError($"'{Name.TrimEnd(':')}' must be specified.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
