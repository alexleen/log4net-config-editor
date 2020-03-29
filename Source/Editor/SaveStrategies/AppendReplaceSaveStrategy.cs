// Copyright © 2018 Alex Leendertsen

using System;
using Editor.Interfaces;

namespace Editor.SaveStrategies
{
    internal class AppendReplaceSaveStrategy : ISaveStrategy
    {
        private readonly IElementConfiguration mConfiguration;
        private readonly bool mForceAppend;

        public AppendReplaceSaveStrategy(IElementConfiguration configuration, bool forceAppend)
        {
            mConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            mForceAppend = forceAppend;
        }

        public void Execute()
        {
            if (mForceAppend || mConfiguration.OriginalNode == null)
            {
                //New node - add
                mConfiguration.Log4NetNode.AppendChild(mConfiguration.NewNode);
            }
            else
            {
                //Edit - replace
                mConfiguration.Log4NetNode.ReplaceChild(mConfiguration.NewNode, mConfiguration.OriginalNode);
            }
        }
    }
}
