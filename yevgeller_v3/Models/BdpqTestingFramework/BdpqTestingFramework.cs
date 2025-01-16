using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using yevgeller_v3.Infrastructure;

namespace yevgeller_v3.Models.BdpqTestingFramework
{

    public interface IBdpqTestingFramework
    {
        List<TestItem> GetTestItems();
        List<TestItem> GetTestItemsByCategory();
        List<TestItem> GetTestItemsByCategory(QuestionCategory category);
        List<TestItem> GetTestItemsForATest();
        TestQuestion GetNextQuestion();
        TestQuestion ProcessAnswer(string answer);
        int GetQuestionCount();
    }

    public class BdpqTestingFramework : IBdpqTestingFramework
    {
        private readonly IBdpqTestingRepository repository;
        private int count = 0;
        private TestQuestion testQuestion = new();

        public BdpqTestingFramework(IBdpqTestingRepository _repository)
        {
            this.repository = _repository;
            count = 0;
        }

        public List<TestItem> GetTestItems() => repository.AllTestItems();
        public List<TestItem> GetTestItemsByCategory() => repository.TestItemsByCategory();
        public List<TestItem> GetTestItemsByCategory(QuestionCategory category) => repository.TestItemsByCategory(category);
        public List<TestItem> GetTestItemsForATest() => repository.GetItemsForATest();

        public TestQuestion GetNextQuestion()
        {
            testQuestion = new TestQuestion
            {
                Question = "b",
                Answers = new()
            };

            testQuestion.Answers.Add(new TestAnswer { Answer = "b", IsCorrect = true, IsDisabled = false });
            testQuestion.Answers.Add(new TestAnswer { Answer = "d", IsCorrect = false, IsDisabled = false });
            testQuestion.Answers.Add(new TestAnswer { Answer = "p", IsCorrect = false, IsDisabled = false });
            testQuestion.Answers.Add(new TestAnswer { Answer = "q", IsCorrect = false, IsDisabled = false });
            testQuestion.Answers.Shuffle();

            return testQuestion;
            /*
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
            */
        }

        public TestQuestion ProcessAnswer(string answer)
        {
            if (testQuestion.Answers.Count == 0 || answer == string.Empty) //new request
                return GetNextQuestion();

            var a = testQuestion.Answers
                .Where(x => String.Compare(x.Answer, answer, StringComparison.OrdinalIgnoreCase) == 0)
                .FirstOrDefault();
            if (a == null)
                throw new Exception($"No answer \"{answer}\" is found for this question");

            if (a.IsCorrect)
                testQuestion = GetNextQuestion();
            else
                a.IsDisabled = true;

            return testQuestion;
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
