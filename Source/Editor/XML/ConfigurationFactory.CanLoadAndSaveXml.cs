// Copyright © 2019 Alex Leendertsen

using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;
using SystemInterface.IO;
using SystemInterface.Xml;

namespace Editor.XML
{
    internal partial class ConfigurationFactory
    {
        private class CanLoadAndSaveXml : ICanLoadAndSaveXml
        {
            private readonly string mFilename;
            private readonly IXmlDocumentFactory mXmlDocFactory;
            private readonly IFileStreamFactory mFileStreamFactory;
            private readonly IXmlWriterFactory mXmlWriterFactory;
            private readonly IFile mFile;

            public CanLoadAndSaveXml(string filename, IXmlDocumentFactory xmlDocFactory, IFileStreamFactory fileStreamFactory, IXmlWriterFactory xmlWriterFactory, IFile fileWrap)
            {
                mFilename = filename ?? throw new ArgumentNullException(nameof(filename));
                mXmlDocFactory = xmlDocFactory ?? throw new ArgumentNullException(nameof(xmlDocFactory));
                mFileStreamFactory = fileStreamFactory ?? throw new ArgumentNullException(nameof(fileStreamFactory));
                mXmlWriterFactory = xmlWriterFactory ?? throw new ArgumentNullException(nameof(xmlWriterFactory));
                mFile = fileWrap ?? throw new ArgumentNullException(nameof(fileWrap));
            }

            public IXmlDocument Load()
            {
                IXmlDocument configXml = mXmlDocFactory.Create();

                if (mFile.Exists(mFilename))
                {
                    configXml.Load(mFilename);
                }
                else
                {
                    configXml.AppendChild(configXml.CreateElement(Log4NetXmlConstants.Log4Net));
                }

                return configXml;
            }

            public Task SaveAsync(IXmlDocument configXml)
            {
                return Task.Run(() =>
                {
                    using (IFileStream fileStream = mFileStreamFactory.Create(mFilename, FileMode.Create))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                        using (IXmlWriter xtw = mXmlWriterFactory.Create(fileStream.FileStreamInstance, settings))
                        {
                            configXml.Save(xtw.Writer);
                        }
                    }
                });
            }
        }
    }
}
