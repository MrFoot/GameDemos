using UnityEngine;
using System.Collections;
using Soulgame.Util;
using Soulgame.Asset;

public class CharacterBase : AbstractCharacterBase {

    private const string _prefabPath = "Prefabs/";
    private string _modelName;

    public CharacterStateManager CharacterStateManager {
        get;
        protected set;
    }


    #region Properties

    /// <summary>
    /// 鱼所在容器（鱼缸/渔场）
    /// </summary>
    public BaseTank Room
    {
        get;
        private set;
    }

    #endregion


    public CharacterBase() {
        this.CharacterStateManager = new CharacterStateManager(this);
    }

    public CharacterBase(string model) {
        this.CharacterStateManager = new CharacterStateManager(this);
        this.Load(model);
    }

    public CharacterBase(string model,BaseTank room)
    {
        Room = room;
        this.CharacterStateManager = new CharacterStateManager(this);

        this.Load(model);
    }

	public override void Init()
	{
	}

	protected bool Load(string model) {
		return this.Load (model,false);
	}

	protected bool Load(string model, bool forceUpdate)
	{
		if (base.Model != null && !forceUpdate)
		{
			return false;
		}

        _modelName = model;
        AssetManager.LoadAssetFromResources(_prefabPath + _modelName, typeof(GameObject), OnModelLoaded);

		return true;
	}

    void OnModelLoaded(AssetManager.Asset asset) 
    {
        GameObject go = UnityEngine.Object.Instantiate(asset.AssetObject) as GameObject;
        this.Model = go.AddComponent<CharacterModel>();
        this.Model.name = asset.AssetObject.name;

        this.LoadAnimator();
        this.Reset();

        this.CharacterStateManager.Init();
    }

	public override void Unload()
	{
		base.Unload();
	}

	public override void Reset()
	{
		base.Reset();
	}

	public bool Show()
	{
		if (base.Model == null)
		{
			return false;
		}
		base.Model.Show();
		this.ForceUpdate();
		return true;
	}
	
	public bool Hide()
	{
		if (base.Model == null)
		{
			return false;
		}
		base.Model.Hide();
		return true;
	}

    public void OnApplicationPause() 
    {
 
    }

	public void OnApplicationResume()
	{
	}

	public override void OnUpdate()
	{
        this.CharacterStateManager.OnUpdate();
		base.OnUpdate();
	}

}
