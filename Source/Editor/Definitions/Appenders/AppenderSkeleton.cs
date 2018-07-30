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
            AddProperty(new TypeAttribute(Properties, Descriptor));
            Name nameProperty = new Name(Properties, Configuration);
            AddProperty(nameProperty);
            AddProperty(new Threshold(Properties));

            AddAppenderSpecificProperties();

            AddProperty(new Layout(Properties, new HistoryManager.HistoryManager("HistoricalPatterns", new SettingManager<string>()), mRequiresLayout));
            AddProperty(new ConfigProperties.Filters(Properties, Configuration, MessageBoxService));
            AddProperty(new IncomingRefs(Properties, nameProperty, Configuration));
            AddProperty(new Params(Properties, Configuration, MessageBoxService));
        }

        protected virtual void AddAppenderSpecificProperties()
        {
        }
    }
}
