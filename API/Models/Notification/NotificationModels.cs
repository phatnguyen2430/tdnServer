using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Notification
{
    public class NotificationModels
    {
    }
    public class NotificationRequestModel
    {
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public int TestId { get; set; }
    }

    public class NotificationResponseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
    }
}
