using Grpc.Core;
using Ratelimiter;
using RateLimiter.Interfaces;

namespace RateLimiter.Interfaces
{
    public interface IChannelMaker
    {
        Channel MakeRPCChannel();
    }
    public interface IRateLimiterClient
    {
        State checkRequest(Request request);
    }
}
namespace RateLimiter.Services
{
    public class ChannelMaker : Interfaces.IChannelMaker
    {
        public Channel MakeRPCChannel()
        {
            return new Channel("localhost:50051", ChannelCredentials.Insecure);
        }
    }
    public class RateLimiterClient :IRateLimiterClient
    {
        public Channel channel { get; }
        public Ratelimiter.RateLimiter.RateLimiterClient client { get; }

        public RateLimiterClient(IChannelMaker channelMaker)
        {
            channel = channelMaker.MakeRPCChannel();
            client = new Ratelimiter.RateLimiter.RateLimiterClient(channel);
        }

        public State checkRequest(Request request)
        {
            return client.checkRequest(request);
        }
    }

    
}
