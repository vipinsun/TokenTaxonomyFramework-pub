using System;
using Microsoft.Extensions.Caching.Memory;
using TTI.TTF.Taxonomy.Model;

namespace TaxonomyHost
{
	internal static class TaxonomyCache
	{
		private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

		public static void SaveToCache(string cacheKey, Taxonomy taxonomy, DateTime absoluteExpiration)
		{
			_cache.Set(cacheKey.ToLower(), taxonomy, absoluteExpiration);
		}

		public static Taxonomy GetFromCache(string cacheKey)
		{
			return _cache.Get(cacheKey.ToLower()) as Taxonomy;
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