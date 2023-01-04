using ElectronicLibrary.Core.Auth;
using ElectronicLibrary.Core.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElectronicLibrary.Core.DataBase.Client.MemoryClient
{
#if USE_MEMORY_DB
    internal class MemoryAuth : IAuthentication
    {
        public LibraryUser Login(UserAuthConfig config)
        {
            var theUser = MemoryDataStorage.GetUserByUsername(config.Username);
            if (theUser == null)
                throw new InvalidUsernameException();

            if (theUser.Password != config.Password)
                throw new InvalidPasswordException();

            return theUser;
        }
        public LibraryUser Register(UserAuthConfig config)
        {
            if (MemoryDataStorage
                .GetUserByUsername(config.Username) != null)
                throw new InvalidUsernameException();
            
            LibraryMember theUser = new()
            {
                Username = config.Username,
                Email = config.Email,
                Password = config.Password,
                Name = config.Name,
                Role = UserRole.NormalUser,
            };
            MemoryDataStorage.AddNewUser(theUser);

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
            MemoryDataStorage.AddNewUser(theUser);

            return theUser;
        }            
    }
#endif // USE_MEMORY_DB
}
