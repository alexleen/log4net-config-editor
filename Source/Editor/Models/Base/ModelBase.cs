// Copyright © 2018 Alex Leendertsen

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Editor.Models.Base
{
    public class ModelBase : INotifyPropertyChanged
    {
        private XmlNode mNode;

        public virtual XmlNode Node
        {
            get => mNode;
            set
            {
                if (value == mNode)
                {
                    return;
                }

                mNode = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
