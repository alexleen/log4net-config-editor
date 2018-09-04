// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using Editor.ConfigProperties;
using Editor.Interfaces;

namespace Editor.Test.Definitions.Appenders
{
    internal static class TestHelpers
    {
        internal static void AssertDefaultPropertiesExist(IEnumerable<IProperty> properties)
        {
            //Single throws if the specified type is not found, which is good enough to fail the test
            //No asserts needed
            properties.Single(p => p.GetType() == typeof(TypeAttribute));
            properties.Single(p => p.GetType() == typeof(Name));
            properties.Single(p => p.GetType() == typeof(Threshold));
            properties.Single(p => p.GetType() == typeof(Layout));
            properties.Single(p => p.GetType() == typeof(Editor.ConfigProperties.Filters));
            properties.Single(p => p.GetType() == typeof(IncomingRefs));
            properties.Single(p => p.GetType() == typeof(Params));
        }
    }
}
