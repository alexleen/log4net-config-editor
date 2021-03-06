﻿// Copyright © 2018 Alex Leendertsen

using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Models
{
    public sealed class MappingModel : ModelBase, IEquatable<MappingModel>
    {
        private const string LevelName = "level";
        private const string ForeColorName = "foreColor";
        private const string BackColorName = "backColor";

        private readonly Action<MappingModel> mRemove;
        private readonly Action<MappingModel> mShowMappingWindow;

        public MappingModel(Action<MappingModel> remove,
                            Action<MappingModel> showMappingWindow,
                            XmlNode node)
            : base(node, MappingDescriptor.Mapping)
        {
            mRemove = remove ?? throw new ArgumentNullException(nameof(remove));
            mShowMappingWindow = showMappingWindow ?? throw new ArgumentNullException(nameof(showMappingWindow));
            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
        }

        public MappingModel(Action<MappingModel> remove,
                            Action<MappingModel> showMappingWindow)
            : this(remove, showMappingWindow, null)
        {
        }

        public Level Level { get; private set; }

        public ConsoleColor? ForeColor { get; private set; }

        public ConsoleColor? BackColor { get; private set; }

        public ICommand Edit { get; }

        public ICommand Remove { get; }

        private void ParseNode()
        {
            string level = Node.GetValueAttributeValueFromChildElement(LevelName);
            string foreColor = Node.GetValueAttributeValueFromChildElement(ForeColorName);
            string backColor = Node.GetValueAttributeValueFromChildElement(BackColorName);

            if (string.IsNullOrEmpty(level) || !Log4NetUtilities.TryParseLevel(level, out Level parsedLevel))
            {
                return;
            }

            Level = parsedLevel;
            OnPropertyChanged(nameof(Level));

            if (Enum.TryParse(foreColor, out ConsoleColor foreParsed))
            {
                ForeColor = foreParsed;
                OnPropertyChanged(nameof(ForeColor));
            }

            if (Enum.TryParse(backColor, out ConsoleColor backParsed))
            {
                BackColor = backParsed;
                OnPropertyChanged(nameof(BackColor));
            }
        }

        private void EditFilterOnClick()
        {
            mShowMappingWindow(this);
        }

        private void RemoveFilterOnClick()
        {
            mRemove(this);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Node))
            {
                ParseNode();
            }

            base.OnPropertyChanged(propertyName);
        }

        public override string ToString()
        {
            return $"{Level} | {ForeColor} | {BackColor}";
        }

        public bool Equals(MappingModel other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Level, other.Level) && ForeColor == other.ForeColor && BackColor == other.BackColor;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as MappingModel);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                //I shouldn't disable this, but I am in order to facilitate easier testing.
                //Should we ever need to put these in a set, this will need to be fixed.
                // ReSharper disable NonReadonlyMemberInGetHashCode
                int hashCode = Level.GetHashCode();
                hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
                hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
                // ReSharper restore NonReadonlyMemberInGetHashCode
                return hashCode;
            }
        }
    }
}
