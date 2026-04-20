using CodeBase.Services.ProgressService;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.PurchaseService
{
    public class MockPurchaseService : IPurchaseService
    {
        private const bool IS_OPERATION_SUCCESS = true;
        
        private IProgressService _progressService;
        public MockPurchaseService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public async UniTask<bool> TryBuyOffer(string offerId)
        {
            await UniTask.Delay(1000);
            
            // Какая-то асинхронная логика связанная например с получением наград
            // в зависимости от конкретного оффера или проверкой на то, хватает
            // ли игроку денег.
            
            return IS_OPERATION_SUCCESS;
        }
    }
}