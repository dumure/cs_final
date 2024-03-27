using System.Text.Json;

internal class QuizzesDatabase
{
    private List<Quiz> _quizzes;

    public QuizzesDatabase()
    {
        _quizzes = new List<Quiz>();
    }
    private void Serialize()
    {
        using (FileStream file = new FileStream("quizzes.json", FileMode.OpenOrCreate, FileAccess.Write))
        {
            JsonSerializer.Serialize(file, _quizzes);
        }
    }
    private void Deserialize()
    {
        using (FileStream file = new FileStream("quizzes.json", FileMode.OpenOrCreate, FileAccess.Read))
        {
            if (file.Length > 0)
            {
                _quizzes = JsonSerializer.Deserialize<List<Quiz>>(file)!;
            }
        }
    }
    public List<Quiz> GetQuizzes()
    {
        Deserialize();
        return _quizzes;
    }
    public void UpdateQuizzes(List<Quiz> quizzes)
    {
        _quizzes = quizzes;
        Serialize();
    }
}