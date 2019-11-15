using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Lossy : BooleanPropertyBase
    {
        public Lossy(ReadOnlyCollection<IProperty> container) 
            : base(container, "Lossy:", "lossy", false, false)
        {
        }
    }
}
