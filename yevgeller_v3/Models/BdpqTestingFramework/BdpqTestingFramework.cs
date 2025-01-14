using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;

namespace yevgeller_v3.Models.BdpqTestingFramework
{

    public interface IBdpqTestingRepository
    {
        List<TestItem> GetTestItems();
        List<TestItem> GetTestItemsByCategory();
        TestQuestion GetNextQuestion();
        TestQuestion ProcessAnswer(string answer);
        int GetQuestionCount();
    }

    public class Repository : IBdpqTestingRepository
    {
        private int count = 0;
        public List<TestItem> Items { get; set; } = new List<TestItem>();
        public List<TestItem> ItemsByCategory { get; set; } = new List<TestItem> { };
        public Repository()
        {
            count = 0;
            GenerateTestItems();
            GenerateTestItemsByCategory(QuestionCategory.LetterToPicture);
        }
        private void GenerateTestItems()
        {

            if (Items.Count > 0)
                return;

            int idCounter = 1;
            var categories = Enum.GetValues(typeof(QuestionCategory));

            var itemsByLetter = new Dictionary<string, string[]>
            {
                { "b", new[] { "fa-book", "fa-bicycle", "fa-bread-slice" } },
                { "d", new[] { "fa-door-open", "fa-drum", "fa-dog" } },
                { "p", new[] { "fa-pencil-alt", "fa-pizza-slice", "fa-paper-plane" } },
                { "q", new[] { "fa-question-circle", "fa-quidditch", "fa-qrcode" } }
            };

            foreach (QuestionCategory category in categories)
            {
                foreach (var letter in itemsByLetter.Keys)
                {
                    var icons = itemsByLetter[letter];
                    foreach (var icon in icons)
                    {
                        var t = new TestItem
                        {
                            Id = idCounter++,
                            QuestionType = letter,
                            Question = letter,
                            Answer = letter,
                            AnswerWord = ExtractWordFromIconName(icon),
                            HintIcon = icon,
                            QuestionCategory = category
                        };

                        Items.Add(t);
                    }
                }
            }
        }

        private void GenerateTestItemsByCategory(QuestionCategory category)
        {
            ItemsByCategory = Items.Where(x => x.QuestionCategory == category).ToList();
        }

        private string ExtractWordFromIconName(string iconName)
        {
            var temp = iconName.Replace("fa-", "");
            if (temp.IndexOf("-") == -1)
                return temp;

            return temp.Substring(0, temp.IndexOf("-"));
        }
        public List<TestItem> GetTestItems() => Items;
        public List<TestItem> GetTestItemsByCategory() => ItemsByCategory;

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

        public TestQuestion ProcessAnswer(string answer)
        {
            throw new NotImplementedException();
        }

        public int GetQuestionCount() => count;
    }

    public enum QuestionCategory
    {
        LetterToPicture,
        LetterToWord,
        PictureToLetter,
        WordToLetter
    }

    public class TestItem
    {
        public int Id { get; set; }
        public string QuestionType { get; set; } = ""; //b,d,p,q
        public string Question { get; set; } = "?"; //q
        public string Answer { get; set; } = "!"; //q
        public string AnswerWord { get; set; } = "!"; //question
        public string HintIcon { get; set; } = "fa-question";
        public QuestionCategory QuestionCategory { get; set; }

        public string IconAsHtml() => "<i class='fas " + HintIcon + "'></i>";
        public override string ToString()
        {
            return $"Id: {Id}, Type: {QuestionType}, ?: {Question}, !: {Answer}, Word: {AnswerWord}, Icon: {HintIcon}, Cat: {QuestionCategory} ";
        }
    }

    public class TestQuestion
    {
        public string Question { get; set; } = "?";
        public List<TestAnswer> Answers { get; set; } = new List<TestAnswer>();
    }

    public class TestAnswer
    {
        public string Answer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
    }
}
