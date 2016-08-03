
using System;

public class SmoothDeltaTime
{
	private float[] DeltaTimes;
	
	private int Count;
	
	public float DeltaTime
	{
		get;
		private set;
	}
	
	public SmoothDeltaTime(int count)
	{
		this.DeltaTimes = new float[count];
		this.Count = 0;
		this.DeltaTime = 0f;
	}
	
	public void Reset()
	{
		this.Count = 0;
	}
	
	public void Update(float newDeltaTime)
	{
		if (this.Count >= this.DeltaTimes.Length)
		{
			for (int i = 1; i < this.DeltaTimes.Length; i++)
			{
				this.DeltaTimes[i - 1] = this.DeltaTimes[i];
			}
			this.Count--;
		}
		this.DeltaTimes[this.Count++] = newDeltaTime;
		this.DeltaTime = 0f;
		for (int j = 0; j < this.Count; j++)
		{
			this.DeltaTime += this.DeltaTimes[j];
		}
		this.DeltaTime /= (float)this.Count;
	}
}


