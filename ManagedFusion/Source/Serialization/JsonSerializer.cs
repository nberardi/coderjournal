﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso href="http://www.ietf.org/rfc/rfc4627.txt"/>
	/// <seealso href="http://www.json.org"/>
	public class JsonSerializer : ISerializer
	{
		/// <summary>
		/// 
		/// </summary>
		public const char BeginObject = '{';

		/// <summary>
		/// 
		/// </summary>
		public const char EndObject = '}';

		/// <summary>
		/// 
		/// </summary>
		public const char BeginArray = '[';

		/// <summary>
		/// 
		/// </summary>
		public const char EndArray = ']';

		/// <summary>
		/// 
		/// </summary>
		public const char ValueSeperator = ',';

		/// <summary>
		/// 
		/// </summary>
		public const char NameSeperator = ':';

		/// <summary>
		/// 
		/// </summary>
		public const char BeginString = '\"';

		/// <summary>
		/// 
		/// </summary>
		public const char EndString = '\"';

		/// <summary>
		/// 
		/// </summary>
		public const string BeginDate = "/Date(";

		/// <summary>
		/// 
		/// </summary>
		public const string EndDate = ")/";

		/// <summary>
		/// 
		/// </summary>
		public const string TrueValue = "true";

		/// <summary>
		/// 
		/// </summary>
		public const string FalseValue = "false";

		/// <summary>
		/// 
		/// </summary>
		public const string NullValue = "null";

		/// <summary>
		/// 
		/// </summary>
		public static readonly long DatetimeMinTimeTicks;

		/// <summary>
		/// Initializes the <see cref="JsonSerializer"/> class.
		/// </summary>
		static JsonSerializer()
		{
			DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			DatetimeMinTimeTicks = time.Ticks;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer"/> class.
		/// </summary>
		public JsonSerializer()
		{
		}

		/// <summary>
		/// Gets a value indicating whether to check for object name.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
		/// </value>
		public virtual bool CheckForObjectName
		{
			get { return false; }
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		public virtual string Serialize(Dictionary<string, object> serialization)
		{
			StringBuilder builder = new StringBuilder();
			BuildObject(builder, serialization);
			return builder.ToString();
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private void BuildObject(StringBuilder builder, IDictionary serialization)
		{
			builder.Append(BeginObject);

			foreach (DictionaryEntry entry in serialization)
			{
				if (!(entry.Key is string))
					throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

				builder.Append(BeginString);
				builder.Append((entry.Key as string).TrimStart(new char[] { Serializer.AttributeMarker, Serializer.CollectionItemMarker }));
				builder.Append(EndString);
				builder.Append(NameSeperator);
				BuildValue(builder, entry.Value);
				builder.Append(ValueSeperator);
			}

			if (builder[builder.Length - 1] == ValueSeperator)
				builder.Remove(builder.Length - 1, 1);

			builder.Append(EndObject);
		}

		/// <summary>
		/// Builds the array.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="array">The array.</param>
		private void BuildArray(StringBuilder builder, IList array)
		{
			builder.Append(BeginArray);

			for (int i = 0; i < array.Count; i++)
			{
				BuildValue(builder, array[i]);

				if (i != array.Count - 1)
					builder.Append(ValueSeperator);
			}

			builder.Append(EndArray);
		}

		/// <summary>
		/// Converts to json value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private void BuildValue(StringBuilder builder, object value)
		{
			if (value == null)
			{
				builder.Append(NullValue);
			}
			else if (value is IList)
			{
				BuildArray(builder, value as IList);
			}
			else if (value is IDictionary)
			{
				BuildObject(builder, value as IDictionary);
			}
			else if (value is String)
			{
				BuildString(builder, value as string);
			}
			else if (value is Enum)
			{
				BuildString(builder, Convert.ToString(value));
			}
			else if (value is DateTime)
			{
				BuildString(builder, BeginDate + ((((DateTime)value).ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000000L) + EndDate);
			}
			else if (value is Boolean)
			{
				builder.Append(((bool)value) ? TrueValue : FalseValue);
			}
			else if (value is Double || value is Single)
			{
				builder.AppendFormat("{0:r}", value);
			}
			else
			{
				BuildString(builder, Convert.ToString(value));
			}
		}

		/// <summary>
		/// Escapes the string.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="s">The s.</param>
		private void BuildString(StringBuilder builder, string s)
		{
			builder.Append(BeginString);

			for (int i = 0; i < s.Length; i++)
			{
				switch (s[i])
				{
					case  '"': builder.Append(@"\"""); break;
					case '\\': builder.Append(@"\\"); break;
					case  '/': builder.Append(@"\/"); break;
					case '\b': builder.Append(@"\b"); break;
					case '\f': builder.Append(@"\f"); break;
					case '\n': builder.Append(@"\n"); break;
					case '\r': builder.Append(@"\r"); break;
					case '\t': builder.Append(@"\t"); break;
					default:
						// chedk for a instance character and escape as unicode
						if (Char.IsControl(s, i))
							builder.Append(@"\u" + ((int)s[i]).ToString("X4"));
						else
							builder.Append(s[i]);
						break;
				}
			}

			builder.Append(EndString);
		}
	}
}
