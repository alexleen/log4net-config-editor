// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using Editor.Models.ConfigChildren;
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
        private readonly IFile mFile;
        private readonly IAppenderFactory mAppenderFactory;

        public ConfigurationFactory(IMessageBoxService messageBoxService)
        {
            mMessageBoxService = messageBoxService;

            mXmlDocFactory = new XmlDocumentFactory();
            mFileStreamFactory = new FileStreamWrapFactory();
            mXmlWriterFactory = new XmlWriterFactory();
            mFile = new FileWrap();
            mAppenderFactory = new AppenderFactory();
        }

        public IConfigurationXml Create(string filename)
        {
            return new ConfigurationXml(mMessageBoxService, new CanLoadAndSaveXml(filename, mXmlDocFactory, mFileStreamFactory, mXmlWriterFactory, mFile), mAppenderFactory);
        }
    }
}
