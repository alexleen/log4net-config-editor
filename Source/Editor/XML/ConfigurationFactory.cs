using Editor.Interfaces;
using SystemInterface.IO;
using SystemInterface.Xml;
using SystemWrapper.IO;
using SystemWrapper.Xml;

namespace Editor.XML
{
    internal partial class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly IXmlDocumentFactory mXmlDocFactory;
        private readonly IFileStreamFactory mFileStreamFactory;
        private readonly IXmlWriterFactory mXmlWriterFactory;

        public ConfigurationFactory(IMessageBoxService messageBoxService)
        {
            mMessageBoxService = messageBoxService;

            mXmlDocFactory = new XmlDocumentFactory();
            mFileStreamFactory = new FileStreamWrapFactory();
            mXmlWriterFactory = new XmlWriterFactory();
        }

        public IConfigurationXml Create(string filename)
        {
            return new ConfigurationXml(filename, mMessageBoxService, new CanLoadAndSaveXml(filename, mXmlDocFactory, mFileStreamFactory, mXmlWriterFactory));
        }
    }
}
