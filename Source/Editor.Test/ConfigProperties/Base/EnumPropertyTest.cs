using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
	[TestFixture]
	public class EnumPropertyTest
	{
		private EnumProperty<MailPriority> mSut;

		[SetUp]
		public void SetUp()
		{
			mSut = new EnumProperty<MailPriority>(new ReadOnlyCollection<IProperty>(new IProperty[] { }), "name", 100, "elementName");
		}

		[Test]
		public void Ctor_ShouldUseStringValues()
		{
			CollectionAssert.AreEquivalent(Enum.GetNames(typeof(MailPriority)), mSut.Values);
		}

		[Test]
		public void Ctor_ShouldSelectFirstValue()
		{
			Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
		}

		[Test]
		public void Load_ShouldNotLoadValuesItCannotParse()
		{
			XmlDocument xmlDoc = new XmlDocument();

			XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
			valueAttribute.Value = "whatev"; // Not a valid MailPriority enum value

			XmlElement encodingElement = xmlDoc.CreateElement("elementName");
			encodingElement.Attributes.Append(valueAttribute);

			XmlElement appenderElement = xmlDoc.CreateElement("appender");
			appenderElement.AppendChild(encodingElement);

			mSut.Load(appenderElement);

			Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
		}

		[Test]
		public void Load_ShouldLoadValue()
		{
			XmlDocument xmlDoc = new XmlDocument();

			XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
			valueAttribute.Value = MailPriority.High.ToString();

			XmlElement encodingElement = xmlDoc.CreateElement("elementName");
			encodingElement.Attributes.Append(valueAttribute);

			XmlElement appenderElement = xmlDoc.CreateElement("appender");
			appenderElement.AppendChild(encodingElement);

			mSut.Load(appenderElement);

			Assert.AreEqual(MailPriority.High.ToString(), mSut.SelectedValue);
		}

		[Test]
		public void Save_ShouldSave()
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement appenderElement = xmlDoc.CreateElement("appender");

			mSut.SelectedValue = MailPriority.High.ToString();
			mSut.Save(xmlDoc, appenderElement);

			XmlNode element = appenderElement.SelectSingleNode("elementName");
			Assert.IsNotNull(element);
			Assert.AreEqual(mSut.SelectedValue, element.Attributes["value"].Value);
		}
	}
}
