﻿namespace yevgeller_v3.Models
{
    public class Article
    {
        public string URL { get; set; } = "";
        public string Description { get; set; } = "";
        public Category CategoryName { get; set; } = new Category { Name = "Link"};

    }
}