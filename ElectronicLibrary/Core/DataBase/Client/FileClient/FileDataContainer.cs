using ElectronicLibrary.Core.DataBase.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Client.FileClient
{
    [Serializable]
    public class FileDataContainer
    {
        [JsonProperty("AllBooks")]
        public List<LibraryBook> AllBooks { get; set; }
        [JsonProperty("AllUsers")]
        public List<LibraryUser> AllUsers { get; set; }
        [JsonProperty("AllBorrowed")]
        public List<BookBorrowed> AllBorrowed { get; set; }
        [JsonProperty("AllReturned")]
        public List<BookReturned> AllReturned { get; set; }
    }
}
