using Grpc.Core;
using Ratelimiter;
using RateLimiter.Interfaces;

namespace RateLimiter.Interfaces
{
    public interface IChannelMaker
    {
        Channel MakeRPCChannel();
    }
    public interface IRateLimiterService
    {
        State checkRequest(Request request);
    }
}
namespace RateLimiter.Services
{
    public class ChannelMakerImpl : Interfaces.IChannelMaker
    {
        public Channel MakeRPCChannel()
        {
            return new Channel("localhost:50051", ChannelCredentials.Insecure);
        }
    }
    public class RateLimiterServiceImpl : IRateLimiterService
    {
        public Channel channel { get; }
        public Ratelimiter.RateLimiter.RateLimiterClient client { get; }

        public RateLimiterServiceImpl(IChannelMaker channelMaker)
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
