namespace yevgeller_v3.Models
{
    public class Article
    {
        public string URL { get; set; } = "";
        public string Description { get; set; } = "";
        //public Category CategoryName { get; set; } = new Category { Name = "Link"};
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

        public string Control()
        {
            if(this.URL != null && this.URL.Length > 0) //anchor tag
            {
                return $"<a href={this.URL}>{this.Description}</a>";

            }

            return string.Empty;
        }

        public string ConrolAsListItem()
        {
            return $"<li>{this.Control()}</li>";
        }
    }
}
