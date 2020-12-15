// Copyright © 2020 Alex Leendertsen

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Editor.Interfaces;
using Editor.Properties;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    public abstract class PropertyBase : IProperty, INotifyPropertyChanged
    {
        private int mRowIndex;

        protected PropertyBase(GridLength rowHeight)
        {
            RowHeight = rowHeight;
            Navigate = new Command(HyperlinkOnRequestNavigate);
        }

        public ICommand Navigate { get; }

        public string ToolTip { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int RowIndex
        {
            get => mRowIndex;
            set
            {
                if (value == mRowIndex)
                {
                    return;
                }

                mRowIndex = value;
                OnPropertyChanged();
            }
        }

        public GridLength RowHeight { get; }

        public virtual void Load(IElementConfiguration config)
        {
        }

        public virtual bool TryValidate(IMessageBoxService messageBoxService)
        {
            return true;
        }

        public virtual void Save(IElementConfiguration config)
        {
        }

        private static void HyperlinkOnRequestNavigate(object uri)
        {
            Process.Start(new ProcessStartInfo(uri.ToString()));
        }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{GetType().Name} @ {RowIndex}";
        }
    }
}
