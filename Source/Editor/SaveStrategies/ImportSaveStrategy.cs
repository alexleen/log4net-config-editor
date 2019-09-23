using System;
using Editor.Interfaces;

namespace Editor.SaveStrategies
{
    internal class ImportSaveStrategy : ISaveStrategy
    {
        private readonly IElementConfiguration mConfiguration;

        public ImportSaveStrategy(IElementConfiguration configuration)
        {
            mConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Execute()
        {
            mConfiguration.Log4NetNode.AppendChild(mConfiguration.OriginalNode);
        }
    }
}
