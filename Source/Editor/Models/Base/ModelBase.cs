// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Editor.Models.Base
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected ModelBase(XmlNode node, DescriptorBase descriptor)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));

            if (ElementName != descriptor.ElementName)
            {
                throw new ArgumentException($"Specified node has incorrect element name for {GetType().Name}.");
            }
        }

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

        public string ElementName => Node.Name;

        public DescriptorBase Descriptor { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
