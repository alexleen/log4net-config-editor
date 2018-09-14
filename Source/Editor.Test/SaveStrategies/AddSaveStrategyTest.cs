// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml;
using Editor.Interfaces;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.SaveStrategies;
using NUnit.Framework;

namespace Editor.Test.SaveStrategies
{
    [TestFixture]
    public class AddSaveStrategyTest
    {
        [Test]
        public void Ctor_ShouldThrow_WhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AddSaveStrategy<ModelBase>(null, model => { }, new XmlDocument().CreateElement("element")));
        }

        [Test]
        public void Ctor_ShouldThrow_WhenAddCallbackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AddSaveStrategy<ModelBase>(new RendererModel(null), null, new XmlDocument().CreateElement("element")));
        }

        [Test]
        public void Ctor_ShouldThrow_WhenNewElementIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AddSaveStrategy<ModelBase>(new RendererModel(null), model => { }, null));
        }

        [Test]
        public void Execute_ShouldAddModel_AndShouldAssignNode_WhenModelNodeIsNull()
        {
            ModelBase model = new RendererModel(null);
            XmlElement newElement = new XmlDocument().CreateElement("element");
            bool addCalled = false;

            ISaveStrategy sut = new AddSaveStrategy<ModelBase>(model, modelParam => { addCalled = true; }, newElement);
            sut.Execute();

            Assert.IsTrue(addCalled);
            Assert.AreSame(newElement, model.Node);
        }

        [Test]
        public void Execute_ShouldNotAddModel_AndShouldAssignNode_WhenModelNodeIsNotNull()
        {
            ModelBase model = new RendererModel(null);
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement newElement = xmlDoc.CreateElement("element");
            model.Node = xmlDoc.CreateElement("oldElement");
            bool addCalled = false;

            ISaveStrategy sut = new AddSaveStrategy<ModelBase>(model, modelParam => { addCalled = true; }, newElement);
            sut.Execute();

            Assert.IsFalse(addCalled);
            Assert.AreSame(newElement, model.Node);
        }
    }
}
