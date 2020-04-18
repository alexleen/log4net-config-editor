// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.Interfaces
{
    public interface IValueResult
    {
        IEnumerable<string> ActualElementNames { get; }
        
        string ActualAttributeName { get; }
        
        string AttributeValue { get; }
    }
}
