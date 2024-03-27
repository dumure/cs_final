internal static class Authorization
{
    public static void SignIn()
    {
        while (true)
        {
            Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
            Console.WriteLine("Введите логин:");
            Console.Write(" >>> ");
            string? login = Console.ReadLine();
            login ??= string.Empty;
            Console.Clear();
            if (login == "0")
            {
                return;
            }
            UsersDatabase database = new UsersDatabase();
            var users = database.GetUsers();
            bool flag = false;
            foreach (var user in users)
            {
                if (login == user.Login)
                {
                    flag = true;
                    while (true)
                    {
                        Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                        Console.WriteLine("Введите пароль:");
                        Console.Write(" >>> ");
                        string? password = Console.ReadLine();
                        password ??= string.Empty;
                        Console.Clear();
                        if (password == "0")
                        {
                            return;
                        }
                        if (EncoderSHA256.ToSHA256(password) != user.Password)
                        {
                            Console.WriteLine("Неправильный пароль! Попробуйте заново.");
                            continue;
                        }
                        Console.WriteLine("Успешный вход!");
                        using (QuizzerSession newSession = new QuizzerSession(user))
                        {
                            newSession.StartUsing();
                        }
                        break;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            if (flag)
            {
                break;
            }
            else
            {
                Console.WriteLine("Пользователя с таким логином не существует! Попробуйте заново.");
            }
        }
    }
}