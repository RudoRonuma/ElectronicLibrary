using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase
{
    public interface ILibraryRepository
    {
        public IAuthentication Authentication { get; }
        public BookBorrowed BorrowBook(long userId, long bookId);
        public BookReturned ReturnBook(long userId, long bookId);
        public List<LibraryBook> GetListOfLibraryBooks();
        public List<LibraryBook> GetBooksByGenre(BookGenre genre);
        public LibraryBook GetBook(BookBorrowed borrowed);
        public List<LibraryBook> GetListOfUserBooks(long userId);
        public List<BookBorrowed> GetListOfBorrowedBooks();
        public LibraryUser GetUser(long userId);
        public List<BookGenre> GetBookGenres();
        public bool UserExists(string username);
    }
}
