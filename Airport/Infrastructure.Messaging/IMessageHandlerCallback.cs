using System.Threading.Tasks;

namespace Airport.Infrastructure.Messaging
{
    public interface IMessageHandlerCallback
    {
        Task<bool> HandleMessageAsync(string messageType, string message);
    }
}
