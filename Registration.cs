using System.Text.RegularExpressions;

internal static class Registration
{
    public static void SignUp()
    {
        while (true)
        {
            Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
            Console.WriteLine("Придумайте логин (минимальная длина логина 6 символов):");
            Console.Write(" >>> ");
            string? login = Console.ReadLine()?.ToLower();
            login ??= string.Empty;
            Console.Clear();
            if (login == "0")
            {
                return;
            }
            if (login?.Length < 6)
            {
                Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                continue;
            }
            UsersDatabase database = new UsersDatabase();
            var users = database.GetUsers();
            bool flag = false;
            foreach (var user in users)
            {
                if (user.Login == login)
                {
                    Console.WriteLine("Такой логин уже имеется! Попробуйте заново.");
                    flag = true;
                }
            }
            if (flag)
            {
                continue;
            }
            while (true)
            {
                Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                Console.WriteLine("Придумайте пароль (минимум 8 символов; как минимум одна буква, одна цифра и один специальный символ):");
                Console.Write(" >>> ");
                string? password = Console.ReadLine();
                password ??= string.Empty;
                Console.Clear();
                if (password == "0")
                {
                    return;
                }
                string? passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
                Regex regex = new Regex(passwordPattern);
                if (!regex.IsMatch(password))
                {
                    Console.WriteLine("Ввод не подходит под наши параметры! Попробуйте заново.");
                    continue;
                }
                while (true)
                {
                    Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                    Console.WriteLine("Повторите пароль:");
                    Console.Write(" >>> ");
                    string? repeatPassword = Console.ReadLine();
                    repeatPassword ??= string.Empty;
                    Console.Clear();
                    if (repeatPassword == "0")
                    {
                        return;
                    }
                    if (repeatPassword != password)
                    {
                        Console.WriteLine("Пароли не совпадают! Попробуйте заново.");
                        continue;
                    }
                    while (true)
                    {
                        Console.WriteLine("[ Введите 0 если хотите вернуться назад ]");
                        Console.WriteLine("Введите дату рождения (формат: дд/мм/гггг; вам должно быть больше 6-и лет):");
                        string? dateString = Console.ReadLine();
                        dateString ??= string.Empty;
                        Console.Clear();
                        if (dateString == "0")
                        {
                            return;
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
                        User user = new User() { Birthday = date, Password = EncoderSHA256.ToSHA256(password), Login = login};
                        database.AddUser(user);
                        Console.WriteLine("Успешная регистрация!");
                        using (QuizzerSession newSession = new QuizzerSession(user))
                        {
                            newSession.StartUsing();
                        }
                        break;
                    }
                    break;
                }
                break;
            }
            break;
        }
    }
}