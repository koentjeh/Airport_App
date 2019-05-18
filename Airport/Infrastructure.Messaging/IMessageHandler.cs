using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Infrastructure.Messaging
{
    public interface IMessageHandler
    {
        void Start(IMessageHandlerCallback callback);
        void Stop();
    }
}
