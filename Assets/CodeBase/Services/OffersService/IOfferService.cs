using CodeBase.Core;
using Zenject;

namespace CodeBase.Services.OffersService
{
    public interface IOfferService : IInitializable, IAsyncInitializable
     {
        OfferData GetOfferWithID(string id);
    }
}