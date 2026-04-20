using Newtonsoft.Json;

namespace CodeBase.Core.Static
{
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings Default = new()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
    }
}