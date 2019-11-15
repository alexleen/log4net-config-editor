// Copyright © 2019 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Encoding : MultiValuePropertyBase<System.Text.Encoding>
    {
        private static readonly System.Text.Encoding[] sValues =
        {
            null,
            System.Text.Encoding.ASCII,
            System.Text.Encoding.Unicode,
            System.Text.Encoding.BigEndianUnicode,
            System.Text.Encoding.UTF7,
            System.Text.Encoding.UTF8,
            System.Text.Encoding.UTF32
        };

        public Encoding(ReadOnlyCollection<IProperty> container, string name)
            : base(container, GridLength.Auto, name, sValues, 100)
        {
            SelectedValue = sValues[0];
        }

        public override void Load(XmlNode originalNode)
        {
            throw new NotImplementedException();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            throw new NotImplementedException();
        }
    }
}
