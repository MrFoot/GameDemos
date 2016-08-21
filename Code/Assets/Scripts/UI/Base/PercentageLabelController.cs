using System;
using UnityEngine;

public class PercentageLabelController : MonoBehaviour
{
	//public UILabel Label;

	//public UIWidget Container;

	public float FadeSpeed = 1f;

	public bool Animating;

	private float percent;

	private float StartAnimTime = 3.40282347E+38f;

	private float FadeDelayTime = 1.5f;

	public float Percent
	{
		get
		{
			return this.percent;
		}
		set
		{
			if (Mathf.RoundToInt(this.percent * 100f) == Mathf.RoundToInt(value * 100f))
			{
				return;
			}
			this.SetPercent(value);
		}
	}

	private void Awake()
	{
		//this.Container.alpha = 0f;
	}

	public void ForceShowPercent(float v)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.SetPercent(v);
	}

	public void ForceHidePercent()
	{
		//this.Container.alpha = 0f;
	}

	private void SetPercent(float targetPercent)
	{
		this.percent = targetPercent;
		//this.Label.text = Mathf.RoundToInt(this.percent * 100f) + "%";
		this.StartAnimTime = Time.time + this.FadeDelayTime;
		//this.Container.alpha = 1f;
	}

	private void Update()
	{
        /*
		if (this.Container.alpha > 0f && Time.time > this.StartAnimTime)
		{
			this.Container.alpha = Mathf.MoveTowards(this.Container.alpha, 0f, Time.deltaTime * this.FadeSpeed);
		}
         * */
	}
}


