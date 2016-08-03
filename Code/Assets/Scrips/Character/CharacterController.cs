using UnityEngine;
using System.Collections;
using Soulgame.Util;

public class CharacterController : AbstractCharacterController {

	private const string BearPrefabPath = "Bear/bear";

	private float[] StatePriority = new float[]
	{
		2f,
		8f,
		15f
	};

	private bool Hidden = true;

	private bool ForcedHidden;

	public BearCharacterAnimationEvents BearCharacterAnimationEvents;

	public bool RegisteredToEvents;

	public BearSlotController BearSlotController
	{
		get;
		private set;
	}

	public override void Init()
	{
		
	}

	protected bool Load() {
		return this.Load (false);
	}

	protected bool Load( bool forceUpdate)
	{
		if (base.CharacterEditor.Model != null && !forceUpdate)
		{
			return false;
		}
		base.CharacterEditor.Load(BearPrefabPath);
		this.BearSlotController = base.Model.GetComponent<BearSlotController>();
		this.BearSlotController.OnUpdateItemsEvent += new BearSlotController.OnUpdateItems(this.OnUpdateWardrobeItems);
		this.LoadAnimator();

		this.BearCharacterAnimationEvents = (base.CharacterAnimationEvents as BearCharacterAnimationEvents);
		this.RegisterToEvents();

		return true;
	}

	public override void Unload()
	{
		base.Unload();
		this.UnregisterToEvents();
	}

	public override void Reset()
	{
		base.Reset();

	}

	public bool Show()
	{
		if (!this.Hidden)
		{
			return false;
		}
		if (base.Model == null)
		{
			return false;
		}
		this.RegisterToEvents();
		base.Model.SetActive(true);
		this.Hidden = false;
		this.ForceUpdate();
		this.BearCharacterAnimationEvents.SoundEnabled = true;
		return true;
	}
	
	public bool Hide()
	{
		if (this.Hidden)
		{
			return false;
		}
		if (base.Model == null)
		{
			return false;
		}
		base.Model.SetActive(false);
		this.UnregisterToEvents();
		this.Hidden = true;
		this.BearCharacterAnimationEvents.SoundEnabled = false;
		return true;
	}

	private void RegisterToEvents()
	{
		if (this.RegisteredToEvents)
		{
			return;
		}
		this.RegisteredToEvents = true;
	}

	private void UnregisterToEvents()
	{
		if (!this.RegisteredToEvents)
		{
			return;
		}
		this.RegisteredToEvents = false;
	}

	public void OnGameStateEnter(Level.LevelEnum level)
	{
		this.OnGameStateEnter(level, false);
	}
	
	public void OnGameStateEnter(Level.LevelEnum level, bool forceUpdate)
	{
		this.Load(forceUpdate);
		base.SetLevel(level);
		this.Show();
		this.Reset();
		this.ForcedHidden = this.IsForceHidden();
		if (this.ForcedHidden)
		{
			this.Hide();
		}
		if ((this.ForcedHidden || this.NotInBedroom()) && !Main.Instance.MainGameLogic.IsSleeping && !Main.Instance.MainGameLogic.IsBedroomLightOn)
		{

		}
	}
	
	private bool IsForceHidden()
	{
		return Main.Instance.MainGameLogic.IsSleeping && this.NotInBedroom();
	}

	private bool NotInBedroom()
	{
		return base.LevelEnum == Level.LevelEnum.Kitchen || base.LevelEnum == Level.LevelEnum.Terrace || base.LevelEnum == Level.LevelEnum.Bathroom;
	}

	protected void OnUpdateWardrobeItems()
	{
		this.UpdateTransformOverride();
	}

	public void UpdateTransformOverride()
	{

	}

	public void OnApplicationResume()
	{
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

}
