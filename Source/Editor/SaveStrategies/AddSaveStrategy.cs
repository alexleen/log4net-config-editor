// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml;
using Editor.Interfaces;
using Editor.Models.Base;

namespace Editor.SaveStrategies
{
    internal class AddSaveStrategy<TModelType> : ISaveStrategy where TModelType : ModelBase
    {
        private readonly TModelType mModel;
        private readonly Action<TModelType> mAdd;
        private readonly XmlElement mNewElement;

        public AddSaveStrategy(TModelType model, Action<TModelType> add, XmlElement newElement)
        {
            mModel = model ?? throw new ArgumentNullException(nameof(model));
            mAdd = add ?? throw new ArgumentNullException(nameof(add));
            mNewElement = newElement ?? throw new ArgumentNullException(nameof(newElement));
        }

        public void Execute()
        {
            if (mModel.Node == null)
            {
                mAdd(mModel);
            }

            mModel.Node = mNewElement;
        }
    }
}
