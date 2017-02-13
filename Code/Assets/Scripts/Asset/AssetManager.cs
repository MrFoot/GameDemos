using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Soulgame.Util;


namespace Soulgame.Asset
{
	public class AssetManager
	{
		public enum State
		{
			Invalid,
			Loading,
			Loaded,
			Failed
		}
		
		public class Bundle
		{
			public string Name;
			
			public WWW WWW;
			
			public AssetBundle AssetBundle;
			
			public AssetManager.State State;
			
			public event AssetManager.OnBundleLoaded BundleLoaded;
			
			public void OnLoaded()
			{
				if (this.BundleLoaded == null)
				{
					return;
				}
				this.BundleLoaded(this);
			}
			
			public override string ToString()
			{
				return string.Format("Bundle:({0}, {1}, {2})", this.Name, this.AssetBundle.ToString(), this.State.ToString());
			}
		}

		public class Asset
		{
			public string Name;
			
			public UnityEngine.Object AssetObject;
			
			public Type Type;
			
			public AssetManager.Bundle Bundle;
			
			public AssetManager.State State;
			
			public int MemoryUsed;
			
			public event AssetManager.OnAssetLoaded AssetLoaded;
			
			public void OnLoaded()
			{
				if (this.AssetLoaded == null)
				{
					return;
				}
				this.AssetLoaded(this);
			}
			
			public void OnFailed()
			{
				if (this.AssetLoaded == null)
				{
					return;
				}
				this.AssetLoaded(this);
			}
			
			public void ClearEvents()
			{
				this.AssetLoaded = null;
			}
			
			public override string ToString()
			{
				return string.Format("[Asset: Name={0}, AssetObject={1}, Type={2}, Bundle={3}, State={4}]", new object[] {
					this.Name,
					this.AssetObject,
					this.Type,
					this.Bundle,
					this.State
				});
			}
		}

		private class AsyncActiveItem
		{
			public AssetManager.Asset Asset;
			
			public AssetBundleRequest AssetBundleRequest;
			
			public WWW WWW;
			
			public ResourceRequest ResourceRequest;
		}

		private delegate AssetManager.Asset LoadAssetDelegate(AssetManager.Asset asset);
		
		public delegate void OnBundleLoaded(AssetManager.Bundle bundle);
		
		public delegate void OnAssetLoaded(AssetManager.Asset asset);

		private const int MaxActiveItems = 1;
		
		private static string TAG = "AssetManager";
		

        //所有包（包括异步加载的）
		private static List<AssetManager.Bundle> Bundles = new List<AssetManager.Bundle>();
		
		private static List<AssetManager.Bundle> AsyncBundles = new List<AssetManager.Bundle>();

		private static Dictionary<string, AssetManager.Asset> Assets = new Dictionary<string, AssetManager.Asset>();
		
        //等待异步加载列表
		private static List<AssetManager.Asset> AsyncQueueAssets = new List<AssetManager.Asset>();
		
        //异步加载完成列表
		private static List<AssetManager.AsyncActiveItem> AsyncActiveItems = new List<AssetManager.AsyncActiveItem>(1);

		public static bool DebugInfo = false;
		
		private static Vector2 DebugScrollViewPosition = Vector2.zero;

		public static int ActiveAssetsCount
		{
			get
			{
				return AssetManager.Assets.Count;
			}
		}
		
		private static void AssetLoaded(AssetManager.Asset asset)
		{
			asset.State = AssetManager.State.Loaded;
			asset.OnLoaded();
		}
		
		private static void AssetFailed(AssetManager.Asset asset)
		{
			asset.State = AssetManager.State.Failed;
			asset.OnFailed();
		}

		public static string GetAssetNameFromPath(string path)
		{
			return path.Remove(path.LastIndexOf(".")).Replace("Assets/Resources/", string.Empty);
            //string s1 = path.Remove(0,path.LastIndexOf("/")+1);
            //return s1.Remove(s1.LastIndexOf("."));
		}
		
		public static string GetStreamingAssetPath(string path)
		{
			return string.Format("{0}/{1}", Application.streamingAssetsPath, path);
		}
		
		public static string GetStreamingAssetWWWPath(string path)
		{
            return "file://" + AssetManager.GetStreamingAssetPath(path);
		}
		
		public static string GetBundleAssetName(string name)
		{
			return Path.GetFileName(name);
		}
		
		public static void Initialize()
		{
			Caching.CleanCache();
		}

