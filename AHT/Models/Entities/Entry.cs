using System;
using System.Collections.Generic;

namespace AHT.Models.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public DateTime DateTime { get; set; }
        public int ResultId { get; set; }
        public virtual Result Result { get; set; }
        public virtual ICollection<Well> Wells { get; } = new List<Well>();
    }
}
