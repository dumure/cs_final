using System;
using System.Reflection;
using System.Text.RegularExpressions;

internal class QuizzerSession : IDisposable
{
    private User _currentUser;
    private bool _isAdmin;
    private List<Quiz> _quizzes;
    public QuizzerSession(User user)
    {
        QuizzesDatabase database = new QuizzesDatabase();
        _quizzes = database.GetQuizzes();
        _currentUser = user;
        if (_currentUser.Login == "admin" && _currentUser.Password == EncoderSHA256.ToSHA256("admin"))
        {
            _isAdmin = true;
        }
        else
        {
            _isAdmin = false;
        }
    }

    public void StartUsing()
    {
        MainMenu();
    }

    public void Dispose()
    {
        QuizzesDatabase database = new QuizzesDatabase();
        database.UpdateQuizzes(_quizzes);
    }

    private void MainMenu()
    {
        if (_isAdmin)
        {
            bool flag = true;
            Console.WriteLine($"Здравствуйте, Администратор! Вы можете создавать, удалять и редактировать викторины. Выберите следующее действие:");
            while (flag)
            {
                while (flag)
                {
                    Console.WriteLine("1. Управлять викторинами;");
                    Console.WriteLine("2. Выход их аккаунта;");
                    Console.Write(" >>> ");
                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.Clear();
                        switch (choice)
                        {
                            case 1:
                                {
                                    ManageQuizzes();
                                    break;
                                }
                            case 2:
                                {
                                    flag = false;
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Ввод не подходит нашим параметрам! Попробуйте заново.");
                                    continue;
                                }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
                    }
                }
            }
        }
        else
        {
            bool flag = true;
            Console.WriteLine($"Здравствуй, {_currentUser.Login}! Выбери следующее действие:");
            while (flag)
            {
                Console.WriteLine("1. Все викторины;");
                Console.WriteLine("2. История;");
                Console.WriteLine("3. Настройки аккаунта;");
                Console.WriteLine("4. Выход их аккаунта;");
                Console.Write(" >>> ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            {
                                Quizzes();
                                break;
                            }
                        case 2:
                            {
                                History();
                                break;
                            }
                        case 3:
                            {
                                AccountOptions();
                                break;
                            }
                        case 4:
                            {
                                flag = false;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Ввод не подходит нашим параметрам! Попробуйте заново.");
                                continue;
                            }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
                }
            }
        }
    }

    private void ManageQuizzes()
    {
        while (true)
        {
            Console.WriteLine("Все викторины:");
            if (_quizzes.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < _quizzes.Count; i++)
                {
                    Console.Write($"{i + 1}. {_quizzes[i]}");
                    if (i == _quizzes.Count - 1)
                    {
                        Console.WriteLine('.');
                    }
                    else
                    {
                        Console.WriteLine(';');
                    }
                }
            }
            Console.WriteLine("Чтобы создать новую викторину введите create;");
            Console.WriteLine("Чтобы редактировать викторину введите edit НОМЕР_ВИКТОРИНЫ;");
            Console.WriteLine("Чтобы удалить викторину введите delete НОМЕР_ВИКТОРИНЫ;");
            Console.WriteLine("Чтобы вернуться назад введите 0:");
            Console.Write(" >>> ");
            string? command = Console.ReadLine()?.ToLower();
            Console.Clear();
            command ??= string.Empty;
            if (command == "0")
            {
                return;
            }
            else if (command.Split(' ')[0] == "create")
            {
                CreateQuiz();
                continue;
            }
            else if (command.Split(' ')[0] == "edit" && int.TryParse(command.Split(' ')[1], out int choiceEdit))
            {
                if (choiceEdit >= 1 && choiceEdit <= _quizzes.Count)
                {
                    EditQuiz(choiceEdit - 1);
                    continue;
                }
            }
            else if (command.Split(' ')[0] == "delete" && int.TryParse(command.Split(' ')[1], out int choiceRemove))
            {
                if (choiceRemove >= 1 && choiceRemove <= _quizzes.Count)
                {
                    _quizzes.Remove(_quizzes[choiceRemove - 1]);
                    Console.WriteLine("Викторина успешно удалена!");
                    continue;
                }
            }
            Console.WriteLine("Ввод не подходит нашим параметрам! Попробуйте заново.");
            continue;
        }
    }

    private void EditQuiz(int index)
    {
        while (true)
        {
            Console.WriteLine("Выберите новую категорию для этой викторины:");
            Console.WriteLine("M - Математика;");
            Console.WriteLine("B - Биология;");
            Console.WriteLine("G - География;");
            Console.WriteLine("P - Физика;");
            Console.WriteLine("L - Литература;");
            Console.WriteLine("X - Смешанный;");
            Console.WriteLine("0 - Вернуться назад.");
            Console.WriteLine($"( Прежняя категория: {((_quizzes[index].QuizCategory == Category.MIXED) ? _quizzes[index].QuizCategory.ToString()[2] : _quizzes[index].QuizCategory.ToString()[0])} )");
            Console.Write(" >>> ");
            Category category;
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                Console.Clear();
                switch (choice)
                {
                    case 'm':
                    case 'M':
                        {
                            category = Category.MATH;
                            break;
                        }
                    case 'b':
                    case 'B':
                        {
                            category = Category.BIOLOGY;
                            break;
                        }
                    case 'g':
                    case 'G':
                        {
                            category = Category.GEOGRAPHY;
                            break;
                        }
                    case 'p':
                    case 'P':
                        {
                            category = Category.PHYSICS;
                            break;
                        }
                    case 'l':
                    case 'L':
                        {
                            category = Category.LITERATURE;
                            break;
                        }
                    case 'x':
                    case 'X':
                        {
                            category = Category.MIXED;
                            break;
                        }
                    case '0':
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                            continue;
                        }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
                continue;
            }
            while (true)
            {
                Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                Console.WriteLine("Введите название для викторины:");
                Console.WriteLine($"( Прежнее название: {_quizzes[index].QuizName} )");
                Console.Write(" >>> ");
                string? quizName = Console.ReadLine();
                quizName ??= string.Empty;
                Console.Clear();
                if (quizName == "0")
                {
                    return;
                }
                Quiz quiz = new Quiz();
                quiz.QuizName = quizName;
                string? questionText = string.Empty;
                for (int i = 0; i < 20; i++)
                {
                    Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                    Console.WriteLine($"Введите {i + 1} вопрос (их максимум может быть 20 штук, чтобы прервать до 20, введите stop):");
                    Console.WriteLine($"( Прежний вопрос: {_quizzes[index].Questions[i].QuestionText} )");
                    Console.Write(" >>> ");
                    questionText = Console.ReadLine();
                    questionText ??= string.Empty;
                    Console.Clear();
                    if (questionText == "0")
                    {
                        quiz.Questions.Clear();
                        break;
                    }
                    else if (questionText.ToLower() == "stop")
                    {
                        break;
                    }
                    while (true)
                    {
                        Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                        Console.WriteLine("ТЕКСТ_ВАРИАНТА/0,ТЕКСТ_ВАРИАНТА/1,ТЕКСТ_ВАРИАНТА/0,ТЕКСТ_ВАРИАНТА/0");
                        Console.WriteLine("1 после / значит вариант правильный, а 0 после / значит вариант неправильный");
                        Console.WriteLine("Пожалуйста, придерживайтесь этих примеров.");
                        Console.WriteLine($"Вопрос: {questionText}");
                        string? pastAnswers = string.Empty;
                        for (int j = 0; j < _quizzes[index].Questions[i].Answers.Count; j++)
                        {
                            pastAnswers += _quizzes[index].Questions[i].Answers[j].AnswerText;
                            if (_quizzes[index].Questions[i].Answers[j].IsCorrect)
                            {
                                pastAnswers += "1";
                            }
                            else
                            {
                                pastAnswers += "0";
                            }
                            if (j != _quizzes[index].Questions[i].Answers.Count - 1)
                            {
                                pastAnswers += ",";
                            }
                        }
                        Console.WriteLine($"( Прежние варианты: {pastAnswers}");
                        Console.WriteLine("Введите варианты:");
                        Console.Write(" >>> ");
                        try
                        {
                            string? answersString = Console.ReadLine();
                            answersString ??= string.Empty;
                            Console.Clear();
                            if (answersString == "0")
                            {
                                break;
                            }
                            List<string> stringAnswers = answersString.Split(',').ToList();
                            List<Answer> answers = new List<Answer>();
                            foreach (var stringAnswer in stringAnswers)
                            {
                                Answer answer = new Answer() { AnswerText = stringAnswer.Split('/')[0] };
                                if (stringAnswer.Split('/')[1] == "1")
                                {
                                    answer.IsCorrect = true;
                                }
                                else if (stringAnswer.Split('/')[1] == "0")
                                {
                                    answer.IsCorrect = false;
                                }
                                answers.Add(answer);

                            }
                            Question question = new Question() { QuestionText = questionText, Answers = answers };
                            quiz.Questions.Add(question);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неправильный ввод! Попробуйте заново.");
                            continue;
                        }
                        break;
                    }
                }
                if (quiz.Questions.Count > 0)
                {
                    _quizzes.Remove(_quizzes[index]);
                    _quizzes.Insert(index, quiz);
                    Console.WriteLine("Викторина успешно отредактирована!");
                }
                break;
            }
            break;
        }
    }

    private void CreateQuiz()
    {
        while (true)
        {
            Console.WriteLine("Выберите категорию:");
            Console.WriteLine("M - Математика;");
            Console.WriteLine("B - Биология;");
            Console.WriteLine("G - География;");
            Console.WriteLine("P - Физика;");
            Console.WriteLine("L - Литература;");
            Console.WriteLine("X - Смешанный;");
            Console.WriteLine("0 - Вернуться назад.");
            Console.Write(" >>> ");
            Category category;
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                Console.Clear();
                switch (choice)
                {
                    case 'm':
                    case 'M':
                        {
                            category = Category.MATH;
                            break;
                        }
                    case 'b':
                    case 'B':
                        {
                            category = Category.BIOLOGY;
                            break;
                        }
                    case 'g':
                    case 'G':
                        {
                            category = Category.GEOGRAPHY;
                            break;
                        }
                    case 'p':
                    case 'P':
                        {
                            category = Category.PHYSICS;
                            break;
                        }
                    case 'l':
                    case 'L':
                        {
                            category = Category.LITERATURE;
                            break;
                        }
                    case 'x':
                    case 'X':
                        {
                            category = Category.MIXED;
                            break;
                        }
                    case '0':
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                            continue;
                        }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
                continue;
            }
            while (true)
            {
                Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                Console.WriteLine("Введите название для викторины:");
                Console.Write(" >>> ");
                string? quizName = Console.ReadLine();
                quizName ??= string.Empty;
                Console.Clear();
                if (quizName == "0")
                {
                    return;
                }    
                Quiz quiz = new Quiz();
                string? questionText = string.Empty;
                quiz.QuizName = quizName;
                for (int i = 0; i < 20; i++)
                {
                    Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                    Console.WriteLine($"Введите {i + 1} вопрос (их максимум может быть 20 штук, чтобы прервать до 20, введите stop):");
                    Console.Write(" >>> ");
                    questionText = Console.ReadLine();
                    questionText ??= string.Empty;
                    Console.Clear();
                    if (questionText == "0")
                    {
                        quiz.Questions.Clear();
                        break;
                    }
                    else if (questionText.ToLower() == "stop")
                    {
                        break;
                    }
                    while (true)
                    {
                        Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                        Console.WriteLine("ТЕКСТ_ВАРИАНТА/0,ТЕКСТ_ВАРИАНТА/1,ТЕКСТ_ВАРИАНТА/0,ТЕКСТ_ВАРИАНТА/0");
                        Console.WriteLine("1 после / значит вариант правильный, а 0 после / значит вариант неправильный");
                        Console.WriteLine("Пожалуйста, придерживайтесь этих примеров.");
                        Console.WriteLine($"Вопрос: {questionText}");
                        Console.WriteLine("Введите варианты:");
                        Console.Write(" >>> ");
                        try
                        {
                            string? answersString = Console.ReadLine();
                            answersString ??= string.Empty;
                            Console.Clear();
                            if (answersString == "0")
                            {
                                break;
                            }
                            List<string> stringAnswers = answersString.Split(',').ToList();
                            List<Answer> answers = new List<Answer>();
                            foreach (var stringAnswer in stringAnswers)
                            {
                                Answer answer = new Answer() { AnswerText = stringAnswer.Split('/')[0] };
                                if (stringAnswer.Split('/')[1] == "1")
                                {
                                    answer.IsCorrect = true;
                                }
                                else if (stringAnswer.Split('/')[1] == "0")
                                {
                                    answer.IsCorrect = false;
                                }
                                answers.Add(answer);

                            }
                            Question question = new Question() { QuestionText = questionText, Answers = answers };
                            quiz.Questions.Add(question);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Неправильный ввод! Попробуйте заново.");
                            continue;
                        }
                        break;
                    }
                }
                if (quiz.Questions.Count > 0)
                {
                    _quizzes.Add(quiz);
                    Console.WriteLine("Викторина успешно создана!");
                }
                break;
            }
            break;
        }
    }

    private void Quizzes()
    {
        while (true)
        {
            Console.WriteLine("Все викторины:");
            if (_quizzes.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < _quizzes.Count; i++)
                {
                    Console.Write($"{i + 1}. {_quizzes[i]}");
                    if (i == _quizzes.Count - 1)
                    {
                        Console.WriteLine('.');
                    }
                    else
                    {
                        Console.WriteLine(';');
                    }
                }
            }
            Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
            Console.WriteLine("Выберите викторину:");
            Console.Write(" >>> ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.Clear();
                if (choice == 0)
                {
                    return;
                }    
                if (choice < 1 || choice > _quizzes.Count)
                {
                    Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                    continue;
                }
                Quiz(choice - 1);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
            }
        }
    }

    private void Quiz(int index)
    {
        while (true)
        {
            var top = _quizzes[index].Progresses;
            Console.WriteLine(_quizzes[index]);
            Console.WriteLine("ТОП:");
            if (top.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                top = top.OrderByDescending(p => p.CorrectAnswersCount).ToList();
                for (int i = 0; i < top.Count; i++)
                {
                    Console.Write($"{i + 1}. {top[i]}");
                    if (top[i].UserLogin == _currentUser.Login)
                    {
                        Console.WriteLine(" (Вы)");
                    }
                    else if (i == top.Count - 1)
                    {
                        Console.WriteLine('.');
                    }
                    else
                    {
                        Console.WriteLine(';');
                    }
                }
            }
            Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
            Console.WriteLine("Чтобы начать проходить викторину введите точное название самой викторины:");
            Console.Write(" >>> ");
            string? name = Console.ReadLine();
            name ??= string.Empty;
            Console.Clear();
            if (name == "0")
            {
                return;
            }
            if (name != _quizzes[index].QuizName)
            {
                Console.WriteLine("Имена не совпадают! Попробуйте заново.");
                continue;
            }
            bool flag = true;
            Progress progress = new Progress() { UserLogin = _currentUser.Login, CorrectAnswersCount = 0, MaxAnswersCount = _quizzes[index].Questions.Count };
            for (int i = 0; i < _quizzes[index].Questions.Count; i++)
            {
                int result = Question(_quizzes[index].Questions[i], i + 1);
                if (result == 1)
                {
                    progress.CorrectAnswersCount += 1;
                }
                else if (result == 0)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                progress.CreationDate = DateTime.Now;
                for (int i = 0; i < _quizzes[index].Progresses.Count; i++)
                {
                    if (progress.UserLogin == _currentUser.Login)
                    {
                        if (progress.CorrectAnswersCount > _quizzes[index].Progresses[i].CorrectAnswersCount)
                        {
                            _quizzes[index].Progresses.Remove(_quizzes[index].Progresses[i]);
                            _quizzes[index].Progresses.Insert(i, progress);
                        }
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    _quizzes[index].Progresses = _quizzes[index].Progresses.Append(progress).ToList();
                }
            }
        }
    }
    private int Question(Question question, int number)
    {
        while (true)
        {
            Console.Write($"{number}. ");
            Console.WriteLine(question);
            Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
            Console.WriteLine("Выберите номер(или номера без пробелов в предоставленной последовательности) правильного по вашему мнению ответа:");
            Console.Write(" >>> ");
            string? userAnswer = Console.ReadLine();
            userAnswer ??= string.Empty;
            Console.Clear();
            string? correctAnswer = string.Empty;
            for (int i = 0; i < question.Answers.Count; i++)
            {
                if (question.Answers[i].IsCorrect)
                {
                    correctAnswer += i + 1;
                }
            }
            if (userAnswer == "0")
            {
                return 0;
            }
            if (userAnswer == correctAnswer)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }

    private void History()
    {
        while (true)
        {
            Console.WriteLine("История:");
            List<KeyValuePair<Quiz, Progress>> kvp = new List<KeyValuePair<Quiz, Progress>>();
            foreach (var quiz in _quizzes)
            {
                foreach (var progress in quiz.Progresses)
                {
                    if (progress.UserLogin == _currentUser.Login)
                    {
                        kvp = kvp.Prepend(new KeyValuePair<Quiz, Progress>(quiz, progress)).ToList();
                    }
                }
            }
            var history = kvp.OrderByDescending(kvp => kvp.Value.CreationDate).ToList();
            if (history.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Вы прошли викторину {history[i].Key} на {history[i].Value.CorrectAnswersCount} из {history[i].Value.MaxAnswersCount} | {history[i].Value.CreationDate}");
                    if (i == history.Count - 1)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.WriteLine(';');
                    }
                }
            }
            Console.WriteLine("Введите clear чтобы очистить историю или 0 чтобы вернуться назад:");
            Console.Write(" >>> ");
            string? choice = Console.ReadLine()?.ToLower();
            choice ??= string.Empty;
            Console.Clear();
            if (choice == "0")
            {
                return;
            }
            else if (choice == "clear")
            {
                history.Clear();
            }
            else
            {
                Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
            }
        }
    }

    private void AccountOptions()
    {
        bool flag = true;
        while (flag)
        {
            Console.WriteLine("Выберите следующую опцию:");
            Console.WriteLine("1. Сменить пароль;");
            Console.WriteLine("2. Сменить дату рождения;");
            Console.WriteLine("3. Вернуться назад.");
            Console.Write(" >>> ");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.Clear();
                switch (choice)
                {
                    case 1:
                        {
                            while (flag)
                            {
                                Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                                Console.WriteLine("Введите ваш пароль:");
                                Console.Write(" >>> ");
                                string? password = Console.ReadLine();
                                password ??= string.Empty;
                                Console.Clear();
                                if (password == "0")
                                {
                                    flag = false;
                                    break;
                                }
                                if (EncoderSHA256.ToSHA256(password) != _currentUser.Password)
                                {
                                    Console.WriteLine("Неправильный пароль! Попробуйте заново.");
                                    continue;
                                }
                                while (flag)
                                {
                                    Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                                    Console.WriteLine("Придумайте новый пароль:");
                                    Console.Write(" >>> ");
                                    string? newPassword = Console.ReadLine();
                                    newPassword ??= string.Empty;
                                    Console.Clear();
                                    if (password == "0")
                                    {
                                        flag = false;
                                        break;
                                    }
                                    string? passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
                                    Regex regex = new Regex(passwordPattern);
                                    if (!regex.IsMatch(newPassword))
                                    {
                                        Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                                        continue;
                                    }
                                    Console.WriteLine("Пароль успешно сменён!");
                                    _currentUser.Password = EncoderSHA256.ToSHA256(newPassword);
                                    break;
                                }
                                break;
                            }
                            break;
                        }
                    case 2:
                        {
                            {
                                while (true)
                                {
                                    Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                                    Console.WriteLine("Введите дату рождения (формат: дд/мм/гггг; вам должно быть больше 6-и лет):");
                                    string? dateString = Console.ReadLine();
                                    dateString ??= string.Empty;
                                    Console.Clear();
                                    if (dateString == "0")
                                    {
                                        flag = false;
                                        break;
                                    }
                                    dateString ??= string.Empty;
                                    if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                                    {
                                        Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                                        continue;
                                    }
                                    if ((DateTime.Now - date).Days < 6 * 365)
                                    {
                                        Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                                        continue;
                                    }
                                    Console.WriteLine("Дата рождения успешно сменена!");
                                    _currentUser.Birthday = date;
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            flag = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                            break;
                        }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ошибка при чтении запроса! Попробуйте заново.");
                continue;
            }
        }
    }
}