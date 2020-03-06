// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;

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
