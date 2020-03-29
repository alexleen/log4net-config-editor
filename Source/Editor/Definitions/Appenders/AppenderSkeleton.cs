// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Base;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.Definitions.Appenders
{
    internal abstract class AppenderSkeleton : ElementDefinition, IAppenderDefinition
    {
        protected readonly IElementConfiguration Configuration;
        private readonly bool mRequiresLayout;

        protected AppenderSkeleton(IElementConfiguration configuration, bool requiresLayout = true)
        {
            Configuration = configuration;
            mRequiresLayout = requiresLayout;
        }

        public override string Icon => "pack://application:,,,/Editor;component/Images/list-add.png";

        public abstract AppenderDescriptor Descriptor { get; }

        public override void Initialize()
        {
            AddProperty(new TypeAttribute(Descriptor));
            Name nameProperty = new Name(Configuration);
            AddProperty(nameProperty);
            AddProperty(new StringValueProperty("Error Handler:", "errorHandler", Log4NetXmlConstants.Type) { ToolTip = "Specify a type that implements IErrorHandler to handle appender related errors." });
            AddProperty(new Threshold());

            AddAppenderSpecificProperties();

            AddProperty(new Layout(new HistoryManagerFactory(new SettingManager<string>())));
            AddProperty(new ConfigProperties.Filters(Configuration, MessageBoxService));
            AddProperty(new IncomingRefs(nameProperty, Configuration));
            AddProperty(new Params(Configuration, MessageBoxService));
        }

        protected virtual void AddAppenderSpecificProperties()
        {
        }
    }
}
