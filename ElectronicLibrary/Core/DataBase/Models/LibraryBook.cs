using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Models
{
    public class LibraryBook
    {
        [Key]
        public long BookId { get; set; }

        [MaxLength(100)]
        [MinLength(1)]
        public string Title { get; set; }

        [MaxLength(45)]
        [MinLength(2)]
        public string Author { get; set; }

        public BookGenre Genre { get; set; }

        public override string ToString() =>
            $"{BookId} - {Title} By {Author}";
    }
}
