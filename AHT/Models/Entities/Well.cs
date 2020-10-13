using System.ComponentModel.DataAnnotations;

namespace AHT.Models.Entities
{
    public class Well
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public virtual Entry Entry { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        [Display(Name = "Well")]
        public string Cell { get; set; }
        [Display(Name = "Well Name")]
        public WellType Type { get; set; }
        [Display(Name = "Sample Name")]
        public string Patient { get; set; }
        [Display(Name = "Cq (∆R)")]
        public double Cq { get; set; }
    }
    public enum WellType
    {
        SSR,
        TEL,
        NTC
    }
}
