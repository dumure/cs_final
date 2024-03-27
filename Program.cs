class Program
{
    static void Main(string[] args)
    {
        UsersDatabase database = new UsersDatabase();
        var users = database.GetUsers();
        bool found = false;
        foreach (var user in users)
        {
            if (user.Login == "admin")
            {
                found = true;
            }
        }
        if (!found)
        {
            database.AddUser(new User() { Login = "admin", Password = EncoderSHA256.ToSHA256("admin"), Birthday = DateTime.Now });
        }
        bool flag = true;
        Console.WriteLine("Добро пожаловать в приложение Quizzer!");
        Console.WriteLine("Здесь вы сможете проходить тесты на различные темы или создавать свои.");
        Console.WriteLine("Для начала вам нужно зайти в свой личный аккаунт, выберите следующее действие:");
        while (flag)
        {
            Console.WriteLine("R - Регистрация;");
            Console.WriteLine("L - Авторизация;");
            Console.WriteLine("Q - Выход.");
            Console.Write(" >>> ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                Console.Clear();
                switch (choice)
                {
                    case 'r':
                    case 'R':
                        {
                            Registration.SignUp();
                            break;
                        }
                    case 'l':
                    case 'L':
                        {
                            Authorization.SignIn();
                            break;
                        }
                    case 'q':
                    case 'Q':
                        {
                            flag = false;
                            break;
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
            }
        }
    }
}
