namespace DUNWorkflow.Models
{
    public class Option
    {
        public string OptionLabel { get; set; }
        public object Value { get; set; }
        public string RulebookText { get; set; }
        public string NextCodeNumber { get; set; }
        public string OptionText { get; set; }
    }

    public class GuideData
    {
        public string CodeNumber { get; set; }
        public string Header { get; set; }
        public string AnswerType { get; set; }
        public List<Option> Options { get; set; }
        public string RulebookText { get; set; }
    }
}
