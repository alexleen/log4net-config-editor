using SystemInterface.Xml;

namespace Editor.Interfaces
{
    internal interface ICanLoadAndSaveXml
    {
        IXmlDocument Load();

        void Save();
    }
}
