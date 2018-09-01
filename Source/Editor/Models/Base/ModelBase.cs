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
            Node = node;
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));

            if (ElementName != null && ElementName != descriptor.ElementName)
            {
                throw new ArgumentException($"Specified node has incorrect element name ({ElementName}) for {GetType().Name} ({descriptor.ElementName}).");
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

        //TODO is this really needed? We can probably inline...
        public string ElementName => Node?.Name;

        public DescriptorBase Descriptor { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
