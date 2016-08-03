
using System;

public abstract class CommonSceneState : BaseSceneState
{
	public BearSceneController BearSceneController
	{
		get;
		protected set;
	}


	public CommonSceneState(SceneStateManager sceneStateManager) : base(sceneStateManager)
	{
	}
	
	public override void OnEnter(BaseSceneState previousState, object data)
	{
		this.BearSceneController.OnEnter();
	}
	
	public override void OnExit(BaseSceneState nextState, object data)
	{
		this.BearSceneController.OnExit(nextState == null);;
	}
	
	public override void OnAppResume()
	{
		base.OnAppResume();
		this.BearSceneController.OnAppResume();
	}
	
	public override void OnAppPause()
	{
		base.OnAppPause();
		this.BearSceneController.OnAppPause();
	}
	
	public override void OnAction(SceneAction gameAction, object data)
	{
//		switch (gameAction)
//		{
//		case SceneAction.PokeHead:
//			this.AngelaController.OnHeadPoke();
//			return;
//		case SceneAction.PokeUpperBody:
//			this.AngelaController.OnUpperBodyPoke();
//			return;
//		case SceneAction.PokeTail:
//			this.AngelaController.OnTailPoke();
//			return;
//		case SceneAction.PokeLeftLeg:
//			this.AngelaController.OnLegPoke(true);
//			return;
//		case SceneAction.PokeRightLeg:
//			this.AngelaController.OnLegPoke(false);
//			return;
//		case SceneAction.SwipeBody:
//			this.AngelaController.OnBodySwipe();
//			return;
//		case SceneAction.PokeLowerBody:
//			this.AngelaController.OnUpperBodyPoke();
//			return;
//		case SceneAction.TriggerSound:
//		{
//			Collider collider = data as Collider;
//			if (collider != null)
//			{
//				AudioClip[] audioList = collider.GetComponent<SceneAudioTrigger>().AudioList;
//				if (audioList != null && audioList.Length > 0)
//				{
//					Main.Instance.MainAudioPlayer.PlayOneShotSoundSFX(audioList);
//				}
//			}
//			return;
//		}
//		case SceneAction.PokeLockedSticker:
//			return;
//		}
//		throw new NotImplementedException(string.Concat(new object[] {
//			"Unimplemented Game Action triggered: ",
//			gameAction,
//			" on ",
//			this
//		}));
	}
	
	public override void OnUpdate()
	{
		this.BearSceneController.OnUpdate();
	}
}


