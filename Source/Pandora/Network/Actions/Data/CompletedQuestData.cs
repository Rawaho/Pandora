using System;

namespace Pandora.Network.Actions.Data
{
    [Serializable]
    public class CompletedQuestData : DataSSetAction
    {
        [ServerActionField("completionDate")]
        public uint CompletionDate { get; set; }

        public CompletedQuestData() : base("completedQuests", "COMPLETED_QUEST") { }
    }
}
