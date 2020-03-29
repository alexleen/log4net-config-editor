// Copyright © 2020 Alex Leendertsen

namespace Editor.ConfigProperties
{
    /// <summary>
    /// This type exists for the sole purpose of selecting the password data template in XAML so that the value is obfuscated.
    /// </summary>
    internal class Password : RequiredStringProperty
    {
        public Password(string name, string elementName)
            : base(name, elementName)
        {
        }
    }
}
