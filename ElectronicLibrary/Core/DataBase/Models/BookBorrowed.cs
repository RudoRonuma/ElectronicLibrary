using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Models
{
    public class BookBorrowed
    {
        /// <summary>
        /// The unique id of the event.
        /// </summary>
        [Key]
        public string EventId { get; set; }

        /// <summary>
        /// The id of the user who borrowed the book.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// The id of the book which was borrowed.
        /// </summary>
        public long BookId { get; set; }

        /// <summary>
        /// The event id of the return-event.
        /// </summary>
        public string ReturnEventId { get; set; }

        /// <summary>
        /// The date in which the book was borrowed by user.
        /// </summary>
        public DateTime BorrowedDate { get; set; } = DateTime.Now;

        public override string ToString() =>
            $"Book:{BookId}; Borrowed by {UserId}";
    }
}
