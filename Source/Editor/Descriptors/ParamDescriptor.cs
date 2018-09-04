// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Utilities;

namespace Editor.Descriptors
{
    internal class ParamDescriptor : DescriptorBase
    {
        public static readonly ParamDescriptor Param;

        static ParamDescriptor()
        {
            Param = new ParamDescriptor();
        }

        private ParamDescriptor()
            : base("Param", Log4NetXmlConstants.Param)
        {
        }
    }
}
