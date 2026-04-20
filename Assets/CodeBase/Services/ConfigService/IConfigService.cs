using Cysharp.Threading.Tasks;

namespace CodeBase.Services.ConfigService
{
    public interface IConfigService
    {
        UniTask<T> LoadAsync<T>(string id = null) where T : IConfig;
    }
}