﻿using yevgeller_v3.Infrastructure;

namespace yevgeller_v3.Models.BdpqTestingFramework
{
    public interface IBdpqTestingRepository
    {
        List<TestItem> AllTestItems();
        List<TestItem> TestItemsByCategory();
        List<TestItem> TestItemsByCategory(QuestionCategory category);
        List<TestItem> GetItemsForATest();
        TestQuestion GenerateTestQuestion();
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

        public TestQuestion GenerateTestQuestion()
        {
            var items = new List<TestItem> {
             TestItemsByCategory().Where(x => x.QuestionType == "b").ToList().RandomElement(),
             TestItemsByCategory().Where(x => x.QuestionType == "d").ToList().RandomElement(),
             TestItemsByCategory().Where(x => x.QuestionType == "p").ToList().RandomElement(),
             TestItemsByCategory().Where(x => x.QuestionType == "q").ToList().RandomElement()
                };

            items.Shuffle();

            Random r = new Random();
            int correctPosition = r.Next(0, 4);

            TestQuestion tq = new TestQuestion
            {
                Question = items.ElementAt(correctPosition).Question
            };

            for (int i = 0; i < items.Count(); i++)
            {
                tq.Answers.Add(new TestAnswer { Answer = items[i].AnswerWord, IsCorrect = i == correctPosition, IsDisabled = false });
            }
            return tq;
        }
        private void GenerateAllTestItems()
        {

            if (itemsBank.Count > 0)
                return;

            int idCounter = 1;
            var categories = Enum.GetValues(typeof(QuestionCategory));

            var itemsByLetter = new Dictionary<string, string[]>
            {
                { "b", new[] { "fa-book", "fa-baby", "fa-bath", "fa-bed", "fa-bell", "fa-box", "fa-brush", "fa-bug", "fa-bus", "fa-bacon" } },
                { "d", new[] { "fa-door-open", "fa-drum", "fa-dog", "fa-dollar-sign", "fa-dove", "fa-dragon", "fa-dot-circle" } },
                { "p", new[] { "fa-pencil-alt", "fa-pizza-slice", "fa-paper-plane", "fa-pen", "fa-paw", "fa-phone", "fa-plug", "fa-plus", "fa-poop", "fa-puzzle-piece" } },
                { "q", new[] { "fa-question-circle", "queen", "quick", "quiz", "quest", "question" } }
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
