internal class Question
{
    public string? QuestionText { get; set; }
    public List<Answer> Answers { get; set; }
    public Question()
    {
        Answers = new List<Answer>();
    }
    public override string ToString()
    {
        string result = $"{QuestionText}\n";
        for (int i = 0; i < Answers.Count; i++)
        {
            result += $"{i + 1}) {Answers[i].AnswerText}\t";
        }
        return result;
    }
}