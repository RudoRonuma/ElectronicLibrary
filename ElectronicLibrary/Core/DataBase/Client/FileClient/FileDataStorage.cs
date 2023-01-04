using ElectronicLibrary.Core.DataBase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#if USE_FILE_DB

namespace ElectronicLibrary.Core.DataBase.Client.FileClient
{
    [Serializable]
    public class FileDataStorage
    {
        public string DBFilePath { get; set; }
        public FileDataContainer Container { get; protected set; }
        public List<LibraryBook> AllBooks { get => Container.AllBooks; }
        public List<LibraryUser> AllUsers { get => Container.AllUsers; }
        public List<BookBorrowed> AllBorrowed { get => Container.AllBorrowed; }
        public List<BookReturned> AllReturned { get => Container.AllReturned; }

        private static long _currentBookId = 0;
        private static readonly object _bookIdLock = new();
        private static long _currentUserId = 0;
        private static readonly object _userIdLock = new();
        internal FileDataStorage(string dbName)
        {
            DBFilePath = dbName;
            if (!File.Exists(dbName))
            {
                File.Create(dbName).Dispose();
                Container = new()
                {
                    AllBooks = new(),
                    AllBorrowed = new(),
                    AllReturned = new(),
                    AllUsers = new(),
                };
                return;
            }
            Container = JsonConvert.DeserializeObject<FileDataContainer>(
                File.ReadAllText(dbName));
            Container ??= new()
            {
                AllBooks = new(),
                AllBorrowed = new(),
                AllReturned = new(),
                AllUsers = new(),
            };

        }

        private void SaveData() =>
            File.WriteAllText(DBFilePath,
                JsonConvert.SerializeObject(Container));
        private void DoAndSave(Action action)
        {
            action.Invoke();
            SaveData();
        }
        private bool DoAndSave(Func<bool> action)
        {
            if (action.Invoke())
            {
                SaveData();
                return true;
            }
            return false;
        }
        //private T DoAndSave<T>(Func<T> action)
        //{
        //    var result = action.Invoke();
        //    SaveData();
        //    return result;
        //}

        public LibraryUser GetUserByUsername(string username) =>
            AllUsers.Find(x => x.Username == username);
        public LibraryUser GetUserById(long userId) =>
            AllUsers.Find(x => x.UserId == userId);

        /// <summary>
        /// Returns the book with the specified <paramref name="bookId"/>.
        /// </summary>
        /// <param name="bookId">
        /// The unique-id of the book.
        /// </param>
        /// <returns></returns>
        public LibraryBook GetLibraryBook(long bookId) =>
            AllBooks.Find(b => b.BookId == bookId);

        public BookBorrowed GetBorrowed(long userId, long bookId) =>
            AllBorrowed.Find(b => b.BookId == bookId && b.UserId == userId);

        /// <summary>
        /// Returns the books which was borrowed by the user, but haven't returned
        /// yet.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<LibraryBook> GetUsersBook(long userId) =>
            AllBorrowed.Where(
                b => b.UserId == userId && !string.IsNullOrEmpty(b.ReturnEventId))
                .Select(borrowed => GetLibraryBook(borrowed.BookId));

        public void AddNewBook(LibraryBook book) =>
            DoAndSave(() => AllBooks.Add(VerifyModelId(book)));
        public bool RemoveBook(long bookId) =>
            DoAndSave(() => AllBooks.Remove(GetLibraryBook(bookId)));
        public void AddNewUser(LibraryUser user) =>
            DoAndSave(() => AllUsers.Add(VerifyModelId(user)));
        public void AddNewBorrowed(BookBorrowed borrowed) =>
            DoAndSave(() => AllBorrowed.Add(VerifyModelId(borrowed)));
        public void AddNewReturned(BookReturned returned) =>
            DoAndSave(() => AllReturned.Add(VerifyModelId(returned)));

        private static LibraryBook VerifyModelId(LibraryBook book)
        {
            if (book.BookId > 0)
                return book;

            book.BookId = GetNextBookId();
            return book;
        }
        private static LibraryUser VerifyModelId(LibraryUser user)
        {
            if (user.UserId > 0)
                return user;

            user.UserId = GetNextUserId();
            return user;
        }
        private static BookBorrowed VerifyModelId(BookBorrowed borrowed)
        {
            if (!string.IsNullOrEmpty(borrowed.EventId))
                return borrowed;

            borrowed.EventId = GetNextEventId("BR");
            return borrowed;
        }
        private static BookReturned VerifyModelId(BookReturned returned)
        {
            if (!string.IsNullOrEmpty(returned.EventId))
                return returned;

            returned.EventId = GetNextEventId("RE");
            return returned;
        }
        private static long GetNextBookId()
        {
            lock (_bookIdLock)
            {
                _currentBookId++;
            }

            return _currentBookId;
        }
        private static long GetNextUserId()
        {
            lock (_userIdLock)
            {
                _currentUserId++;
            }

            return _currentUserId;
        }
        private static string GetNextEventId(string eventPrefix) =>
            eventPrefix + $"_{GetUniqueEventString()}";
        private static string GetUniqueEventString() =>
            Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("=", "")
            .Replace("+", "");
    }
}

#endif // USE_MEMORY_DB

