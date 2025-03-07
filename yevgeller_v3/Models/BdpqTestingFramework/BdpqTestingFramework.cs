﻿using System.Diagnostics.Metrics;
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
        List<TestItemStatistic> GetTestItemStatistics();
        void ResetStatistics();
        int GetQuestionCount();
    }

    public class BdpqTestingFramework : IBdpqTestingFramework
    {
        private readonly IBdpqTestingRepository repository;
        private int count = 0;
        private TestQuestion testQuestion = new();
        private List<TestItemStatistic> CurrentTestItemStatistics = new();

        public BdpqTestingFramework(IBdpqTestingRepository _repository)
        {
            this.repository = _repository;
            InitializeStats();
            count = 0;
        }

        private void InitializeStats()
        {
            CurrentTestItemStatistics = new();

            var distinctQuestionTypes = repository
                .AllTestItems()
                .Select(x => x.QuestionType)
                .Distinct();
            foreach (var questionType in distinctQuestionTypes)
            {
                CurrentTestItemStatistics.Add(new TestItemStatistic { Item = questionType });
            }
        }

        public List<TestItem> GetTestItems() => repository.AllTestItems();
        public List<TestItem> GetTestItemsByCategory() => repository.TestItemsByCategory();
        public List<TestItem> GetTestItemsByCategory(QuestionCategory category) => repository.TestItemsByCategory(category);
        public List<TestItem> GetTestItemsForATest() => repository.GetItemsForATest();
        public List<TestItemStatistic> GetTestItemStatistics() => CurrentTestItemStatistics;
        public void ResetStatistics() => InitializeStats();
        public TestQuestion GetNextQuestion()
        {
            testQuestion = repository.GenerateTestQuestion();
            return testQuestion;
        }

        public TestQuestion ProcessAnswer(string answer)
        {
            if (testQuestion.Answers.Count == 0)  //new request
                return GetNextQuestion();

            if (answer == string.Empty)
                return testQuestion;

            var a = testQuestion.Answers
                .Where(x => String.Compare(x.Answer, answer, StringComparison.OrdinalIgnoreCase) == 0)
                .FirstOrDefault();
            if (a == null)
                throw new Exception($"No answer \"{answer}\" is found for this question");

            if (a.IsCorrect)
            {
                ProcessTestItemStatistic(testQuestion.Question, true);
                testQuestion = GetNextQuestion();
            }
            else
            {
                ProcessTestItemStatistic(testQuestion.Question, false);
                a.IsDisabled = true;
            }
            return testQuestion;
        }

        private void ProcessTestItemStatistic(string question, bool isCorrect)
        {
            var stat = this.CurrentTestItemStatistics.FirstOrDefault(x => x.Item == question);
            if(stat == null)
            {
                CurrentTestItemStatistics.Add(new TestItemStatistic { Item = question, Total = 1, Correct = (isCorrect== true? 1 : 0) });
            }
            else
            {
                stat.Total++;
                stat.Correct += (isCorrect ? 1 : 0);
            }
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

    public class TestItemStatistic
    {
        public string Item { get; set; } = string.Empty;
        public int Correct { get; set; }
        public int Total { get; set; }

        public bool IsPerfect() => Total == 0 || (double)Correct / (double)Total > 0.9;
    }
}
