using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLibrary.Core.DataBase.Models
{
    public class BookReturned
    {
        /// <summary>
        /// The unique id of the event.
        /// </summary>
        [Key]
        public string EventId { get; set; }

        /// <summary>
        /// Id of the user who has returned the book.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// The id of the book which was returned.
        /// </summary>
        public long BookId { get; set; }

        /// <summary>
        /// The id of the borrowing-event.
        /// </summary>
        public string BorrowEventId { get; set; }

        /// <summary>
        /// The date in which the user has returned us this book.
        /// </summary>
        public DateTime ReturnedDate { get; set; }
    }
}
