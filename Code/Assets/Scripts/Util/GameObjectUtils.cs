using System;
using UnityEngine;

namespace Soulgame.Util
{
	public static class GameObjectUtils
	{
		public static GameObject CreateEmpty(string name, GameObject parent)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			if (parent != null)
			{
				gameObject.transform.parent = parent.transform;
			}
			return gameObject;
		}

		public static GameObject CreatePrimitive(string name, GameObject parent, PrimitiveType type)
		{
			GameObject gameObject = GameObject.CreatePrimitive(type);
			gameObject.name = name;
			if (parent != null)
			{
				gameObject.transform.parent = parent.transform;
			}
			return gameObject;
		}

		public static GameObject CreateFromResource(string resourcePath, string name, GameObject parent)
		{
			GameObject gameObject = ResourceManager.Load(resourcePath) as GameObject;
			Assert.NotNull(gameObject, "No resource exists at the specified path: {0}", resourcePath, new object[0]);
			GameObject gameObject2 = GameObjectUtils.Clone(gameObject, name);
			if (parent != null)
			{
				gameObject2.transform.parent = parent.transform;
			}
			return gameObject2;
		}

		public static GameObject Clone(GameObject original, string name)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			gameObject.name = name;
			return gameObject;
		}

		public static GameObject Clone(GameObject original, string name, Vector3 position, Quaternion quaternion)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original, position, quaternion) as GameObject;
			gameObject.name = name;
			return gameObject;
		}
	}
}

