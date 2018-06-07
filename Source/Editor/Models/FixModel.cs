// Copyright © 2018 Alex Leendertsen

using System.ComponentModel;
using System.Runtime.CompilerServices;
using log4net.Core;

namespace Editor.Models
{
    public class FixModel : INotifyPropertyChanged
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
