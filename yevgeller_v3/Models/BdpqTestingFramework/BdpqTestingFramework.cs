namespace yevgeller_v3.Models.BdpqTestingFramework
{

    public class Repository
    {
        private int count = 0;
        public List<TestItem> Items { get; set; }
        public Repository() {

            count = 0;
            if(Items == null) { Items = new List<TestItem>(); }

            Items.Add(new TestItem { Id = 1, Question = "b", Answer = "bee" });
            Items.Add(new TestItem { Id = 2, Question = "d", Answer = "dee" });
            Items.Add(new TestItem { Id = 3, Question = "p", Answer = "pee" });
            Items.Add(new TestItem { Id = 4, Question = "q", Answer = "queue" });
        }

        public List<TestItem> GetTestItems() => Items;

        public TestQuestion GetNextQuestion()
        {
            count++;
            if (count % 2 == 0)
            {
                return new TestQuestion
                {
                    Question = "b",
                    Answers = new List<TestAnswer> {
                        new TestAnswer { Answer = "deer", IsCorrect = false },
                        new TestAnswer { Answer = "peer", IsCorrect = false },
                        new TestAnswer { Answer = "bee", IsCorrect = true },
                        new TestAnswer { Answer = "queue", IsCorrect = false },
                    }
                };

            }
            else
            {
                return new TestQuestion
                {
                    Question = "d",
                    Answers = new List<TestAnswer>
                    {
                        new TestAnswer { Answer = "deer", IsCorrect = true },
                        new TestAnswer { Answer = "peer", IsCorrect = false },
                        new TestAnswer { Answer = "bee", IsCorrect = false },
                        new TestAnswer { Answer = "queue", IsCorrect = false },
                    }
                };
            }
        }
    }

    public class TestItem
    {
        public int Id { get; set; }
        public string Question { get; set; } = "?";
        public string Answer { get; set; } = "!";
    }

    public class TestQuestion
    {
        public string Question { get; set; } = "?";

        public List<TestAnswer> Answers { get; set; } = new List<TestAnswer>();
    }

    public class TestAnswer
    {
        public string Answer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
