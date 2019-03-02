namespace Zone.UmbracoPersonalisationGroups.Common.Helpers
{
    using System;
    using System.Web;
    using System.Web.Caching;

    public static class RuntimeCacheHelper
    {
        public static T GetCacheItem<T>(string cacheKey, Func<T> getCacheItem) where T : class
        {
            var result = HttpRuntime.Cache[cacheKey] as T;
            if (result != null)
            {
                return result;
            }

            result = getCacheItem();
            if (result != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            }

            return result;
        }
    }
}
