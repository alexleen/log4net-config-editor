// Copyright © 2018 Alex Leendertsen

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using log4net.Core;

namespace Editor.Models
{
    public sealed class FixModel : INotifyPropertyChanged, IEquatable<FixModel>
    {
        public FixModel(FixFlags flag, bool performanceImpact, string description = null)
        {
            Flag = flag;
            PerformanceImpact = performanceImpact;
            Description = description;
        }

        public FixFlags Flag { get; }

        public bool PerformanceImpact { get; }

        public string Description { get; }

        private bool mEnabled;

        public bool Enabled
        {
            get => mEnabled;
            set
            {
                if (mEnabled == value)
                {
                    return;
                }

                mEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(FixModel other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Flag == other.Flag && PerformanceImpact == other.PerformanceImpact && string.Equals(Description, other.Description);
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

            return obj is FixModel model && Equals(model);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)Flag;
                hashCode = (hashCode * 397) ^ PerformanceImpact.GetHashCode();
                hashCode = (hashCode * 397) ^ Description.GetHashCode();
                return hashCode;
            }
        }
    }
}
