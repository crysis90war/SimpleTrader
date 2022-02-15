namespace SimpleTrader.Domain.Models
{
    public enum MajorIndexType
    {
        DowJones,
        Nasdaq,
        SP500
    }

    public class MajorIndex
    {
        public double Changes { get; set; }
        public double Price { get; set; }
        public MajorIndexType Type { get; set; }
        public string IndexName { get; set; }
    }
}
