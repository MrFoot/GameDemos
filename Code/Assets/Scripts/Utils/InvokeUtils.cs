
using System;
using System.Collections.Generic;
using System.Reflection;


namespace FootStudio.Util
{
	public static class InvokeUtils
	{
		private static Dictionary<string, MethodInfo> Methods = new Dictionary<string, MethodInfo>();
		
		public static object Invoke(object objectThis, string name, object[] parameters)
		{
			Type type = objectThis.GetType();
			string text = string.Format("{0}.{1}", type.Name, name);
			MethodInfo method;
			if (!InvokeUtils.Methods.TryGetValue(text, out method))
			{
				try
				{
					method = type.GetMethod(name);
				}
				catch (Exception ex)
				{
					GameLog.Error("Invoke get method failed: {0}\nException: {1}", new object[]{
						text,
						ex
					});
				}
				InvokeUtils.Methods.Add(text, method);
			}
			if (method != null)
			{
				try
				{
					ParameterInfo[] array = null;
					for (int i = 0; i < parameters.Length; i++)
					{
						if (parameters[i] == null || parameters[i].Equals(null))
						{
							if (array == null)
							{
								array = method.GetParameters();
							}
							parameters[i] = Convert.ChangeType(null, array[i].GetType());
						}
					}
					return method.Invoke(objectThis, parameters);
				}
				catch (Exception ex2)
				{
					GameLog.Error("Invoke failed: {0}\nException: {1}", new object[]{
						text,
						ex2
					});
				}
			}
			return null;
		}
	}
}

