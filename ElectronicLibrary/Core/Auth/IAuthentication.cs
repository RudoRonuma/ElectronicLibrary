using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Auth
{
    public interface IAuthentication
    {
        LibraryUser Login(UserAuthConfig config);
        LibraryUser Register(UserAuthConfig config);
        LibraryUser RegisterAdmin(UserAuthConfig config);
    }
}