        // Speed : LoadAsyncBundle < LoadBundle
		public static AssetManager.Bundle LoadAsyncBundle(string name, AssetManager.OnBundleLoaded bundleLoaded)
		{
			int num = AssetManager.Bundles.FindIndex((AssetManager.Bundle b) => b.Name == name);
			if (num != -1)
			{
				if (AssetManager.Bundles[num].State != AssetManager.State.Loaded)
				{
					AssetManager.Bundles[num].BundleLoaded += bundleLoaded;
				}
				return AssetManager.Bundles[num];
			}
			WWW wWW = WWW.LoadFromCacheOrDownload(AssetManager.GetStreamingAssetWWWPath(name), 1);
			if (wWW == null)
			{
				GameLog.ErrorT(AssetManager.TAG, "Asset bundle load failed: {0}!", new object[] {
					name
				});
				return null;
			}
			AssetManager.Bundle bundle = new AssetManager.Bundle();
			bundle.Name = name;
			bundle.WWW = wWW;
			bundle.State = AssetManager.State.Loading;
			bundle.BundleLoaded += bundleLoaded;
			AssetManager.Bundles.Add(bundle);
			AssetManager.AsyncBundles.Add(bundle);
			return bundle;
		}

		public static AssetManager.Bundle LoadBundle(string name, AssetManager.OnBundleLoaded bundleLoaded)
		{
			int num = AssetManager.Bundles.FindIndex((AssetManager.Bundle b) => b.Name == name);
			if (num != -1)
			{
				if (AssetManager.Bundles[num].State != AssetManager.State.Loaded)
				{
					AssetManager.Bundles[num].BundleLoaded += bundleLoaded;
				}
				return AssetManager.Bundles[num];
			}
			AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetManager.GetStreamingAssetPath(name));
			if (assetBundle == null)
			{
				GameLog.ErrorT(AssetManager.TAG, "Asset bundle load failed: {0}!", new object[]{
					name
				});
				return null;
			}
			AssetManager.Bundle bundle = new AssetManager.Bundle();
			bundle.Name = name;
			bundle.AssetBundle = assetBundle;
			bundle.State = AssetManager.State.Loaded;
			bundle.BundleLoaded += bundleLoaded;
			AssetManager.Bundles.Add(bundle);
			bundle.OnLoaded();
			return bundle;
		}

        public static void UnloadBundle(AssetManager.Bundle bundle, bool unloadAllLoadedObjects)
		{
            bundle.AssetBundle.Unload(unloadAllLoadedObjects);
			bundle.AssetBundle = null;
			bundle.State = AssetManager.State.Invalid;
			AssetManager.Bundles.Remove(bundle);
		}

		public static AssetManager.Asset FindAsset(string name)
		{
			AssetManager.Asset result;
			if (AssetManager.Assets.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}
		
		private static AssetManager.Asset LoadAsset(AssetManager.Bundle bundle, string name, Type type, AssetManager.OnAssetLoaded assetLoaded, AssetManager.LoadAssetDelegate OnLoadAsset)
		{
			AssetManager.Asset asset = AssetManager.FindAsset(name);
			if (asset != null)
			{
				if (asset.State == AssetManager.State.Loaded)  //已经正确加载，重定向回调
				{
					asset.ClearEvents();
					asset.AssetLoaded += assetLoaded;
					asset.OnLoaded();
					return asset;
				}
				AssetManager.StopLoadAsyncAsset(asset);
			}
			else
			{
				asset = new AssetManager.Asset();
				AssetManager.Assets.Add(name, asset);
			}
			asset.Name = name;
			asset.Type = type;
			asset.Bundle = bundle;
            asset.ClearEvents();   //如果先请求loadAsset，然后才loadBundle，不管前面的无效请求，只管loadBundle之后的loadAsset
			asset.AssetLoaded += assetLoaded;
			return OnLoadAsset(asset);
		}

		private static AssetManager.Asset OnLoadAssetFromResources(AssetManager.Asset asset)
		{
			asset.AssetObject = ResourceManager.Load(asset.Name, asset.Type);
			if (asset.AssetObject == null)
			{
				AssetManager.AssetFailed(asset);
			}
			else
			{
				AssetManager.AssetLoaded(asset);
			}
			return asset;
		}
		
		private static AssetManager.Asset OnLoadAssetFromEditorResources(AssetManager.Asset asset)
		{
			Assert.IsTrue(false, "Can't use LoadAssetFromEditorResources in non-editor mode!", new object[0]);
			return asset;
		}
		
		private static AssetManager.Asset OnLoadAsyncAsset(AssetManager.Asset asset)
		{
			AssetManager.AsyncQueueAssets.Add(asset);
			return asset;
		}
		
		private static AssetManager.Asset OnLoadAssetFromBundle(AssetManager.Asset asset)
		{
			asset.AssetObject = asset.Bundle.AssetBundle.LoadAsset(AssetManager.GetBundleAssetName(asset.Name), asset.Type);
			if (asset.AssetObject == null)
			{
				AssetManager.AssetFailed(asset);
			}
			else
			{
				AssetManager.AssetLoaded(asset);
			}
			return asset;
		}

        // Speed : LoadAssetFromResources > LoadAsyncAssetFromResources
		public static AssetManager.Asset LoadAssetFromResources(string name, Type type, AssetManager.OnAssetLoaded assetLoaded)
		{
			return AssetManager.LoadAsset(null, name, type, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAssetFromResources));
		}
		
