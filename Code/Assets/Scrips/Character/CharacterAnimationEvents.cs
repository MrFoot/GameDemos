using UnityEngine;
using System.Collections;

public class CharacterAnimationEvents : MonoBehaviour {

	private bool soundEnabled = true;

	private const float DefaultAudioClipFadeOutTime = 0.02f;
	
	protected const string TAG = "CharacterAnimationEvents";
	
	private const string AudioPath = "Audio/Bear/";

	public bool SoundEnabled
	{
		get
		{
			return this.soundEnabled;
		}
		set
		{
			this.soundEnabled = value;
			if (!this.soundEnabled)
			{
			}
		}
	}

	protected virtual void Awake()
	{
		this.SoundEnabled = true;
	}
	
	protected virtual void Reset()
	{
	}
	
	protected virtual void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			this.soundEnabled = false;
		}
		else
		{
			this.soundEnabled = true;
		}
	}
	
	public virtual void Update()
	{
	}
	
	public virtual void OnDestroy()
	{
		this.SoundEnabled = false;
	}
	
	public virtual void LateUpdate()
	{
		if (this.SoundEnabled)
		{

		}
	}

	public virtual void OnEnter()
	{
		this.SoundEnabled = true;
	}
	
	public virtual void OnExit()
	{
	}
}
