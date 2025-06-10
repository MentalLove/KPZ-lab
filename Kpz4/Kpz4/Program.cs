using System;
using System.Collections.Generic;

namespace SupportSystem
{
    // Абстрактний Handler
    abstract class SupportHandler
    {
        protected SupportHandler nextHandler;

        public void SetNext(SupportHandler handler)
        {
            nextHandler = handler;
        }

        public abstract bool Handle(string input);
    }

    // Конкретний Handler для Технічної підтримки
    class TechnicalSupportHandler : SupportHandler
    {
        public override bool Handle(string input)
        {
            if (input == "1")
            {
                Console.WriteLine("Ви підключені до Технічної підтримки.");
                return true;
            }
            else if (nextHandler != null)
            {
                return nextHandler.Handle(input);
            }
            return false;
        }
    }

    // Конкретний Handler для Фінансової підтримки
    class FinancialSupportHandler : SupportHandler
    {
        public override bool Handle(string input)
        {
            if (input == "2")
            {
                Console.WriteLine("Ви підключені до Фінансової підтримки.");
                return true;
            }
            else if (nextHandler != null)
            {
                return nextHandler.Handle(input);
            }
            return false;
        }
    }

    // Конкретний Handler для Підтримки клієнтів
    class CustomerSupportHandler : SupportHandler
    {
        public override bool Handle(string input)
        {
            if (input == "3")
            {
                Console.WriteLine("Ви підключені до Підтримки клієнтів.");
                return true;
            }
            else if (nextHandler != null)
            {
                return nextHandler.Handle(input);
            }
            return false;
        }
    }

    // Конкретний Handler для Адміністративної підтримки
    class AdministrativeSupportHandler : SupportHandler
    {
        public override bool Handle(string input)
        {
            if (input == "4")
            {
                Console.WriteLine("Ви підключені до Адміністративної підтримки.");
                return true;
            }
            else if (nextHandler != null)
            {
                return nextHandler.Handle(input);
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо ланцюжок Handler'ів
            SupportHandler technical = new TechnicalSupportHandler();
            SupportHandler financial = new FinancialSupportHandler();
            SupportHandler customer = new CustomerSupportHandler();
            SupportHandler admin = new AdministrativeSupportHandler();

            technical.SetNext(financial);
            financial.SetNext(customer);
            customer.SetNext(admin);

            bool resolved = false;

            while (!resolved)
            {
                Console.WriteLine("Вітаємо у системі підтримки. Оберіть тип підтримки:");
                Console.WriteLine("1 - Технічна підтримка");
                Console.WriteLine("2 - Фінансова підтримка");
                Console.WriteLine("3 - Підтримка клієнтів");
                Console.WriteLine("4 - Адміністративна підтримка");
                Console.WriteLine("Введіть номер опції:");

                string input = Console.ReadLine();

                // Пробуємо обробити запит
                resolved = technical.Handle(input);

                if (!resolved)
                {
                    Console.WriteLine("Не вдалося знайти відповідний рівень підтримки. Спробуйте ще раз.\n");
                }
            }

            Console.WriteLine("Дякуємо за звернення!");
        }
    }
}
