using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase
{
    public interface ILibrarianRepository
    {
        public void AddNewBook(LibraryBook book);
        public bool RemoveBook(long bookId);
        public string PrintBook(long bookId);
    }
}
