using System.Diagnostics;

namespace SimpleLibrary.DesignPatterns
{
    /// <summary>
    /// 🏛️ 提供其他類別繼承，使其快速成為獨體 (Singleton) 模式
    /// </summary>
    /// <typeparam name="T">📦 準備要繼承並實作為獨體的類別</typeparam>
    public class Singleton<T> where T : class, new()
    {
        protected Singleton()
        {
            Debug.Assert(null == _instance);
        }
        private static readonly T _instance = new T();

        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
