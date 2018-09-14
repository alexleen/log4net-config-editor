using Editor.Interfaces;
using System;
using System.IO;
using System.Xml;
using SystemInterface.IO;
using SystemInterface.Xml;
using SystemWrapper.IO;
using SystemWrapper.Xml;

namespace Editor.XML
{
    internal class ConfigurationFactory : IConfigurationFactory
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

        private class CanLoadAndSaveXml : ICanLoadAndSaveXml
        {
            private readonly string mFilename;
            private readonly IXmlDocumentFactory mXmlDocFactory;
            private readonly IFileStreamFactory mFileStreamFactory;
            private readonly IXmlWriterFactory mXmlWriterFactory;
            private IXmlDocument mConfigXml;

            public CanLoadAndSaveXml(string filename, IXmlDocumentFactory xmlDocFactory, IFileStreamFactory fileStreamFactory, IXmlWriterFactory xmlWriterFactory)
            {
                mFilename = filename ?? throw new ArgumentNullException(nameof(filename));
                mXmlDocFactory = xmlDocFactory ?? throw new ArgumentNullException(nameof(xmlDocFactory));
                mFileStreamFactory = fileStreamFactory ?? throw new ArgumentNullException(nameof(fileStreamFactory));
                mXmlWriterFactory = xmlWriterFactory ?? throw new ArgumentNullException(nameof(xmlWriterFactory));
            }

            public IXmlDocument Load()
            {
                mConfigXml = mXmlDocFactory.Create();
                mConfigXml.Load(mFilename);
                return mConfigXml;
            }

            public void Save()
            {
                using (IFileStream fileStream = mFileStreamFactory.Create(mFilename, FileMode.Create))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                    using (IXmlWriter xtw = mXmlWriterFactory.Create(fileStream.FileStreamInstance, settings))
                    {
                        mConfigXml.Save(xtw.Writer);
                    }
                }
            }
        }
    }
}
