
using System;
using UnityEngine;


namespace Soulgame.Util
{
	public class TriangleUtils
	{
		public static Vector2 GetBarycentricFromPosition(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			Vector3 vector = c - a;
			Vector3 vector2 = b - a;
			Vector3 rhs = p - a;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector, vector2);
			float num3 = Vector3.Dot(vector, rhs);
			float num4 = Vector3.Dot(vector2, vector2);
			float num5 = Vector3.Dot(vector2, rhs);
			float num6 = 1f / (num * num4 - num2 * num2);
			float x = (num4 * num3 - num2 * num5) * num6;
			float y = (num * num5 - num2 * num3) * num6;
			return new Vector2(x, y);
		}
		
		public static Vector3 GetPositionFromBarycentric(Vector3 a, Vector3 b, Vector3 c, Vector2 bc)
		{
			Vector3 a2 = b - a;
			Vector3 a3 = c - a;
			return a + bc.x * a2 + bc.y * a3;
		}
		
		public static float GetTriangleArea(Vector3 a, Vector3 b, Vector3 c)
		{
			float num = Vector3.Distance(a, b);
			float num2 = Vector3.Distance(b, c);
			float num3 = Vector3.Distance(c, a);
			float num4 = (num + num2 + num3) * 0.5f;
			return Mathf.Sqrt(num4 * (num4 - num) * (num4 - num2) * (num4 - num3));
		}
		
