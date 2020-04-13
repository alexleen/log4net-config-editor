// Copyright © 2020 Alex Leendertsen

namespace Editor.Interfaces
{
    public interface IValueResult
    {
        string ActualElementName { get; }
        
        string ActualAttributeName { get; }
        
        string AttributeValue { get; }
    }
}
