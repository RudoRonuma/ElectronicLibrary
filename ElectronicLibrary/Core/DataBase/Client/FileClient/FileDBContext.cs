using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
#if USE_MEMORY_DB

namespace ElectronicLibrary.Core.DataBase.Client.FileClient
{
    public class FileDBContext : ILibraryRepository
    {
        public IAuthentication Authentication { get; }
        internal FileDataStorage Storage { get; set; }
        public FileDBContext(string dbName = "database.txt")
        {
            Authentication = new FileAuth();
            Storage = new(dbName);
        }

        public BookBorrowed BorrowBook(long userId, long bookId)
        {
            BookBorrowed borrowed = new()
            {
                BookId = bookId,
                UserId = userId,
            };
            Storage.AddNewBorrowed(borrowed);

            return borrowed;
        }
        public BookReturned ReturnBook(long userId, long bookId)
        {
            var borrowedObject = Storage.GetBorrowed(userId, bookId);
            BookReturned returned = new()
            {
                BookId = bookId,
                UserId = userId,
                BorrowEventId = borrowedObject.EventId,
            };
            Storage.AddNewReturned(returned);

            borrowedObject.ReturnEventId = returned.EventId;
            return returned;
        }
        public List<LibraryBook> GetListOfLibraryBooks() =>
            FileDataStorage.AllBooks;
        public List<LibraryBook>? GetListOfUserBooks(int userId) =>
            FileDataStorage.GetUsersBook(userId)?.ToList();
        public bool UserExists(string username) =>
            FileDataStorage.AllUsers.Any(u => u.Username == username);
    }
}

#endif // USE_MEMORY_DB

