using System;

namespace Domain.Entities.Supplies
{
    public class SupplyInfo : Entity
    {
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public int OldSupplyHistoryId { get; set; }

        public bool NewImport { get; set; }
    }
}