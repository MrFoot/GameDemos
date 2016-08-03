
using System;
using UnityEngine;
using Soulgame.Util;

public class CharacterEditor
{
	private string Tag = "CharacterEditor";

	public GameObject Model
	{
		get;
		private set;
	}

	public CharacterData CharacterData
	{
		get;
		private set;
	}

	public float YOffset
	{
		get;
		set;
	}

	public void Load(string model)
	{
		if (this.Model != null)
		{
			this.Unload();
		}
		GameObject gameObject = ResourceManager.Load(model) as GameObject;
		this.Model = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		this.Model.name = gameObject.name;
		UnityEngine.Object.DontDestroyOnLoad(this.Model);
		this.CharacterData = this.Model.GetComponent<CharacterData>();
	}
	
	public void Unload()
	{
		if (this.Model == null)
		{
			return;
		}
		UnityEngine.Object.DestroyImmediate(this.Model);
	}
	
	public void Reset()
	{
		this.YOffset = 0f;
	}

	public void UpdateLevelOffsets(int level)
	{
		//设置人物位置等属性
	}
}


