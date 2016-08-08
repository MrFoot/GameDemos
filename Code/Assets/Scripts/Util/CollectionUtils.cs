using System;
using System.Collections.Generic;


namespace Soulgame.Util
{
	public static class CollectionUtils
	{
		public static bool IsEmpty<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return true;
			}
			if (enumerable is ICollection<T>)
			{
				return (enumerable as ICollection<T>).Count == 0;
			}
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			return !enumerator.MoveNext();
		}
		
		public static bool IsEmpty<T>(ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}
		
		public static bool IsEmpty<T, V>(IDictionary<T, V> dictionary)
		{
			return dictionary == null || dictionary.Count == 0;
		}
		
		public static int Count<T>(ICollection<T> collection)
		{
			return (collection != null) ? collection.Count : 0;
		}
		
		public static int Count<T, V>(IDictionary<T, V> dictionary)
		{
			return (dictionary != null) ? dictionary.Count : 0;
		}
		
		public static bool HasElements<T>(ICollection<T> collection)
		{
			if (CollectionUtils.IsEmpty<T>(collection))
			{
				return false;
			}
			IEnumerator<T> enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == null)
				{
					return false;
				}
			}
			return true;
		}
		
		public static bool HasUniqueObject<T>(ICollection<T> collection)
		{
			if (CollectionUtils.IsEmpty<T>(collection))
			{
				return false;
			}
			T t = default(T);
			foreach (T current in collection)
			{
				if (t == null)
				{
					t = current;
				}
				else if (!t.Equals(current))
				{
					return false;
				}
			}
			return true;
		}
		
		public static bool Contains<T>(IEnumerable<T> enumerable, T element)
		{
			if (enumerable == null)
			{
				return false;
			}
			if (enumerable is ICollection<T>)
			{
				return (enumerable as ICollection<T>).Contains(element);
			}
			foreach (T current in enumerable)
			{
				if (object.Equals(current, element))
				{
					return true;
				}
			}
			return false;
		}
		
		public static bool Contains<T>(ICollection<T> collection, T element)
		{
			return collection != null && collection.Contains(element);
		}
		
		public static bool ContainsAll<T>(ICollection<T> targetCollection, ICollection<T> sourceCollection)
		{
			if (targetCollection == null)
			{
				throw new ArgumentNullException("targetCollection", "Collection must not be null.");
			}
			if (sourceCollection == null)
			{
				throw new ArgumentNullException("sourceCollection", "Collection must not be null.");
			}
			if (sourceCollection == targetCollection)
			{
				return true;
			}
			if (sourceCollection.Count == 0 && targetCollection.Count > 1)
			{
				return true;
			}
			foreach (T current in sourceCollection)
			{
				if (!targetCollection.Contains(current))
				{
					return false;
				}
			}
			return true;
		}
		
		public static bool EqualsAll<T>(ICollection<T> firstCollection, ICollection<T> secondCollection)
		{
			if (firstCollection == secondCollection)
			{
				return true;
			}
			if (firstCollection == null && secondCollection != null)
			{
				return false;
			}
			if (firstCollection != null && secondCollection == null)
			{
				return false;
			}
			if (firstCollection.Count == 0 && secondCollection.Count == 0)
			{
				return true;
			}
			if (firstCollection.Count != secondCollection.Count)
			{
				return false;
			}
			if (firstCollection is HashSet<T>)
			{
				return (firstCollection as HashSet<T>).SetEquals(secondCollection);
			}
			if (secondCollection is HashSet<T>)
			{
				return (secondCollection as HashSet<T>).SetEquals(firstCollection);
			}
			return CollectionUtils.ContainsAll<T>(firstCollection, secondCollection) && CollectionUtils.ContainsAll<T>(secondCollection, firstCollection);
		}
		
		public static T FindFirstMatch<T>(IEnumerable<T> source, IEnumerable<T> candidates)
		{
			if (CollectionUtils.IsEmpty<T>(source) || CollectionUtils.IsEmpty<T>(candidates))
			{
				return default(T);
			}
			foreach (T current in source)
			{
				if (CollectionUtils.Contains<T>(candidates, current))
				{
					return current;
				}
			}
			return default(T);
		}
		
		public static void AddAll<T>(ICollection<T> target, IEnumerable<T> source)
		{
			if (source == null)
			{
				return;
			}
			foreach (T current in source)
			{
				target.Add(current);
			}
		}
		
		public static void Shuffle<T>(IList<T> list)
		{
			Random random = new Random();
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = random.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}
		
		public static T EnumerateFirstElement<T>(IEnumerator<T> enumerator)
		{
			return CollectionUtils.EnumerateElementAtIndex<T>(enumerator, 0);
		}
		
		public static T EnumerateFirstElement<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable", "Enumerable must not be null.");
			}
			return CollectionUtils.EnumerateElementAtIndex<T>(enumerable.GetEnumerator(), 0);
		}
		
		public static T EnumerateElementAtIndex<T>(IEnumerator<T> enumerator, int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			T result = default(T);
			int num = 0;
			while (enumerator.MoveNext())
			{
				result = enumerator.Current;
				if (++num > index)
				{
					break;
				}
			}
			if (num < index)
			{
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}
		
		public static T EnumerateElementAtIndex<T>(IEnumerable<T> enumerable, int index)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable", "Enumerable must not be null.");
			}
			return CollectionUtils.EnumerateElementAtIndex<T>(enumerable.GetEnumerator(), index);
		}
	}
}

