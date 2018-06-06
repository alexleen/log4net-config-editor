// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using log4net.Core;

namespace Editor.Windows.Appenders.Properties
{
    public class Fix : PropertyBase
    {
        public const string NonePreset = "None";
        public const string PartialPreset = "Partial";
        public const string AllPreset = "All";
        public const string CustomPreset = "Custom";
        private const string FixName = "Fix";

        public Fix(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
            Presets = new[] { NonePreset, PartialPreset, AllPreset, CustomPreset };

            Fixes = new[]
            {
                new FixModel(FixFlags.Ndc, false),
                new FixModel(FixFlags.Message, false),
                new FixModel(FixFlags.ThreadName, false),
                new FixModel(FixFlags.LocationInfo, true, "Possible performance degradation"),
                new FixModel(FixFlags.UserName, true, "Possible performance degradation"),
                new FixModel(FixFlags.Domain, false),
                new FixModel(FixFlags.Identity, true, "Possible performance degradation"),
                new FixModel(FixFlags.Exception, false),
                new FixModel(FixFlags.Properties, false)
            };

            SelectedPreset = NonePreset;
        }

        public IEnumerable<string> Presets { get; }

        private string mSelectedPreset;

        public string SelectedPreset
        {
            get => mSelectedPreset;
            set
            {
                if (value == mSelectedPreset)
                {
                    return;
                }

                switch (value)
                {
                    case NonePreset:
                        foreach (FixModel fix in Fixes)
                        {
                            fix.Enabled = false;
                        }
                        break;
                    case PartialPreset:
                        foreach (FixModel fix in Fixes)
                        {
                            if (FixFlags.Partial.HasFlag(fix.Flag))
                            {
                                fix.Enabled = true;
                            }
                            else
                            {
                                fix.Enabled = false;
                            }
                        }
                        break;
                    case AllPreset:
                        foreach (FixModel fix in Fixes)
                        {
                            fix.Enabled = true;
                        }
                        break;
                }

                mSelectedPreset = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<FixModel> Fixes { get; }

        public override void Load(XmlNode originalNode)
        {
            string fixValue = originalNode.GetValueAttributeValueFromChildElement(FixName);

            if (int.TryParse(fixValue, out int fixValueInt) && Enum.IsDefined(typeof(FixFlags), fixValueInt))
            {
                //SelectedFix = (FixFlags)fixValueInt;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            //xmlDoc.CreateElementWithValueAttribute(FixName, ((int)SelectedFix).ToString()).AppendTo(newNode);
        }
    }
}