		public static AssetManager.Asset LoadAsyncAssetFromResources(string name, Type type, AssetManager.OnAssetLoaded assetLoaded)
		{
			return AssetManager.LoadAsset(null, name, type, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAsyncAsset));
		}
		
		public static AssetManager.Asset LoadAssetFromEditorResources(string name, Type type, AssetManager.OnAssetLoaded assetLoaded)
		{
			return AssetManager.LoadAsset(null, name, type, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAssetFromEditorResources));
		}

        //Speed : LoadAsyncAssetFromBundle >>> LoadAssetFromBundle 
		public static AssetManager.Asset LoadAssetFromBundle(AssetManager.Bundle bundle, string name, Type type, AssetManager.OnAssetLoaded assetLoaded)
		{
			return AssetManager.LoadAsset(bundle, name, type, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAssetFromBundle));
		}
		
		public static AssetManager.Asset LoadAsyncAssetFromBundle(AssetManager.Bundle bundle, string name, Type type, AssetManager.OnAssetLoaded assetLoaded)
		{
			return AssetManager.LoadAsset(bundle, name, type, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAsyncAsset));
		}
		
		public static AssetManager.Asset LoadImage(string name, AssetManager.OnAssetLoaded assetLoaded)
		{
			string name2 = (name.IndexOf(':') == -1) ? AssetManager.GetStreamingAssetWWWPath(name) : name;
			return AssetManager.LoadAsset(null, name2, null, assetLoaded, new AssetManager.LoadAssetDelegate(AssetManager.OnLoadAsyncAsset));
		}
		
		public static void StopLoadAsyncAsset(AssetManager.Asset asset)
		{
			if (asset.State == AssetManager.State.Loaded)
			{
				return;
			}
			int num = AssetManager.AsyncQueueAssets.FindIndex((AssetManager.Asset a) => a == asset);
			if (num != -1)
			{
				AssetManager.AsyncQueueAssets.RemoveAt(num);
				return;
			}
			num = AssetManager.AsyncActiveItems.FindIndex((AssetManager.AsyncActiveItem i) => i.Asset == asset);
			if (num != -1)
			{
				AssetManager.AsyncActiveItems.RemoveAt(num);
				return;
			}
		}
		
		public static bool UnloadAsset(string name)
		{
			AssetManager.Asset asset;
			if (!AssetManager.Assets.TryGetValue(name, out asset))
			{
				return false;
			}
			AssetManager.UnloadAsset(asset);
			return true;
		}
		
		public static bool UnloadAsset(AssetManager.Asset asset)
		{
			AssetManager.StopLoadAsyncAsset(asset);
			if (!AssetManager.Assets.Remove(asset.Name))
			{
				return false;
			}
			if (asset.Bundle == null && (asset.Type == typeof(AnimationClip) || asset.Type == typeof(Texture2D) || asset.Type == typeof(Texture3D)))
			{
				Resources.UnloadAsset(asset.AssetObject);
			}
			asset.AssetObject = null;
			asset.Type = null;
			asset.Bundle = null;
			asset.State = AssetManager.State.Invalid;
			return true;
		}
		
		public static bool isAsyncLoading()
		{
			return AssetManager.AsyncActiveItems.Count > 0 || AssetManager.AsyncQueueAssets.Count > 0 || AssetManager.AsyncBundles.Count > 0;
		}

		private static void UpdateBundleLoading()
		{
			for (int i = 0; i < AssetManager.AsyncBundles.Count; i++)
			{
				AssetManager.Bundle bundle = AssetManager.AsyncBundles[i];
				if (bundle.WWW != null && bundle.WWW.isDone)
				{
					if (bundle.WWW.error != null && bundle.WWW.error.Length > 0)
					{
						GameLog.ErrorT(AssetManager.TAG, "Asset bundle load failed: {0} {1}!", new object[]{
							bundle.Name,
							bundle.WWW.error
						});
						bundle.State = AssetManager.State.Failed;
					}
					else
					{
						bundle.AssetBundle = bundle.WWW.assetBundle;
						bundle.WWW = null;
						bundle.State = AssetManager.State.Loaded;
						bundle.OnLoaded();
					}
					AssetManager.AsyncBundles.RemoveAt(i--);
				}
			}
		}
		
		private static void UpdateAssetLoading()
		{
			for (int i = 0; i < AssetManager.AsyncActiveItems.Count; i++)
			{
				AssetManager.AsyncActiveItem asyncActiveItem = AssetManager.AsyncActiveItems[i];
				bool flag = false;
				bool flag2 = false;
				if (asyncActiveItem.Asset.Type == null)
				{
					if (asyncActiveItem.WWW.isDone)
					{
						asyncActiveItem.Asset.AssetObject = asyncActiveItem.WWW.textureNonReadable;
						flag = true;
					}
				}
				else if (asyncActiveItem.AssetBundleRequest != null)
				{
					if (asyncActiveItem.AssetBundleRequest.isDone)
					{
						asyncActiveItem.Asset.AssetObject = asyncActiveItem.AssetBundleRequest.asset;
						if (asyncActiveItem.Asset.AssetObject == null)
						{
							flag2 = true;
						}
						flag = true;
					}
				}
				else if (asyncActiveItem.ResourceRequest != null && asyncActiveItem.ResourceRequest.isDone)
				{
					asyncActiveItem.Asset.AssetObject = asyncActiveItem.ResourceRequest.asset;
					if (asyncActiveItem.Asset.AssetObject == null)
					{
						flag2 = true;
					}
					flag = true;
				}
				if (flag)
				{
					AssetManager.AsyncActiveItems.RemoveAt(i--);
					if (flag2)
					{
						AssetManager.AssetFailed(asyncActiveItem.Asset);
					}
					else
					{
						AssetManager.AssetLoaded(asyncActiveItem.Asset);
					}
				}
			}
			if (AssetManager.AsyncQueueAssets.Count > 0)
			{
				int num = Mathf.Min(1 - AssetManager.AsyncActiveItems.Count, AssetManager.AsyncQueueAssets.Count);
				for (int j = 0; j < num; j++)
				{
					AssetManager.Asset asset = AssetManager.AsyncQueueAssets[0];
					AssetManager.AsyncActiveItem asyncActiveItem2 = new AssetManager.AsyncActiveItem();
					asyncActiveItem2.Asset = asset;
					bool flag3 = false;
					if (asset.Type == null)
					{
						asyncActiveItem2.WWW = new WWW(asset.Name);
						if (asyncActiveItem2.WWW != null)
						{
							flag3 = true;
						}
					}
					else if (asset.Bundle != null)
					{
						if (asset.Bundle.State == AssetManager.State.Loaded)
						{
							asyncActiveItem2.AssetBundleRequest = asset.Bundle.AssetBundle.LoadAssetAsync(AssetManager.GetBundleAssetName(asset.Name), asset.Type);
							if (asyncActiveItem2.AssetBundleRequest != null)
							{
								flag3 = true;
							}
						}
					}
					else
					{
						asyncActiveItem2.ResourceRequest = ResourceManager.LoadAsync(asset.Name, asset.Type);
						if (asyncActiveItem2.ResourceRequest != null)
						{
							flag3 = true;
						}
					}
					if (flag3)
					{
						AssetManager.AsyncActiveItems.Add(asyncActiveItem2);
					}
					else
					{
						AssetManager.AssetFailed(asyncActiveItem2.Asset);
					}
					AssetManager.AsyncQueueAssets.RemoveAt(0);
				}
			}
		}
		
		public static void Update()
		{
			AssetManager.UpdateBundleLoading();
			AssetManager.UpdateAssetLoading();
		}
		
		public static void OnGUI()
		{
			if (AssetManager.DebugInfo)
			{
				float num = 10f;
				float top = (float)Screen.height / 4f;
				float width = (float)Screen.width - 2f * num;
				float height = (float)Screen.height / 2f;
				GUILayout.BeginArea(new Rect(num, top, width, height), GUI.skin.box);
				AssetManager.DebugScrollViewPosition = GUILayout.BeginScrollView(AssetManager.DebugScrollViewPosition, true, true, new GUILayoutOption[]
				                                                                 {
					GUILayout.Width(width),
					GUILayout.Height(height)
				});
				GUI.color = Color.white;
				GUILayout.Label(string.Format("ASSET DEBUG\nActiveAssets:{0}\nAsyncActiveItems:{1}\nAsyncQueueAssets:{2}\n", AssetManager.ActiveAssetsCount, AssetManager.AsyncActiveItems.Count, AssetManager.AsyncQueueAssets.Count), new GUILayoutOption[0]);
                GUILayout.Label(string.Format("Bundles:{0}\nAsyncBundles:{1}\n", AssetManager.Bundles.Count, AssetManager.AsyncBundles.Count), new GUILayoutOption[0]);
				int num2 = 0;
				GUI.color = Color.yellow;
				foreach (KeyValuePair<string, AssetManager.Asset> current in AssetManager.Assets)
				{
					AssetManager.Asset value = current.Value;
					if (value.State == AssetManager.State.Loaded)
					{
						GUILayout.Label(string.Format("{0}: {1}", num2++, value.Name), new GUILayoutOption[0]);
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
			}
		}
	}
}

