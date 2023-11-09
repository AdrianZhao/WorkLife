namespace WorkLife.Models
{
    public class JobIndustryArea
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int IndustryAreaId { get; set; }
        public IndustryArea IndustryArea { get; set; }
    }
}
