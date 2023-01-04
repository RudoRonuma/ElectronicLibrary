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
        private readonly FileDataStorage _storage;
        internal FileAuth(FileDataStorage storage)
        {
            _storage = storage;
        }
        public LibraryUser Login(UserAuthConfig config)
        {
            var theUser = _storage.GetUserByUsername(config.Username);
            if (theUser.Password != theUser.Password)
                throw new InvalidPasswordException();

            return theUser;
        }
        public LibraryUser Register(UserAuthConfig config)
        {
            LibraryMember theUser = new()
            {
                Username = config.Username,
                Email = config.Email,
                Password = config.Password,
                Name = config.Name,
                Role = UserRole.NormalUser,
            };
            _storage.AddNewUser(theUser);

            return theUser;
        }

        public LibraryUser RegisterAdmin(UserAuthConfig config)
        {
            Librarian theUser = new()
            {
                Username = config.Username,
                Email = config.Email,
                Password = config.Password,
                Name = config.Name,
                Role = UserRole.Administrator,
            };
            _storage.AddNewUser(theUser);

            return theUser;
        }
    }
}
