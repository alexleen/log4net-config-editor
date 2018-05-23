﻿// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.Descriptors
{
    public class LockingModelDescriptor
    {
        public static readonly LockingModelDescriptor Exclusive, Minimal, InterProcess;
        private static readonly IDictionary<string, LockingModelDescriptor> sDescriptorsByTypeNamespace;

        static LockingModelDescriptor()
        {
            Exclusive = new LockingModelDescriptor("Exclusive", "log4net.Appender.FileAppender+ExclusiveLock");
            Minimal = new LockingModelDescriptor("Minimal", "log4net.Appender.FileAppender+MinimalLock");
            InterProcess = new LockingModelDescriptor("Inter-Process", "log4net.Appender.FileAppender+InterProcessLock");

            sDescriptorsByTypeNamespace = new Dictionary<string, LockingModelDescriptor>
            {
                { Exclusive.TypeNamespace, Exclusive },
                { Minimal.TypeNamespace, Minimal },
                { InterProcess.TypeNamespace, InterProcess }
            };
        }

        private LockingModelDescriptor(string name, string typeNamespace)
        {
            Name = name;
            TypeNamespace = typeNamespace;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out LockingModelDescriptor appender)
        {
            if (typeNamespace == null)
            {
                appender = null;
                return false;
            }

            return sDescriptorsByTypeNamespace.TryGetValue(typeNamespace, out appender);
        }

        public string Name { get; }

        public string TypeNamespace { get; }
    }
}