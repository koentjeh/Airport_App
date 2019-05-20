using System;
using System.Collections.Generic;
using System.Text;
using Airport.Infrastructure.Messaging;

namespace Airport.NotificationService.Events
{
    public class DayHasPassed
    {
        public DayHasPassed(Guid messageId) : base(messageId)
        {

        }
    }
}
