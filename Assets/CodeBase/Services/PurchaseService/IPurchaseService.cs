using Cysharp.Threading.Tasks;

namespace CodeBase.Services.PurchaseService
{
    public interface IPurchaseService
    {
        UniTask<bool> TryBuyOffer(string offerId);
    }
}