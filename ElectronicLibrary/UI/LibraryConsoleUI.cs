using ElectronicLibrary.Core.DataBase;
using ElectronicLibrary.Core.DataBase.Models;
using ElectronicLibrary.Core.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if USE_CONSOLE_UI

namespace ElectronicLibrary.UI
{
    internal class LibraryConsoleUI : LibraryUI
    {
        public override void InitUI(
            ILibraryRepository libraryRepository,
            ILibrarianRepository librarianRepository)
        {
            LibraryRepository = libraryRepository;
            LibrarianRepository = librarianRepository;
            AddAdmins();
            AddBooks();
            while (!DoOperation()) ;
        }
        public bool DoOperation()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine(
                    "Select one of the following options: " +
                    "\n1. Login" +
                    "\n2. Register" +
                    "\n3. exit"
                );

                switch (Console.ReadLine()!.Trim())
                {
                    case "1":
                        if (ShowLoginPage())
                            break;
                        return false;
                    case "2":
                        ShowRegisterPage();
                        return false;
                    default:
                        return true;
                }

            }

            if (CurrentUser.Role == UserRole.Administrator)
            {
                while (!ShowAdminMainMenu()) ;
            }
            else
            {
                while (!ShowMainMenu()) ;
            }

            return CurrentUser != null;
        }

        bool ShowLoginPage()
        {
            Console.WriteLine("Welcome to the login system.");
            try
            {
                CurrentUser = LibraryRepository.Authentication.Login(new()
                {
                    Username = GetConsoleInput("Enter your username: "),
                    Password = GetSecureConsoleInput("Enter your password: "),
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error of {ex.GetType().Name} happened!\n" +
                    $"Detail: {ex.Message}"
                );
            }

            return false;
        }

        void ShowRegisterPage()
        {
            Console.WriteLine("Welcome to registeration system.");
            try
            {
                LibraryRepository.Authentication.Register(new()
                {
                    Username = GetConsoleInput("Enter your username: "),
                    Password = GetSecureConsoleInput("Enter your password: "),
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error of {ex.GetType().Name} happened!\n" +
                    $"Detail: {ex.Message}"
                );
            }
        }

        bool ShowAdminMainMenu()
        {
            var optionsList = new Dictionary<Tuple<int, string>, Action>
            {
                { new(1, "Add Book"), ShowAddBook },
                { new(2, "Remove Book"), ShowRemoveBook },
                { new(3, "Show Books List"), ShowBooksList },
                { new(4, "Show Borrowed List"), ShowBorrowedList },
                { new(5, "Sign out"), null },
                { new(6, "Exit"), null },
            };

            var userChoice = GetConsoleInput(
                "Select one of the following options: \n" +
                string.Join("\n", optionsList.Keys
                    .Select(s => $"{s.Item1}. {s.Item2}")) + "\n",
                Convert.ToInt32);

            try
            {
                var f = optionsList.First(o => o.Key.Item1 == userChoice);
                if (f.Value == null)
                {
                    goto switch_cases;
                }

                try
                {
                    f.Value.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error had happened: {ex}");
                    return false;
                }
            }
            catch
            {
                return false;
            }

        switch_cases:
            switch (userChoice)
            {
                case 5:
                    CurrentUser = null;
                    Console.WriteLine("You have been signed out!");
                    return true;
                case 6:
                    Console.WriteLine("Thanks for using this library!");
                    return true;
            }


            return false;
        }

        void ShowBooksList()
        {
            Console.WriteLine(
                "Here is a list of all books: \n" +
                string.Join("\n", LibraryRepository.GetListOfLibraryBooks()));
        }

        void ShowBorrowedList()
        {
            Console.WriteLine("Here is a list of borrowed books by each user:");
            var allBorrowed = from borrowed in LibraryRepository.GetListOfBorrowedBooks()
                           where string.IsNullOrEmpty(borrowed.ReturnEventId)
                           group borrowed by borrowed.UserId into borrowGroup
                           orderby borrowGroup.Key descending
                           select borrowGroup;

            
            Console.WriteLine(string.Join("\n", allBorrowed.
                Select(b => $"{LibraryRepository.GetUser(b.Key)}'s books:\n" + 
                    string.Join("\n", b
                        .Select(borrowed => LibraryRepository.GetBook(borrowed))))));
        }

        void ShowAddBook()
        {
            var theBook = new LibraryBook()
            {
                Author = GetConsoleInput("Give me the Author name: "),
                Title = GetConsoleInput("Give me the Book Title: "),
                Genre = GetConsoleInput("Give me the Genre: ", Enum.Parse<BookGenre>)
            };
            LibrarianRepository.AddNewBook(theBook);

            Console.WriteLine($"Book has been added; ID is {theBook.BookId}");
        }

        void ShowRemoveBook()
        {
            if (!LibrarianRepository.RemoveBook(
                GetConsoleInput("Give the book ID: ",
                Convert.ToInt64)))
            {
                Console.WriteLine("The specified ID was not found.");
                return;
            }

            Console.WriteLine("Book has been removed!");
        }

        bool ShowMainMenu()
        {
            var optionsList = new Dictionary<Tuple<int, string>, Action>
            {
                { new(1, "Borrow book"), ShowBorrowBook },
                { new(2, "Borrow book by genre"), ShowBorrowBookByGenre },
                { new(3, "Return book"), ShowReturnBook },
                { new(4, "Sign out"), null },
                { new(5, "Exit"), null },
            };

            var userChoice = GetConsoleInput(
                "Select one of the following options: \n" +
                string.Join("\n", optionsList.Keys
                    .Select(s => $"{s.Item1}. {s.Item2}")) + "\n",
                Convert.ToInt32);

            try
            {
                var f = optionsList.First(o => o.Key.Item1 == userChoice);
                if (f.Value == null)
                {
                    goto switch_cases;
                }

                try
                {
                    f.Value.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error had happened: {ex}");
                    return false;
                }
            }
            catch
            {
                return false;
            }

            switch_cases:
            switch (userChoice)
            {
                case 4:
                    CurrentUser = null;
                    Console.WriteLine("You have been signed out!");
                    return true;
                case 5:
                    Console.WriteLine("Thanks for using this library!");
                    return true;
            }


            return false;
        }
        void ShowBorrowBook()
        {
            var genreBooks = LibraryRepository.GetListOfLibraryBooks();
            var booksStr = string.Join("\n", genreBooks);

            var targetBook = GetConsoleInput(
                "Give the book ID you want to borrow: ", Convert.ToInt32);
            var borrowed = LibraryRepository.BorrowBook(CurrentUser.UserId, targetBook);

            Console.WriteLine("You have successfully borrowed the book." +
                $"\nThe event ID is {borrowed.EventId}");

            return;
        }
        void ShowBorrowBookByGenre()
        {
            var genres = LibraryRepository.GetBookGenres()
                .Select(g => $"{(int)g}" + g.ToString());
            var genresStr = string.Join("\n", genres);
            var selectedGenre = Enum.Parse<BookGenre>(
                GetConsoleInput($"{genresStr}\nSelect the genre " +
                    "you would like to see: ")
            );

            var genreBooks = LibraryRepository.GetBooksByGenre(selectedGenre);
            var booksStr = string.Join("\n", genreBooks);

            var targetBook = GetConsoleInput(
                $"Give the book ID you want to borrow: \n{booksStr}\n", 
                Convert.ToInt32);
            var borrowed = LibraryRepository.BorrowBook(CurrentUser.UserId, targetBook);

            Console.WriteLine("You have successfully borrowed the book." +
                $"\nThe event ID is {borrowed.EventId}");

            return;
        }

        void ShowReturnBook()
        {
            var borrowed = LibraryRepository.GetListOfUserBooks(CurrentUser.UserId);
            if (borrowed == null || borrowed.Count == 0)
            {
                Console.WriteLine("You have no borrowed books to return!" +
                    "Go and borrow some first, Happy Reading!");
                return;
            }

            Console.WriteLine(string.Join("\n", borrowed) + "\n");
            var userChoice = GetConsoleInput("Select which book to return",
                Convert.ToInt32);
            var returned = LibraryRepository.ReturnBook(CurrentUser.UserId, userChoice);
            Console.WriteLine("You have successfully returned the book." +
                $"\nThe event ID is {returned.EventId}");
        }

        /// <summary>
        /// Shows the <paramref name="prompt"/> value to the user and gets
        /// an string input (if the input's length didn't satisfy the minimum length
        /// requirement, this method will continue getting input from user until it does :).
        /// </summary>
        /// <param name="prompt">
        /// The prompt message to be shown to the user when trying to get their input.
        /// </param>
        /// <param name="minLen">
        /// The minimum allowed lenght for the input value to be returned from this
        /// method. As said before, if the user-input's minimum length is less than this
        /// value, this method will keep asking the user to provide a valid input.
        /// </param>
        /// <returns>
        /// The user input which is >= <paramref name="minLen"/>.
        /// </returns>
        public static string GetConsoleInput(string prompt, int minLen = 1)
        {
            string userInput;
            while (true)
            {
                Console.Write(prompt);
                userInput = Console.ReadLine()!;
                if (userInput.Length >= minLen)
                {
                    break;
                }

                Console.WriteLine($"Sorry, invalid input provided, min length is {minLen}," +
                    "please try again.");
            }

            return userInput;
        }

        /// <summary>
        /// This method does the same job as <see cref="GetConsoleInput(string, int)"/>,
        /// but will mask the input with asterisk ('*') character.
        /// </summary>
        /// <param name="prompt">
        /// The prompt message to be shown to the user when trying to get their input.
        /// </param>
        /// <param name="minLen">
        /// The minimum allowed lenght for the input value to be returned from this
        /// method. As said before, if the user-input's minimum length is less than this
        /// value, this method will keep asking the user to provide a valid input.
        /// </param>
        /// <returns>
        /// The user input which is >= <paramref name="minLen"/>.
        /// </returns>
        public static string GetSecureConsoleInput(string prompt, int minLen = 8)
        {
            string secureInput;
            while (true)
            {
                Console.Write(prompt);
                GetSecureString();
                if (secureInput.Length >= minLen)
                    break;

                Console.WriteLine(
                    $"Sorry, invalid input provided, min len is {minLen}.\n" +
                    "please try again."
                );
            }

            return secureInput;

            void GetSecureString()
            {
                ConsoleKey key;
                secureInput = string.Empty;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && secureInput.Length > 0)
                    {
                        Console.Write("\b \b");
                        secureInput = secureInput[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        secureInput += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.Write("\n");
            }
        }


        private void AddAdmins()
        {
            LibraryRepository.Authentication.RegisterAdmin(new()
            {
                Username = "admin",
                Password = "12345678",
                Email = "admin@google.com",
                Name = "admin",
            });
            LibraryRepository.Authentication.RegisterAdmin(new()
            {
                Username = "admin_user",
                Password = "12345678",
                Email = "admin@google.com",
                Name = "admin",
            });
            LibraryRepository.Authentication.Register(new()
            {
                Username = "ali",
                Name = "Ali Abedini",
                Password = "12345678"
            });

            LibraryRepository.Authentication.Register(new()
            {
                Username = "ali2",
                Name = "Ali Abedini2",
                Password = "12345678"
            });
        }
        private void AddBooks()
        {
            LibrarianRepository.AddNewBook(new()
            {
                Title = "Book1",
                Genre = BookGenre.Science,
                Author = "John",
            });
            LibrarianRepository.AddNewBook(new()
            {
                Title = "The Great Gatsby",
                Genre = BookGenre.Novel,
                Author = "F. Scott Fitzgerald",
            });
            LibrarianRepository.AddNewBook(new()
            {
                Title = "To Kill a Mockingbird",
                Genre = BookGenre.Novel,
                Author = "Harper Lee",
            });
            LibrarianRepository.AddNewBook(new()
            {
                Title = "Invisible Man",
                Genre = BookGenre.Novel,
                Author = "Ralph Ellison",
            });
        }

        /// <summary>
        /// Does kinda the same job as <see cref="GetConsoleInput(string, int)"/>,
        /// but this time, this method will invoke the lambda <paramref name="selector"/>
        /// on the user input (to parse the value to type <typeparamref name="T"/>).
        /// </summary>
        /// <typeparam name="T">
        /// The specified type that we will have to return from this method.
        /// </typeparam>
        /// <param name="prompt">
        /// The prompt message to be shown to the user.
        /// </param>
        /// <param name="selector">
        /// The lambda to be called on the user-input to convert it to the specified
        /// type of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> which is parsed from the
        /// user's input in console.
        /// </returns>
        public static T GetConsoleInput<T>(string prompt, Func<string, T> selector)
        {
            Console.Write(prompt);
            return selector.Invoke(Console.ReadLine()!);
        }
    }
}

#endif

