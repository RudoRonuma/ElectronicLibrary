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
    public class MemoryDBContext : ILibraryRepository
    {
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
        public List<LibraryBook>? GetListOfUserBooks(int userId) =>
            MemoryDataStorage.GetUsersBook(userId)?.ToList();
    }
}

#endif // USE_MEMORY_DB

