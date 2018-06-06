using log4net.Core;
using System;
using System.ComponentModel;
using System.Linq;

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

        bool mEnabled;

        public bool Enabled
        {
            get
            {
                return mEnabled;
            }
            set
            {
                if (mEnabled == value)
                {
                    return;
                }

                mEnabled = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Enabled)));
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
