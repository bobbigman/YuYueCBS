using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingdee.BOS.Orm.DataEntity;

namespace ahu.YuYue.CBS
{

    public static class CacheHelper
    {
        private static readonly ConcurrentDictionary<string, (DynamicObjectCollection Data, DateTime Timestamp)> mdocBillNoCache
            = new ConcurrentDictionary<string, (DynamicObjectCollection, DateTime)>();
        public static class CacheContext
        {
            public static bool UpdateCache { get; set; } = false;
        }

        // 方法来获取缓存中的值
        public static DynamicObjectCollection GetMdocBillNo(string key)
        {
            if (mdocBillNoCache.TryGetValue(key, out var cacheEntry))
            {
                // 检查时间戳是否超过1小时又400秒
                if ((DateTime.Now - cacheEntry.Timestamp).TotalSeconds > 8000)
                {
                    // 如果超过半小时，返回 null 并移除缓存项
                    mdocBillNoCache.TryRemove(key, out _);
                    return null;
                }
                else
                {
                    return cacheEntry.Data;
                }
            }
            return null;
        }

        // 方法来设置缓存中的值
        public static void SetMdocBillNo(string key, DynamicObjectCollection value)
        {
            // 存储数据和当前时间戳
            mdocBillNoCache[key] = (value, DateTime.Now);
        }

        // 清除特定文档的缓存
        public static void ClearCacheForDocument(string key)
        {
            mdocBillNoCache.TryRemove(key, out _);
        }

        public static void ClearAllCache()
        {
            mdocBillNoCache.Clear();
        }

    }

}

