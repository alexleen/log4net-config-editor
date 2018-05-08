// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.Properties;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public abstract class AppenderPropertyBase : IAppenderProperty, INotifyPropertyChanged
    {
        private readonly ObservableCollection<IAppenderProperty> mContainer;

        protected AppenderPropertyBase(ObservableCollection<IAppenderProperty> container, GridLength rowHeight)
        {
            mContainer = container;
            RowHeight = rowHeight;
            Navigate = new Command(HyperlinkOnRequestNavigate);
        }

        public int RowIndex => mContainer.IndexOf(this);

        public GridLength RowHeight { get; }

        public ICommand Navigate { get; }

        public abstract void Load(XmlNode originalAppenderNode);

        public virtual bool TryValidate()
        {
            return true;
        }

        public abstract void Save(XmlDocument xmlDoc, XmlNode newAppenderNode);

        private static void HyperlinkOnRequestNavigate(object uri)
        {
            Process.Start(new ProcessStartInfo(uri.ToString()));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
