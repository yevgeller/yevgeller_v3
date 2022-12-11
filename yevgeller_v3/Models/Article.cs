namespace yevgeller_v3.Models
{
    public class Article
    {
        public string URL { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GroupHeader { get; set; } = "Links";
        public string Category { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public Article()
        {
            this.URL = "#";
            this.Description = "Missing Description";
            this.GroupHeader = "Unset category header";
            this.Category = "Unset category";
        }

        public Article(string uRL, string description, string categoryHeader, string category)
        {
            URL = uRL;
            Description = description;
            GroupHeader = categoryHeader;
            Category = category;
        }
    }
}
