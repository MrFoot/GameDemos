using UnityEngine;
using System.Collections;

public class TouchRemapper : MonoBehaviour {

	public class TouchData
	{
		private Vector3 position;
		
		public float DeltaTime
		{
			get;
			private set;
		}
		
		public int Priority
		{
			get;
			private set;
		}
		
		public int FingerId
		{
			get;
			private set;
		}
		
		public TouchPhase Phase
		{
			get;
			private set;
		}
		
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			protected internal set
			{
				Vector3 vector = value;
				if (vector.x == this.position.x && vector.y == this.position.y)
				{
					this.Phase = TouchPhase.Stationary;
				}
				else
				{
					this.Phase = TouchPhase.Moved;
				}
				this.position = vector;
			}
		}
		
		public void SetData(Vector3 position, int fingerId, TouchPhase phase, int priority, float deltaTime)
		{
			this.Position = position;
			this.FingerId = fingerId;
			this.Phase = phase;
			this.DeltaTime = deltaTime;
			this.Priority = priority;
		}
		
		public override string ToString()
		{
			return string.Format("[TouchWrapper: DeltaTime={0}, Priority={1}, FingerId={2}, Phase={3}, Position={4}]", new object[] {
				this.DeltaTime,
				this.Priority,
				this.FingerId,
				this.Phase,
				this.Position
			});
		}
	}
}
