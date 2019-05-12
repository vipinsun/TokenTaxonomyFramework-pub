using System;
using Microsoft.Extensions.Caching.Memory;

namespace TTI.TTF.Taxonomy
{
	internal static class TaxonomyCache
	{
		private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

		public static void SaveToCache(string cacheKey, Model.Taxonomy taxonomy, DateTime absoluteExpiration)
		{
			_cache.Set(cacheKey.ToLower(), taxonomy, absoluteExpiration);
		}

		public static Model.Taxonomy GetFromCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) as Model.Taxonomy;
		}

		public static void RemoveFromCache(string cacheKey)
		{
			_cache.Remove(cacheKey.ToLower());
		}

		public static bool IsInCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) != null;
		}
	}

}