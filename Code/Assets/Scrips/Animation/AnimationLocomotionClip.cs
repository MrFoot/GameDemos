
using System;
using UnityEngine;

public class AnimationLocomotionClip : ScriptableObject
{
	public enum AnimationCurveType
	{
		PositionX,
		PositionY,
		PositionZ,
		RotationX,
		RotationY,
		RotationZ,
		RotationW,
		ScaleX,
		ScaleY,
		ScaleZ,
		Last
	}
	
	public AnimationCurve[] AnimationCurves;
	
	public float Lenght;
	
	public void GetTransformAtTime(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale, float time)
	{
		int num = 0;
		int i = 0;
		while (i < 3)
		{
			position[i] = this.AnimationCurves[num].Evaluate(time);
			i++;
			num++;
		}
		int j = 0;
		while (j < 4)
		{
			rotation[j] = this.AnimationCurves[num].Evaluate(time);
			j++;
			num++;
		}
		int k = 0;
		while (k < 3)
		{
			scale[k] = this.AnimationCurves[num].Evaluate(time);
			k++;
			num++;
		}
	}
}

