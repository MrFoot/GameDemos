
using System;
namespace Soulgame.Util
{
	public class Pair<F, S>
	{
		public F First
		{
			get;
			set;
		}
		
		public S Second
		{
			get;
			set;
		}
		
		public Pair()
		{
		}
		
		public Pair(F first, S second)
		{
			this.First = first;
			this.Second = second;
		}
		
		public override string ToString()
		{
			return string.Format("[Pair: First={0}, Second={1}]", this.First, this.Second);
		}
	}
}

