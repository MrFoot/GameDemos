using UnityEngine;
using System.Collections;
using Soulgame.Util;
using System;

public abstract class AbstractCharacterController {

	protected enum NewItemEnum
	{
		None,
		Wardrobe,
		Room
	}

	public enum Attribute
	{
		CanTalk,
		CanLook,
		Sleeping,
		NonInterruptible,
	}

	private bool AppStarted = false;

	protected AbstractCharacterController.NewItemEnum NewItem;

	protected int MainLayerAttributeParameter;
	
	protected int BaseLayerAttributeParameter;

	protected int NewItemParameter;

	protected int PoseParameter;

	protected int RandomParameter;

	public int MainLayerIndex;

	public CharacterEditor CharacterEditor
	{
		get;
		private set;
	}

	public Animator Animator
	{
		get;
		private set;
	}

	public CharacterAnimationEvents CharacterAnimationEvents
	{
		get;
		private set;
	}

	public GameObject Model
	{
		get
		{
			return this.CharacterEditor.Model;
		}
	}

	public Level.LevelEnum LevelEnum
	{
		get;
		protected set;
	}

	public bool NonInterruptible
	{
		get
		{
			return this.Animator != null;
		}
	}

	public AbstractCharacterController()
	{
		this.CharacterEditor = new CharacterEditor();
	}
	
	public abstract void Init();

	public void SetLevel(Level.LevelEnum level)
	{
		this.LevelEnum = level;
	}

	protected virtual void LoadAnimator()
	{
		this.Animator = this.CharacterEditor.Model.GetComponent<Animator>();
		this.CharacterAnimationEvents = this.CharacterEditor.Model.GetComponent<CharacterAnimationEvents> ();
	}

	public virtual void Unload()
	{
		this.CharacterEditor.Unload();
	}
		

	public virtual void OnUpdate()
	{
	}

	public void ResetAllIKs()
	{

	}
	
	public void ResetLookAtAngle()
	{

	}
	
	public virtual void ResetCloth()
	{
		Cloth[] componentsInChildren = this.CharacterEditor.Model.GetComponentsInChildren<Cloth>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ClearTransformMotion();
		}
	}
	
	public virtual void Reset()
	{
		this.ResetCloth();
		this.ResetAllIKs();
		if (!this.AppStarted)
		{
			this.AppStarted = true;

		}

		this.CharacterEditor.Reset();
	}
	
	public virtual void ForceUpdate()
	{
		
	}

	public void SetRun(bool enable) {
		this.Animator.SetBool ("Run", enable);
	}

}
