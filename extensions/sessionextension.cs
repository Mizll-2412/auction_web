using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BTL_LTWNC.Extensions
{
    public static class SessionExtensions
    {
        // Phương thức lưu trữ đối tượng phức tạp vào Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Phương thức lấy đối tượng phức tạp từ Session
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
