// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using System;

namespace Editor.Windows
{
    internal class AppendReplaceSaveStrategy : ISaveStrategy
    {
        private readonly IElementConfiguration mConfiguration;

        public AppendReplaceSaveStrategy(IElementConfiguration configuration)
        {
            mConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Execute()
        {
            if (mConfiguration.OriginalNode == null)
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
