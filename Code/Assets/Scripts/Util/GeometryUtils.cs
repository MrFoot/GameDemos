
using System;
using UnityEngine;


namespace FootStudio.Util
{
	public static class GeometryUtils
	{
		public static readonly Vector3[] Axis = new Vector3[]
		{
			Vector2.right,
			Vector3.up,
			Vector3.forward
		};
		
		public static Vector3 GetClosestPointOnLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
		{
			Vector3 vector = lineEnd - lineStart;
			float magnitude = vector.magnitude;
			if (magnitude < Mathf.Epsilon)
			{
				return lineStart;
			}
			vector /= magnitude;
			Vector3 rhs = point - lineStart;
			float value = Vector3.Dot(vector, rhs);
			return lineStart + vector * Mathf.Clamp(value, 0f, magnitude);
		}
		
		public static bool IsPointInCapsule(Vector3 point, Vector3 start, Vector3 end, float radius)
		{
			Vector3 closestPointOnLineSegment = GeometryUtils.GetClosestPointOnLineSegment(point, start, end);
			float num = Vector3.Distance(point, closestPointOnLineSegment);
			return num <= radius;
		}
		
		public static bool IsPointInBox(Vector3 point, Vector3 center, Vector3 extents)
		{
			Vector3 vector = point - center;
			return Mathf.Abs(vector.x) <= extents.x && Mathf.Abs(vector.y) <= extents.y && Mathf.Abs(vector.z) <= extents.z;
		}
		
		public static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
		{
			float num = Vector3.Distance(center, point);
			return num <= radius;
		}
		
		private static bool CheckLiangBrasky(float p, float q, ref float u1, ref float u2)
		{
			if (p == 0f && q < 0f)
			{
				return false;
			}
			float num = q / p;
			if (p < 0f && u1 < num)
			{
				u1 = num;
			}
			if (p > 0f && u2 > num)
			{
				u2 = num;
			}
			return true;
		}
		
		public static bool IntersectLineRectangle(Vector2 a, Vector2 b, Rect rect, out Vector2 intersection)
		{
			Vector2 a2 = b - a;
			float num = -100000f;
			float num2 = 100000f;
			intersection = Vector2.zero;
			if (!GeometryUtils.CheckLiangBrasky(-a2.x, a.x - rect.xMin, ref num, ref num2) || !GeometryUtils.CheckLiangBrasky(a2.x, rect.xMax - a.x, ref num, ref num2) || !GeometryUtils.CheckLiangBrasky(-a2.y, a.y - rect.yMin, ref num, ref num2) || !GeometryUtils.CheckLiangBrasky(a2.y, rect.yMax - a.y, ref num, ref num2))
			{
				return false;
			}
			if (num > num2 || num > 1f || num < 0f)
			{
				return false;
			}
			intersection = a + a2 * num;
			return true;
		}
		
		public static bool IntersectLineSphere(Vector3 lineStart, Vector3 lineEnd, Vector3 spherePosition, float sphereRadius, out Vector3 hitPoint)
		{
			Vector3 vector = lineEnd - lineStart;
			Vector3 vector2 = lineStart - spherePosition;
			float num = Vector3.Dot(vector2, vector2) - sphereRadius * sphereRadius;
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = num2 * num2 - num;
			if (num3 < 0f)
			{
				hitPoint = Vector3.zero;
				return false;
			}
			if (num3 >= Mathf.Epsilon)
			{
				float num4 = Mathf.Sqrt(num3);
				float d = -num2 - num4;
				hitPoint = lineStart + d * vector;
				return true;
			}
			float d2 = -num2;
			hitPoint = lineStart + d2 * vector;
			return true;
		}
		
		public static bool IntersectLineLine2D(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B, out Vector2 intersectionPoint, float epsilon)
		{
			Vector2 a = line1B - line1A;
			Vector2 vector = line2B - line2A;
			Vector2 vector2 = line1A - line2A;
			float num = 1f / (-vector.x * a.y + a.x * vector.y);
			float num2 = (-a.y * vector2.x + a.x * vector2.y) * num;
			float num3 = (vector.x * vector2.y - vector.y * vector2.x) * num;
			float num4 = 1f - epsilon;
			if (num2 >= epsilon && num2 <= num4 && num3 >= epsilon && num3 <= num4)
			{
				intersectionPoint = line1A + a * num3;
				return true;
			}
			intersectionPoint = Vector2.zero;
			return false;
		}
		
		public static bool VectorConeClamp(Vector3 initialiDirection, float angle, ref Vector3 direction)
		{
			float num = Mathf.Cos(angle * 0.0174532924f);
			if (Vector3.Dot(initialiDirection, direction) > num)
			{
				return false;
			}
			Vector3 axis = Vector3.Cross(initialiDirection, direction);
			if (axis.sqrMagnitude < Mathf.Epsilon)
			{
				axis = Vector3.up;
			}
			Quaternion rotation = Quaternion.AngleAxis(angle, axis);
			direction = rotation * initialiDirection;
			return true;
		}
	}
}

