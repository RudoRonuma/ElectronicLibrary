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
        public BookBorrowed BorrowBook(long userId, long bookId);
        public BookReturned ReturnBook(long userId, long bookId);
        public List<LibraryBook> GetListOfLibraryBooks();
        public List<LibraryBook> GetListOfUserBooks(int userId);
    }
}
