using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
#if USE_FILE_DB

namespace ElectronicLibrary.Core.DataBase.Client.FileClient
{
    public class FileDBContext : ILibraryRepository, ILibrarianRepository
    {
        public IAuthentication Authentication { get; }
        internal FileDataStorage Storage { get; set; }
        public FileDBContext(string dbName = "database.txt")
        {
            Storage = new(dbName);
            Authentication = new FileAuth(Storage);
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
            Storage.AllBooks;
        public List<LibraryBook>? GetListOfUserBooks(int userId) =>
            Storage.GetUsersBook(userId)?.ToList();
        public bool UserExists(string username) =>
            Storage.AllUsers.Any(u => u.Username == username);

        public List<LibraryBook>? GetListOfUserBooks(long userId) =>
            Storage.GetUsersBook(userId)?.ToList();
        public List<BookGenre> GetBookGenres() =>
            Storage.AllBooks.DistinctBy(b => b.Genre)
            .Select(b => b.Genre).ToList();
        public List<LibraryBook> GetBooksByGenre(BookGenre genre) =>
            Storage.AllBooks.FindAll(b => b.Genre == genre);
        public LibraryBook GetBook(BookBorrowed borrowed) =>
            Storage.AllBooks.First(b => b.BookId == borrowed.BookId);

        public List<BookBorrowed> GetListOfBorrowedBooks() =>
            Storage.AllBorrowed;
        public LibraryUser GetUser(long userId) =>
            Storage.GetUserById(userId);
        public void AddNewBook(LibraryBook book) =>
            Storage.AddNewBook(book);
        public bool RemoveBook(long bookId) =>
            Storage.RemoveBook(bookId);
        public string PrintBook(long bookId) =>
            Storage.GetLibraryBook(bookId).ToString();
    }
}

#endif // USE_MEMORY_DB

