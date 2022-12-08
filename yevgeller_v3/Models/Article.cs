namespace yevgeller_v3.Models
{
    public class Article
    {
        public string URL { get; set; } = "";
        public string Description { get; set; } = "";
        //public Category CategoryName { get; set; } = new Category { Name = "Link"};
        public string CategoryHeader { get; set; } = "Links";
        public string Category { get; set; } = string.Empty;

        public Article()
        {
            this.URL = "#";
            this.Description = "Missing Description";
            this.CategoryHeader = "Unset category header";
            this.Category = "Unset category";
        }
    
    }
}
