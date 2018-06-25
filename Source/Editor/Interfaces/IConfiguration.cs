// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Interfaces
{
    public interface IConfiguration
    {
        XmlDocument ConfigXml { get; }

        XmlNode Log4NetNode { get; }
    }

    public interface IElementConfiguration : IConfiguration
    {
        XmlNode OriginalNode { get; }

        XmlNode NewNode { get; }
    }

    /// <summary>
    /// Experimental interface that "hides" the various System.Xml properties of its parents.
    /// </summary>
    public interface IAbstractedConfiguration : IElementConfiguration
    {
        void AddNewAppender(XmlNode newAppender);

        void ReplaceAppender(XmlNode oldAppender, XmlNode newAppender);
    }
}
