// Copyright © 2020 Alex Leendertsen

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using Editor.Descriptors.Base;

namespace Editor.Models.Base
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected ModelBase(XmlNode node, DescriptorBase descriptor)
        {
            Node = node;
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));

            if (Node != null && !string.Equals(Node.Name, descriptor.ElementName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Specified node has incorrect element name ({Node.Name}) for {GetType().Name} ({descriptor.ElementName}).");
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

        public DescriptorBase Descriptor { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
