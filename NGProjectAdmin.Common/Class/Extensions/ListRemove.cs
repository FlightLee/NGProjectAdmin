
using System.Collections.Generic;

namespace NGProjectAdmin.Common.Class.Extensions
{
    /// <summary>
    /// list删除扩展
    /// </summary>
    public static class ListRemove
    {
        /// <summary>
        /// list删除
        /// </summary>
        /// <typeparam name="T">泛型数据类型</typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="arr">删除的数据集合</param>
        /// <returns>集合</returns>
        public static List<T> RemoveRange<T>(this List<T> list, List<T> arr)
        {
            foreach (var item in arr)
            {
                list.Remove(item);
            }

            return list;
        }
    }
}
