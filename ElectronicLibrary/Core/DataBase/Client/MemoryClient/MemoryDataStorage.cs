using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if USE_MEMORY_DB

namespace ElectronicLibrary.Core.DataBase.Client.MemoryClient
{
    public static class MemoryDataStorage
    {
        internal static List<LibraryBook> AllBooks { get; private set; }
        internal static List<LibraryUser> AllUsers { get; private set; }
        internal static List<BookBorrowed> AllBorrowed { get; private set; }
        internal static List<BookReturned> AllReturned { get; private set; }

        private static long _currentBookId = 0;
        private static readonly object _bookIdLock = new();
        private static long _currentUserId = 0;
        private static readonly object _userIdLock = new();
        static MemoryDataStorage()
        {
            AllBooks = new();
            AllUsers = new();
            AllBorrowed = new();
            AllReturned = new();
        }
        
        /// <summary>
        /// Returns the book with the specified <paramref name="bookId"/>.
        /// </summary>
        /// <param name="bookId">
        /// The unique-id of the book.
        /// </param>
        /// <returns></returns>
        public static LibraryBook GetLibraryBook(long bookId) =>
            AllBooks.Find(b => b.BookId == bookId);

        public static BookBorrowed GetBorrowed(long userId, long bookId) =>
            AllBorrowed.Find(b => b.BookId == bookId && b.UserId == userId);

        /// <summary>
        /// Returns the books which was borrowed by the user, but haven't returned
        /// yet.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static IEnumerable<LibraryBook> GetUsersBook(long userId) =>
            AllBorrowed.Where(
                b => b.UserId == userId && !string.IsNullOrEmpty(b.ReturnEventId))
                .Select(borrowed => GetLibraryBook(borrowed.BookId));

        public static void AddNewBook(LibraryBook book) =>
            AllBooks.Add(VerifyModelId(book));
        public static void AddNewUser(LibraryUser user) =>
            AllUsers.Add(VerifyModelId(user));
        public static void AddNewBorrowed(BookBorrowed borrowed) =>
            AllBorrowed.Add(VerifyModelId(borrowed));
        public static void AddNewReturned(BookReturned returned) =>
            AllReturned.Add(VerifyModelId(returned));

        private static LibraryBook VerifyModelId(LibraryBook book)
        {
            if (book.BookId > 0)
                return book;

            book.BookId = GetNextBookId();
            return book;
        }
        private static LibraryUser VerifyModelId(LibraryUser user)
        {
            if (user.UsertId > 0)
                return user;

            user.UsertId = GetNextUserId();
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

