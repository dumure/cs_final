using System.Diagnostics.Contracts;

internal class Quiz
{
    public string? QuizName { get; set; }
    public Category QuizCategory { get; set; }
    public List<Question> Questions { get; set; }
    public List<Progress> Progresses { get; set; }
    public Quiz()
    {
        Questions = new List<Question>();
        Progresses = new List<Progress>();
    }
    public override string ToString()
    {
        string? result = $"{QuizName} - Категория: ";
        switch (QuizCategory)
        {
            case Category.MATH:
                {
                    result += "Математика";
                    break;
                }
            case Category.BIOLOGY:
                {
                    result += "Биология";
                    break;
                }
            case Category.GEOGRAPHY:
                {
                    result += "География";
                    break;
                }
            case Category.PHYSICS:
                {
                    result += "Физика";
                    break;
                }
            case Category.LITERATURE:
                {
                    result += "Литература";
                    break;
                }
            case Category.MIXED:
                {
                    result += "Смешанный";
                    break;
                }
        }
        return result;
    }
}