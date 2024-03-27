internal class Progress
{
    public string? UserLogin { get; set; }
    public int CorrectAnswersCount { get; set; }
    public int MaxAnswersCount { get; set; }
    public DateTime CreationDate { get; set; }
    public override string ToString()
    {
        return $"{UserLogin} - {CorrectAnswersCount}/{MaxAnswersCount}";
    }
}