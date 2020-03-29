// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.Definitions.Appenders
{
    internal class FileAppender : TextWriterAppender
    {
        internal FileAppender(IElementConfiguration configuration)
            : base(configuration, false)
        {
        }

        public override string Name => "File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.File;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new File(MessageBoxService, new HistoryManagerFactory(new SettingManager<string>())));
            AddProperty(new LockingModel());
            AddProperty(new Encoding("Encoding:", "encoding"));
            AddProperty(new StringValueProperty("Security Context:", "securityContext", Log4NetXmlConstants.Type));
        }
    }
}
