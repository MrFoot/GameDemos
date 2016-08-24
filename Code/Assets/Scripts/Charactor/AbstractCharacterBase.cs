using UnityEngine;
using System.Collections;
using Soulgame.Util;
using System;

public abstract class AbstractCharacterBase : MonoBehaviour{

	protected int BaseLayer;

	public Animator Animator
	{
		get;
		private set;
	}

    public CharacterModel Model {
        get;
        protected set;
    }

	public AbstractCharacterBase()
	{
        
	}
	
	public abstract void Init();

	protected virtual void LoadAnimator()
	{
        this.Animator = this.Model.Animator;

        if (this.Animator != null)
		    this.BaseLayer = this.Animator.GetLayerIndex ("Base Layer");
	}

	public virtual void Unload()
	{
        if (this.Model == null)
        {
            return;
        }
        UnityEngine.Object.DestroyImmediate(this.Model.gameObject);
        this.Model = null;
        Animator = null;
	}

	public virtual void OnUpdate()
	{
	}
		
	public virtual void Reset()
	{
        this.Model.Reset();
	}

	public virtual void ForceUpdate()
	{
		
	}

	public void ResetIdle() {

	}

}
