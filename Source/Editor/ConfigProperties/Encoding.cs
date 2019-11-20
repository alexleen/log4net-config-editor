// Copyright © 2019 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Encoding : MultiValuePropertyBase<string>
    {
        private static readonly string[] sValues =
        {
            string.Empty,
            System.Text.Encoding.ASCII.EncodingName,
            System.Text.Encoding.Unicode.EncodingName,
            System.Text.Encoding.BigEndianUnicode.EncodingName,
            System.Text.Encoding.UTF7.EncodingName,
            System.Text.Encoding.UTF8.EncodingName,
            System.Text.Encoding.UTF32.EncodingName
        };

        public Encoding(ReadOnlyCollection<IProperty> container, string name)
            : base(container, GridLength.Auto, name, sValues, 140)
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
