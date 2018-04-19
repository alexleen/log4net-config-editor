// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Xml;
using Editor.Models;

namespace Editor.Utilities
{
    public static class XmlUtilities
    {
        public static IEnumerable<RefModel> FindAppenderRefs(XmlNode log4NetNode, string appenderName)
        {
            XmlNodeList appenderRefs = log4NetNode.SelectNodes("//appender-ref");

            foreach (XmlNode appenderRef in appenderRefs)
            {
                if (appenderRef.Attributes?["ref"].Value == appenderName)
                {
                    yield return new RefModel(appenderRef);
                }
            }
        }
    }
}
