using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.NotificationAggregate
{
    public class Notification : BaseEntity, IAggregateRoot
    {
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int Type { get; set; }
        public int testId { get; set; }
    }
    public enum NotificationType
    {
        NewExercise = 1,
        NewResult = 2,
        Expire = 3
    }
}
