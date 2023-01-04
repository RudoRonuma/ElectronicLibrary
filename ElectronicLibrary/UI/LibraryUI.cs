using ElectronicLibrary.Core.DataBase;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.UI
{
    public abstract class LibraryUI
    {
        public virtual ILibraryRepository LibraryRepository { get; set; }
        public virtual ILibrarianRepository LibrarianRepository { get; set; }
        public virtual LibraryUser CurrentUser { get; protected set; }
        public abstract void InitUI(
            ILibraryRepository libraryRepository,
            ILibrarianRepository librarianRepository);
    }
}