		public static bool IsPositionInTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p, float epsilon)
		{
			Vector2 barycentricFromPosition = TriangleUtils.GetBarycentricFromPosition(a, b, c, p);
			return barycentricFromPosition.x >= -epsilon && barycentricFromPosition.y >= -epsilon && barycentricFromPosition.x + barycentricFromPosition.y <= 1f + epsilon;
		}
		
		public static Vector3 GetClosestPointOnTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p, out Vector2 bc)
		{
			Vector3 lhs = a - p;
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Vector3.Dot(vector, vector2);
			float sqrMagnitude2 = vector2.sqrMagnitude;
			float num2 = Vector3.Dot(lhs, vector);
			float num3 = Vector3.Dot(lhs, vector2);
			float num4 = Mathf.Abs(sqrMagnitude * sqrMagnitude2 - num * num);
			float num5 = num * num3 - sqrMagnitude2 * num2;
			float num6 = num * num2 - sqrMagnitude * num3;
			if (num5 + num6 <= num4)
			{
				if (num5 < 0f)
				{
					if (num6 < 0f)
					{
						if (num2 < 0f)
						{
							num6 = 0f;
							if (-num2 >= sqrMagnitude)
							{
								num5 = 1f;
							}
							else
							{
								num5 = -num2 / sqrMagnitude;
							}
						}
						else
						{
							num5 = 0f;
							if (num3 >= 0f)
							{
								num6 = 0f;
							}
							else if (-num3 >= sqrMagnitude2)
							{
								num6 = 1f;
							}
							else
							{
								num6 = -num3 / sqrMagnitude2;
							}
						}
					}
					else
					{
						num5 = 0f;
						if (num3 >= 0f)
						{
							num6 = 0f;
						}
						else if (-num3 >= sqrMagnitude2)
						{
							num6 = 1f;
						}
						else
						{
							num6 = -num3 / sqrMagnitude2;
						}
					}
				}
				else if (num6 < 0f)
				{
					num6 = 0f;
					if (num2 >= 0f)
					{
						num5 = 0f;
					}
					else if (-num2 >= sqrMagnitude)
					{
						num5 = 1f;
					}
					else
					{
						num5 = -num2 / sqrMagnitude;
					}
				}
				else
				{
					float num7 = 1f / num4;
					num5 *= num7;
					num6 *= num7;
				}
			}
			else if (num5 < 0f)
			{
				float num8 = num + num2;
				float num9 = sqrMagnitude2 + num3;
				if (num9 > num8)
				{
					float num10 = num9 - num8;
					float num11 = sqrMagnitude - 2f * num + sqrMagnitude2;
					if (num10 >= num11)
					{
						num5 = 1f;
						num6 = 0f;
					}
					else
					{
						num5 = num10 / num11;
						num6 = 1f - num5;
					}
				}
				else
				{
					num5 = 0f;
					if (num9 <= 0f)
					{
						num6 = 1f;
					}
					else if (num3 >= 0f)
					{
						num6 = 0f;
					}
					else
					{
						num6 = -num3 / sqrMagnitude2;
					}
				}
			}
			else if (num6 < 0f)
			{
				float num8 = num + num3;
				float num9 = sqrMagnitude + num2;
				if (num9 > num8)
				{
					float num10 = num9 - num8;
					float num11 = sqrMagnitude - 2f * num + sqrMagnitude2;
					if (num10 >= num11)
					{
						num6 = 1f;
						num5 = 0f;
					}
					else
					{
						num6 = num10 / num11;
						num5 = 1f - num6;
					}
				}
				else
				{
					num6 = 0f;
					if (num9 <= 0f)
					{
						num5 = 1f;
					}
					else if (num2 >= 0f)
					{
						num5 = 0f;
					}
					else
					{
						num5 = -num2 / sqrMagnitude;
					}
				}
			}
			else
			{
				float num10 = sqrMagnitude2 + num3 - num - num2;
				if (num10 <= 0f)
				{
					num5 = 0f;
					num6 = 1f;
				}
				else
				{
					float num11 = sqrMagnitude - 2f * num + sqrMagnitude2;
					if (num10 >= num11)
					{
						num5 = 1f;
						num6 = 0f;
					}
					else
					{
						num5 = num10 / num11;
						num6 = 1f - num5;
					}
				}
			}
			bc = new Vector2(num5, num6);
			return a + num5 * vector + num6 * vector2;
		}
		
		private static void Sort(ref float a, ref float b)
		{
			if (a > b)
			{
				float num = a;
				a = b;
				b = num;
			}
		}
		
		private static bool EdgeEdgeTest(Vector3 V0, Vector3 U0, Vector3 U1, int i0, int i1, ref float Ax, ref float Ay, ref float Bx, ref float By, ref float Cx, ref float Cy, ref float f, ref float d, ref float e)
		{
			Bx = U0[i0] - U1[i0];
			By = U0[i1] - U1[i1];
			Cx = V0[i0] - U0[i0];
			Cy = V0[i1] - U0[i1];
			f = Ay * Bx - Ax * By;
			d = By * Cx - Bx * Cy;
			if ((f > 0f && d >= 0f && d <= f) || (f < 0f && d <= 0f && d >= f))
			{
				e = Ax * Cy - Ay * Cx;
				if (f > 0f)
				{
					if (e >= 0f && e <= f)
					{
						return true;
					}
				}
				else if (e <= 0f && e >= f)
				{
					return true;
				}
			}
			return false;
		}
		
		private static bool EdgeAgainstTriangleEdges(Vector3 V0, Vector3 V1, Vector3 U0, Vector3 U1, Vector3 U2, int i0, int i1)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			num = V1[i0] - V0[i0];
			num2 = V1[i1] - V0[i1];
			return TriangleUtils.EdgeEdgeTest(V0, U0, U1, i0, i1, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num9, ref num8, ref num7) || TriangleUtils.EdgeEdgeTest(V0, U1, U2, i0, i1, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num9, ref num8, ref num7) || TriangleUtils.EdgeEdgeTest(V0, U2, U0, i0, i1, ref num, ref num2, ref num3, ref num4, ref num5, ref num6, ref num9, ref num8, ref num7);
		}
		
		private static bool PointInTriangle(Vector3 V0, Vector3 U0, Vector3 U1, Vector3 U2, int i0, int i1)
		{
			float num = U1[i1] - U0[i1];
			float num2 = -(U1[i0] - U0[i0]);
			float num3 = -num * U0[i0] - num2 * U0[i1];
			float num4 = num * V0[i0] + num2 * V0[i1] + num3;
			num = U2[i1] - U1[i1];
			num2 = -(U2[i0] - U1[i0]);
			num3 = -num * U1[i0] - num2 * U1[i1];
			float num5 = num * V0[i0] + num2 * V0[i1] + num3;
			num = U0[i1] - U2[i1];
			num2 = -(U0[i0] - U2[i0]);
			num3 = -num * U2[i0] - num2 * U2[i1];
			float num6 = num * V0[i0] + num2 * V0[i1] + num3;
			return num4 * num5 > 0f && num4 * num6 > 0f;
		}
		
		private static bool CoplanarTriangleTriangle(Vector3 N, Vector3 V0, Vector3 V1, Vector3 V2, Vector3 U0, Vector3 U1, Vector3 U2)
		{
			Vector3 zero = Vector3.zero;
			zero[0] = Mathf.Abs(N[0]);
			zero[1] = Mathf.Abs(N[1]);
			zero[2] = Mathf.Abs(N[2]);
			short i;
			short i2;
			if (zero[0] > zero[1])
			{
				if (zero[0] > zero[2])
				{
					i = 1;
					i2 = 2;
				}
				else
				{
					i = 0;
					i2 = 1;
				}
			}
			else if (zero[2] > zero[1])
			{
				i = 0;
				i2 = 1;
			}
			else
			{
				i = 0;
				i2 = 2;
			}
			return TriangleUtils.EdgeAgainstTriangleEdges(V0, V1, U0, U1, U2, (int)i, (int)i2) || TriangleUtils.EdgeAgainstTriangleEdges(V1, V2, U0, U1, U2, (int)i, (int)i2) || TriangleUtils.EdgeAgainstTriangleEdges(V2, V0, U0, U1, U2, (int)i, (int)i2) || TriangleUtils.PointInTriangle(V0, U0, U1, U2, (int)i, (int)i2) || TriangleUtils.PointInTriangle(U0, V0, V1, V2, (int)i, (int)i2);
		}
		
		private static bool ComputeTriangleIntervals(Vector3 N1, Vector3 V0, Vector3 V1, Vector3 V2, Vector3 U0, Vector3 U1, Vector3 U2, float VV0, float VV1, float VV2, float D0, float D1, float D2, float D0D1, float D0D2, ref float A, ref float B, ref float C, ref float X0, ref float X1)
		{
			if (D0D1 > 0f)
			{
				A = VV2;
				B = (VV0 - VV2) * D2;
				C = (VV1 - VV2) * D2;
				X0 = D2 - D0;
				X1 = D2 - D1;
			}
			else if (D0D2 > 0f)
			{
				A = VV1;
				B = (VV0 - VV1) * D1;
				C = (VV2 - VV1) * D1;
				X0 = D1 - D0;
				X1 = D1 - D2;
			}
			else if (D1 * D2 > 0f || D0 != 0f)
			{
				A = VV0;
				B = (VV1 - VV0) * D0;
				C = (VV2 - VV0) * D0;
				X0 = D0 - D1;
				X1 = D0 - D2;
			}
			else if (D1 != 0f)
			{
				A = VV1;
				B = (VV0 - VV1) * D1;
				C = (VV2 - VV1) * D1;
				X0 = D1 - D0;
				X1 = D1 - D2;
			}
			else
			{
				if (D2 == 0f)
				{
					return true;
				}
				A = VV2;
				B = (VV0 - VV2) * D2;
				C = (VV1 - VV2) * D2;
				X0 = D2 - D0;
				X1 = D2 - D1;
			}
			return false;
		}
		
		public static bool IntersectTriangleTriangle(Vector3 V0, Vector3 V1, Vector3 V2, Vector3 U0, Vector3 U1, Vector3 U2)
		{
			Vector3 lhs = V1 - V0;
			Vector3 rhs = V2 - V0;
			Vector3 vector = Vector3.Cross(lhs, rhs);
			float num = -Vector3.Dot(vector, V0);
			float num2 = Vector3.Dot(vector, U0) + num;
			float num3 = Vector3.Dot(vector, U1) + num;
			float num4 = Vector3.Dot(vector, U2) + num;
			float num5 = num2 * num3;
			float num6 = num2 * num4;
			if (num5 > 0f && num6 > 0f)
			{
				return false;
			}
			lhs = U1 - U0;
			rhs = U2 - U0;
			Vector3 vector2 = Vector3.Cross(lhs, rhs);
			float num7 = -Vector3.Dot(vector2, U0);
			float num8 = Vector3.Dot(vector2, V0) + num7;
			float num9 = Vector3.Dot(vector2, V1) + num7;
			float num10 = Vector3.Dot(vector2, V2) + num7;
			float num11 = num8 * num9;
			float num12 = num8 * num10;
			if (num11 > 0f && num12 > 0f)
			{
				return false;
			}
			Vector3 vector3 = Vector3.Cross(vector, vector2);
			float num13 = Mathf.Abs(vector3[0]);
			short index = 0;
			float num14 = Mathf.Abs(vector3[1]);
			float num15 = Mathf.Abs(vector3[2]);
			if (num14 > num13)
			{
				num13 = num14;
				index = 1;
			}
			if (num15 > num13)
			{
				index = 2;
			}
			float vV = V0[(int)index];
			float vV2 = V1[(int)index];
			float vV3 = V2[(int)index];
			float vV4 = U0[(int)index];
			float vV5 = U1[(int)index];
			float vV6 = U2[(int)index];
			float num16 = 0f;
			float num17 = 0f;
			float num18 = 0f;
			float num19 = 0f;
			float num20 = 0f;
			if (TriangleUtils.ComputeTriangleIntervals(vector, V0, V1, V2, U0, U1, U2, vV, vV2, vV3, num8, num9, num10, num11, num12, ref num16, ref num17, ref num18, ref num19, ref num20))
			{
				return TriangleUtils.CoplanarTriangleTriangle(vector, V0, V1, V2, U0, U1, U2);
			}
			float num21 = 0f;
			float num22 = 0f;
			float num23 = 0f;
			float num24 = 0f;
			float num25 = 0f;
			if (TriangleUtils.ComputeTriangleIntervals(vector, V0, V1, V2, U0, U1, U2, vV4, vV5, vV6, num2, num3, num4, num5, num6, ref num21, ref num22, ref num23, ref num24, ref num25))
			{
				return TriangleUtils.CoplanarTriangleTriangle(vector, V0, V1, V2, U0, U1, U2);
			}
			float num26 = num19 * num20;
			float num27 = num24 * num25;
			float num28 = num26 * num27;
			float num29 = num16 * num28;
			float num30 = num29 + num17 * num20 * num27;
			float num31 = num29 + num18 * num19 * num27;
			num29 = num21 * num28;
			float num32 = num29 + num22 * num26 * num25;
			float num33 = num29 + num23 * num26 * num24;
			TriangleUtils.Sort(ref num30, ref num31);
			TriangleUtils.Sort(ref num32, ref num33);
			return num31 >= num32 && num33 >= num30;
		}
		
		public static bool IntersectTriangleTriangleEdge2D(Vector2[] triangleAPoints, Vector2[] triangleBPoints, float epsilon)
		{
			Vector2 vector = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				Vector2 line1A = triangleAPoints[i];
				Vector2 line1B = triangleAPoints[(i + 1) % 3];
				for (int j = 0; j < 3; j++)
				{
					Vector2 line2A = triangleBPoints[j];
					Vector2 line2B = triangleBPoints[(j + 1) % 3];
					if (GeometryUtils.IntersectLineLine2D(line1A, line1B, line2A, line2B, out vector, epsilon))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

