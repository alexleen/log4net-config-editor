// Copyright © 2019 Alex Leendertsen

using SystemInterface.IO;
using SystemInterface.Xml;
using SystemWrapper.IO;
using SystemWrapper.Xml;
using Editor.Interfaces;

namespace Editor.XML
{
    internal partial class ConfigurationFactory : IConfigurationFactory
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly IXmlDocumentFactory mXmlDocFactory;
        private readonly IFileStreamFactory mFileStreamFactory;
        private readonly IXmlWriterFactory mXmlWriterFactory;
        private readonly IFile mFile;

        public ConfigurationFactory(IMessageBoxService messageBoxService)
        {
            mMessageBoxService = messageBoxService;

            mXmlDocFactory = new XmlDocumentFactory();
            mFileStreamFactory = new FileStreamWrapFactory();
            mXmlWriterFactory = new XmlWriterFactory();
            mFile = new FileWrap();
        }

        public IConfigurationXml Create(string filename)
        {
            return new SaveIndication(mMessageBoxService, new CanLoadAndSaveXml(filename, mXmlDocFactory, mFileStreamFactory, mXmlWriterFactory, mFile));
        }
    }
}
