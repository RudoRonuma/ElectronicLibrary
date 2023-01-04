using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
#if USE_MEMORY_DB

namespace ElectronicLibrary.Core.DataBase.Client.MemoryClient
{
    public class MemoryDBContext : ILibraryRepository, ILibrarianRepository
    {
        public IAuthentication Authentication { get; }
        public MemoryDBContext()
        {
            Authentication = new MemoryAuth();
        }

        public BookBorrowed BorrowBook(long userId, long bookId)
        {
            BookBorrowed borrowed = new()
            {
                BookId = bookId,
                UserId = userId,
            };
            MemoryDataStorage.AddNewBorrowed(borrowed);

            return borrowed;
        }
        public BookReturned ReturnBook(long userId, long bookId)
        {
            var borrowedObject = MemoryDataStorage.GetBorrowed(userId, bookId);
            BookReturned returned = new()
            {
                BookId = bookId,
                UserId = userId,
                BorrowEventId = borrowedObject.EventId,
            };
            MemoryDataStorage.AddNewReturned(returned);

            borrowedObject.ReturnEventId = returned.EventId;
            return returned;
        }
        public List<LibraryBook> GetListOfLibraryBooks() =>
            MemoryDataStorage.AllBooks;
        public List<LibraryBook>? GetListOfUserBooks(long userId) =>
            MemoryDataStorage.GetUsersBook(userId)?.ToList();
        public List<BookGenre> GetBookGenres() =>
            MemoryDataStorage.AllBooks.DistinctBy(b => b.Genre)
            .Select(b => b.Genre).ToList();
        public List<LibraryBook> GetBooksByGenre(BookGenre genre) =>
            MemoryDataStorage.AllBooks.FindAll(b => b.Genre == genre);
        public LibraryBook GetBook(BookBorrowed borrowed) =>
            MemoryDataStorage.AllBooks.First(b => b.BookId == borrowed.BookId);
            
        public List<BookBorrowed> GetListOfBorrowedBooks() =>
            MemoryDataStorage.AllBorrowed;
        public LibraryUser GetUser(long userId) =>
            MemoryDataStorage.GetUserById(userId);
        public void AddNewBook(LibraryBook book) =>
            MemoryDataStorage.AddNewBook(book);
        public bool RemoveBook(long bookId) =>
            MemoryDataStorage.RemoveBook(bookId);
        public string PrintBook(long bookId) =>
            MemoryDataStorage.GetLibraryBook(bookId).ToString();
        public bool UserExists(string username) =>
            MemoryDataStorage.AllUsers.Any(u => u.Username == username);
    }
}

#endif // USE_MEMORY_DB

