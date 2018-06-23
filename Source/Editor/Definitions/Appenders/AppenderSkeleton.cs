// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Windows;

namespace Editor.Definitions.Appenders
{
    internal abstract class AppenderSkeleton : PropertyDefinition, IAppenderDefinition
    {
        protected readonly IAppenderConfiguration mAppenderConfiguration;
        protected readonly IMessageBoxService mMessageBoxService;
        private readonly IList<IProperty> mProperties;

        protected AppenderSkeleton(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
        {
            mAppenderConfiguration = appenderConfiguration;
            mMessageBoxService = messageBoxService;
            mProperties = new List<IProperty>();
            Properties = new ReadOnlyCollection<IProperty>(mProperties);
        }

        public abstract AppenderDescriptor Descriptor { get; }

        public override void Initialize()
        {
            Name nameProperty = new Name(Properties, mAppenderConfiguration);
            mProperties.Add(nameProperty);
            mProperties.Add(new Threshold(Properties));

            AddAppenderSpecificProperties();

            mProperties.Add(new Layout(Properties, new HistoryManager.HistoryManager("HistoricalPatterns", new SettingManager<string>())));
            mProperties.Add(new ConfigProperties.Filters(Properties, mAppenderConfiguration, mMessageBoxService));
            mProperties.Add(new IncomingRefs(Properties, nameProperty, mAppenderConfiguration));
        }

        protected virtual void AddAppenderSpecificProperties()
        {
        }

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
        }

        protected void AddProperty(int index, IProperty property)
        {
            mProperties.Insert(index, property);
        }

        protected void RemoveProperty(IProperty property)
        {
            mProperties.Remove(property);
        }

        public override ReadOnlyCollection<IProperty> Properties { get; }
    }
}
