using yevgeller_v3.Infrastructure;

namespace yevgeller_v3.Models.BdpqTestingFramework
{
    public interface IBdpqTestingRepository
    {
        List<TestItem> AllTestItems();
        List<TestItem> TestItemsByCategory();
        List<TestItem> TestItemsByCategory(QuestionCategory category);
        List<TestItem> GetItemsForATest();
    }
    public class BdpqTestingRepository : IBdpqTestingRepository
    {
        public BdpqTestingRepository()
        {
            GenerateAllTestItems();
            GenerateTestItemsForATest();
            DefaultSelectedCategory = QuestionCategory.LetterToPicture;
        }
        public QuestionCategory DefaultSelectedCategory { get; private set; }
        private List<TestItem> itemsBank { get; set; } = new List<TestItem>();
        public List<TestItem> AllTestItems() => itemsBank;
        public List<TestItem> TestItemsByCategory(QuestionCategory category) => itemsBank.Where(i => i.QuestionCategory == category).ToList();
        public List<TestItem> TestItemsByCategory() => itemsBank.Where(x => x.QuestionCategory == DefaultSelectedCategory).ToList();
        public List<TestItem> GetItemsForATest() => ItemsForATest;
        
        public List<TestItem> ItemsForATest { get; set; } = new List<TestItem>();

        private void GenerateAllTestItems()
        {

            if (itemsBank.Count > 0)
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

                        itemsBank.Add(t);
                    }
                }
            }
        }

        private void GenerateTestItemsForATest()
        {
            ItemsForATest.Clear();

            ItemsForATest.Add(RandomTestItemOfAType("b"));
            ItemsForATest.Add(RandomTestItemOfAType("d"));
            ItemsForATest.Add(RandomTestItemOfAType("p"));
            ItemsForATest.Add(RandomTestItemOfAType("q"));
            ItemsForATest.Shuffle();
        }
        private string ExtractWordFromIconName(string iconName)
        {
            var temp = iconName.Replace("fa-", "");
            if (temp.IndexOf("-") == -1)
                return temp;

            return temp.Substring(0, temp.IndexOf("-"));
        }
        private TestItem RandomTestItemOfAType(string type, QuestionCategory category = QuestionCategory.LetterToPicture)
        {
            var q = TestItemsByCategory(category)
                .Where(x => x.QuestionType == type);

            if (q.Count() > 0)
            {
                Random r = new Random();
                return q.ElementAt(r.Next(q.Count()));
            }

            throw new Exception("There were no TestItems of QuestionType " + type);
        }
    }


}
