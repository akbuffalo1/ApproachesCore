using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AD
{
	public static class EnumerableExtensions
	{
	    public static T PickRandom<T>(this IEnumerable<T> source)
	    {
	        return source.PickRandom(1).Single();
	    }
	
	    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
	    {
	        return source.Shuffle().Take(count);
	    }
	
	    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
	    {
	        return source.OrderBy(x => Guid.NewGuid());
	    }
    
	    public static IEnumerable<T> ShuffleYates<T>(this IEnumerable<T> source)
	    {
	        return source.ShuffleYates(new Random());
	    }
		
	    public static IEnumerable<T> ShuffleYates<T>(this IEnumerable<T> source, Random rng)
	    {
	        if (source == null) throw new ArgumentNullException("source");
	        if (rng == null) throw new ArgumentNullException("rng");
	
	        return source.ShuffleIterator(rng);
	    }
	
	    private static IEnumerable<T> ShuffleIterator<T>(
	        this IEnumerable<T> source, Random rng)
	    {
	        var buffer = source.ToList();
	        for (int i = 0; i < buffer.Count; i++)
	        {
	            int j = rng.Next(i, buffer.Count);
	            yield return buffer[j];
	
	            buffer[j] = buffer[i];
	        }
	    }

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if (collection == null || action == null)
			{
				return;
			}
			
			foreach (var item in collection)
			{
				action(item);
			}
		}

		public static void ForEach<T>(this IEnumerable collection, Action<T> action)
		{
			collection.OfType<T>().ForEach(action);
		}

		public static int IndexOf(this IEnumerable collection, object item)
		{
			return collection.OfType<object>().ToList().IndexOf(item);
		}

		public static int Count(this IEnumerable collection)
		{
			var count = 0;
		    if (collection != null)
		    {
		        foreach (var item in collection)
		        {
		            count++;
		        }
		    }

		    return count;
		}

		public static object ElementAt(this IEnumerable collection, int index)
		{
			return collection.OfType<object>().ElementAtOrDefault(index);
		}
	}
}

