namespace MockForge.Models
{
    public class Profession
    {
        public required string Company { get; set; }
        public required string Department { get; set; }
        public string Rank { get; set; } = string.Empty;
    }
}
