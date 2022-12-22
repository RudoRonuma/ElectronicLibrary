using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.Auth
{
    public interface IAuthentication
    {
        public void Login();
        public void Register();
        
    }
}
