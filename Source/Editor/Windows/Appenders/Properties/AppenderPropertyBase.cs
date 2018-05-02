// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using System.Xml;
using Editor.Annotations;

namespace Editor.Windows.Appenders.Properties
{
    public abstract class AppenderPropertyBase : IAppenderProperty, INotifyPropertyChanged
    {
        private readonly ObservableCollection<IAppenderProperty> mContainer;
        protected readonly XmlNode AppenderNode;

        protected AppenderPropertyBase(ObservableCollection<IAppenderProperty> container, GridLength rowHeight, XmlNode appenderNode)
        {
            mContainer = container;
            AppenderNode = appenderNode;
            RowHeight = rowHeight;
        }

        public int RowIndex => mContainer.IndexOf(this);

        public GridLength RowHeight { get; }

        public abstract void Load();

        public virtual bool TryValidate()
        {
            return true;
        }

        public abstract void Save(XmlDocument xmlDoc);

        protected void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
