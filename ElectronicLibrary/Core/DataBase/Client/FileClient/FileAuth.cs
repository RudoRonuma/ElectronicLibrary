using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Client.FileClient
{
    internal class FileAuth : IAuthentication
    {
        public LibraryUser Login(UserAuthConfig config)
        {
            var theUser = FileDataStorage.GetUserByUsername(config.Username);
            if (theUser.Password != theUser.Password)
                throw new InvalidPasswordException();

            return theUser;
        }
        public LibraryUser Register(UserAuthConfig config)
        {
            LibraryUser theUser = new()
            {
                Username = config.Username,
                Email = config.Email,
                Password = config.Password,
                Name = config.Name,
                Role = UserRole.NormalUser,
            };
            FileDataStorage.AddNewUser(theUser);

            return theUser;
        }

        public LibraryUser RegisterAdmin(UserAuthConfig config)
        {
            LibraryUser theUser = new()
            {
                Username = config.Username,
                Email = config.Email,
                Password = config.Password,
                Name = config.Name,
                Role = UserRole.Administrator,
            };
            FileDataStorage.AddNewUser(theUser);

            return theUser;
        }
    }
}
