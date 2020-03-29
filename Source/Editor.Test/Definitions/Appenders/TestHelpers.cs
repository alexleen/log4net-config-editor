// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.Test.Definitions.Appenders
{
    internal static class TestHelpers
    {
        internal static int AppenderSkeletonPropertyCount => 8;

        internal static void AssertAppenderSkeletonPropertiesExist(IEnumerable<IProperty> properties)
        {
            //Single throws if the specified type is not found, which is good enough to fail the test
            //No asserts needed
            properties.Single(p => p.GetType() == typeof(TypeAttribute));
            properties.Single(p => p.GetType() == typeof(Name));
            properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Error Handler:");
            properties.Single(p => p.GetType() == typeof(Threshold));
            properties.Single(p => p.GetType() == typeof(Layout));
            properties.Single(p => p.GetType() == typeof(Editor.ConfigProperties.Filters));
            properties.Single(p => p.GetType() == typeof(IncomingRefs));
            properties.Single(p => p.GetType() == typeof(Params));
        }

        internal static int BufferingAppenderSkeletonPropertyCount => 5;

        internal static void AssertBufferingAppenderSkeletonPropertiesExist(IEnumerable<IProperty> properties)
        {
            //Single throws if the specified type is not found, which is good enough to fail the test
            //No asserts needed
            properties.Single(p => p.GetType() == typeof(NumericProperty<int>));
            properties.Single(p => p.GetType() == typeof(Fix));
            properties.Single(p => p.GetType() == typeof(BooleanPropertyBase) && ((BooleanPropertyBase)p).Name == "Lossy:");
            properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Evaluator:");
            properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Lossy Evaluator:");
        }
    }
}
