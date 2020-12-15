// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.Interfaces
{
    public interface IElement
    {
        /// <summary>
        /// Element name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Set of attributes for the element.
        /// </summary>
        IEnumerable<(string attrName, string attrValue)> Attributes { get; }
    }
}
