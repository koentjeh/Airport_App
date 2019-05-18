using System;
using Airport.Infrastructure.Messaging;

namespace Airport.TimeService.Events
{
    public class DayHasPassed : Event
    {
        public DayHasPassed(Guid messageId) : base(messageId)
        {

        }
    }
}
