using System;
using System.Collections.Generic;

namespace SimpleLibrary.FP
{
    /// <summary>
    /// 🛠️ 擴充方法 (Extension Method)
    /// 💡 由於 IEnumerable 本身並未提供 ForEach() 方法 (僅 List 擁有)，這會導致我們必須先呼叫 ToList()
    /// ✨ 因此在這裡我們為 IEnumerable 打造一個專屬的 ForEach() 方法，提升程式撰寫的順暢度
    /// </summary>
    public static class Extensions
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}
