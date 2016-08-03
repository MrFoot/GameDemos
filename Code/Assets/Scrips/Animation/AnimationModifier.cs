using UnityEngine;
using System.Collections;

public class AnimationModifier : MonoBehaviour {

	public bool UpdateInLateUpdate = true;
	
	public virtual void UpdateModifier()
	{
	}
	
	public void LateUpdate()
	{
		if (this.UpdateInLateUpdate)
		{
			this.UpdateModifier();
		}
	}
}
