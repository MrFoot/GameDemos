
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;


namespace FootStudio.Util
{
	public static class ResourceManager
	{
        private static string Tag = "ResourceManager";

		public class DecompressInfo
		{
			public float Progress;
			
			public bool FailedDecompressing;
			
			public bool Done;
		}
		
		private static AsyncOperation UnloadingResources;
		
		public static void UnloadUnusedResources()
		{
			if (ResourceManager.UnloadingResources == null || ResourceManager.UnloadingResources.isDone)
			{
				ResourceManager.UnloadingResources = Resources.UnloadUnusedAssets();
			}
		}
		
		public static UnityEngine.Object Load(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			UnityEngine.Object @object = Resources.Load(id);
			if (@object == null)
			{
			}
			return @object;
		}
		
		public static UnityEngine.Object Load(string id, Type type)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			UnityEngine.Object @object = Resources.Load(id, type);
			if (@object == null)
			{
			}
			return @object;
		}
		
		public static T Load<T>(string id) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(id))
			{
				return (T)((object)null);
			}
			T t = Resources.Load<T>(id);
			if (t == null)
			{
			}
			return t;
		}
		
		public static UnityEngine.Object[] LoadAll(string id)
		{
			throw new NotImplementedException();
		}

		public static IEnumerator ExtractArchive(ResourceManager.DecompressInfo decompressInfo, long requiredFreeSpace)
		{
			yield return null;

            decompressInfo.Progress = 0;

            while (decompressInfo.Progress < 1.0f)
            {
                decompressInfo.Progress += 0.1f;
                yield return null;
            }
			
			decompressInfo.Done = true;
		}
		
		public static ResourceRequest LoadAsync(string id, Type type)
		{
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            Debug.Log("LoadAsync : " + id);
            return Resources.LoadAsync(id, type);
		}
	}
}

