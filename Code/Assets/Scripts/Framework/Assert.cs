using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace FootStudio.Util
{
	public static class Assert {

		public static void NotNull(object argument, string name)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot be null", new object[]{
					name
				}));
			}
		}
		
		public static void NotNull(UnityEngine.Object unityObject, string name)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Unity object '{0}' cannot be destroyed", new object[]{
					name
				}));
			}
		}
		
		public static void NotNull(object argument, string name, string message, params object[] args)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void NotNull(UnityEngine.Object unityObject, string name, string message, params object[] args)
		{
			if (unityObject == null)
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void HasText(string argument, string name)
		{
			if (StringUtils.IsNullOrEmpty(argument))
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot be null or resolve to an empty string : '{1}'", new object[]{
					name,
					argument
				}));
			}
		}
		
		public static void HasText(string argument, string name, string message, params object[] args)
		{
			if (StringUtils.IsNullOrEmpty(argument))
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void HasLength<T>(ICollection<T> argument, string name)
		{
			if (CollectionUtils.IsEmpty<T>(argument))
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' cannot be null or resolve to an empty array", new object[]{
					name
				}));
			}
		}
		
		public static void HasLength<T>(ICollection<T> argument, string name, string message, params object[] args)
		{
			if (CollectionUtils.IsEmpty<T>(argument))
			{
				throw new ArgumentNullException(name, string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void HasElements<T>(ICollection<T> argument, string name)
		{
			if (!CollectionUtils.HasElements<T>(argument))
			{
				throw new ArgumentException(name, string.Format(CultureInfo.InvariantCulture, "Argument '{0}' must not be null or resolve to an empty collection and must contain non-null elements", new object[] {
					name
				}));
			}
		}
		
		public static void Type(object argument, string argumentName, Type requiredType, string message, params object[] args)
		{
			if (argument != null && requiredType != null && !requiredType.IsAssignableFrom(argument.GetType()))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, message, args), argumentName);
			}
		}
		
		public static void IsTrue(bool expression)
		{
			Assert.IsTrue(expression, "[Assertion failed] - this expression must be true", new object[0]);
		}
		
		public static void IsTrue(bool expression, string message, params object[] args)
		{
			if (!expression)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void State(bool expression, string message, params object[] args)
		{
			if (!expression)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, message, args));
			}
		}
		
		public static void Fail(Action action, Type exceptionType)
		{
			Assert.Fail(action, exceptionType, "Action must throw " + exceptionType, new object[0]);
		}
		
		public static void Fail(Action action, Type exceptionType, string message, params object[] args)
		{
			Assert.NotNull(action, "action");
			Assert.NotNull(exceptionType, "exceptionType");
			try
			{
				action();
			}
			catch (Exception ex)
			{
				if (ex.GetType() != exceptionType)
				{
					throw new ArgumentException(string.Concat(new object[] {
						"Expected ",
						exceptionType,
						" but got ",
						ex.GetType()
					}));
				}
				return;
			}
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, message, args));
		}

	}
}
