using CodeBase.Services.OffersService;
using CodeBase.Services.ProgressService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Scenes.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IOfferService _offerService;
        private IProgressService _progressService;

        [Inject]
        private void Construct(IOfferService offerService, IProgressService progressService)
        {
            _offerService = offerService;
            _progressService = progressService;
        }
    
        public void Awake()
        {
            AsyncInitialize().Forget();
        }

        private async UniTaskVoid AsyncInitialize()
        {
            await UniTask.WhenAll(
                UniTask.WaitUntil(() => _progressService.IsInitialized),
                UniTask.WaitUntil(() => _offerService.IsInitialized)
            );
            Debug.Log("All critical services async initialized.");
            
            // Пока оставил так, потом можно будет сделать какой-то менеджер для сцен
            SceneManager.LoadScene("Hub");
        }
    }
}
