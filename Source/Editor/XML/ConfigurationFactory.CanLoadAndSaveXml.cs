// Copyright © 2018 Alex Leendertsen

using System;
using System.IO;
using System.Xml;
using SystemInterface.IO;
using SystemInterface.Xml;
using Editor.Interfaces;
using Editor.Utilities;

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
            private IXmlDocument mConfigXml;

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
                mConfigXml = mXmlDocFactory.Create();

                if (mFile.Exists(mFilename))
                {
                    mConfigXml.Load(mFilename);
                }
                else
                {
                    mConfigXml.AppendChild(mConfigXml.CreateElement(Log4NetXmlConstants.Log4Net));
                }

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
