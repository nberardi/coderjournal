﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ManagedFusion.Serialization
{
	public class XmlSerializer : ISerializer
	{
		private XmlDocument doc;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSerializer"/> class.
		/// </summary>
		public XmlSerializer()
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
			get { return true; }
		}

		/// <summary>
		/// Serializes the specified serialization.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		public virtual string Serialize(Dictionary<string, object> serialization)
		{
			doc = new XmlDocument();

			BuildObject(doc, serialization);
			doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "utf-8", "yes"), doc.DocumentElement);

			StringBuilder builder = new StringBuilder();
			using (XmlWriter writer = new XmlTextWriter(new StringWriter(builder)))
			{
				doc.WriteTo(writer);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private void BuildObject(XmlNode node, IDictionary serialization)
		{
			foreach (DictionaryEntry entry in serialization)
			{
				if (!(entry.Key is string))
					throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

				string key = (entry.Key as string).TrimStart(new char[] { Serializer.AttributeMarker, Serializer.CollectionItemMarker });

				if ((entry.Key as string).StartsWith(Serializer.AttributeMarker.ToString()))
				{
					XmlAttribute attr = doc.CreateAttribute(key);
					BuildValue(attr, entry.Value);
					node.Attributes.Append(attr);
				}

				else
				{
					XmlElement subNode = doc.CreateElement(key);
					BuildValue(subNode, entry.Value);
					node.AppendChild(subNode);
				}
			}
		}

		/// <summary>
		/// Allows the array pass through.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		private bool AllowArrayPassThrough(object obj)
		{
			if (obj is IDictionary == false)
				return false;

			IDictionary dic = obj as IDictionary;

			if (dic.Count != 1)
				return false;

			foreach (object key in dic.Keys)
				if (key is String && (key as string).StartsWith(Serializer.CollectionItemMarker.ToString()))
					return true;

			return false;
		}

		/// <summary>
		/// Builds the array.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="array">The array.</param>
		private void BuildArray(XmlNode node, IList array)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (AllowArrayPassThrough(array[i]))
					BuildValue(node, array[i]);
				else
				{
					XmlElement subNode = doc.CreateElement("item");
					BuildValue(subNode, array[i]);
					node.AppendChild(subNode);
				}
			}
		}

		/// <summary>
		/// Converts to json value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private void BuildValue(XmlNode node, object value)
		{
			if (value == null)
			{
				node.AppendChild(doc.CreateTextNode(value as string));
			}
			else if (value is IList)
			{
				BuildArray(node, value as IList);
			}
			else if (value is IDictionary)
			{
				BuildObject(node, value as IDictionary);
			}
			else if (value is String)
			{
				node.AppendChild(doc.CreateTextNode(value as string));
			}
			else if (value is DateTime)
			{
				node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc)));
			}
			else if (value is Boolean)
			{
				node.AppendChild(doc.CreateTextNode(XmlConvert.ToString((bool)value)));
			}
			else if (value is Double || value is Single)
			{
				node.AppendChild(doc.CreateTextNode(String.Format("{0:r}", value)));
			}
			else
			{
				node.AppendChild(doc.CreateTextNode(Convert.ToString(value)));
			}
		}

		/// <summary>
		/// Serializes to XML.
		/// </summary>
		/// <param name="doc">The doc.</param>
		/// <param name="name">The name.</param>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		private XmlNode SerializeToXml(XmlDocument doc, string name, IDictionary serialization)
		{
			XmlNode node;

			if (String.IsNullOrEmpty(name))
				node = doc;
			else
				node = doc.CreateElement(name);

			foreach (DictionaryEntry entry in serialization)
			{
				if (!(entry.Key is string))
					throw new ArgumentException("Key of serialization dictionary must be a string.", "serialization");

				if (entry.Value is IDictionary)
					node.AppendChild(SerializeToXml(doc, entry.Key as string, entry.Value as IDictionary));

				else if (entry.Value is IList<object>)
				{
					XmlElement subNode = doc.CreateElement(entry.Key as string);

					foreach (object listItem in entry.Value as IList<object>)
					{
						if (listItem is IDictionary<string, object>)
							subNode.AppendChild(SerializeToXml(doc, "item", listItem as IDictionary));
						else
						{
							XmlElement subNodeItem = doc.CreateElement("item");
							XmlText subNodeValue = doc.CreateTextNode(Convert.ToString(listItem));
							subNodeItem.AppendChild(subNodeValue);
							subNode.AppendChild(subNodeItem);
						}
					}

					node.AppendChild(subNode);
				}

				else if ((entry.Key as string).StartsWith("_"))
				{
					XmlAttribute attr = doc.CreateAttribute((entry.Key as string).Substring(1));
					attr.Value = Convert.ToString(entry.Value);
					node.Attributes.Append(attr);
				}

				else
				{
					XmlElement subNode = doc.CreateElement((entry.Key as string));
					XmlText subNodeValue = doc.CreateTextNode(Convert.ToString(entry.Value));
					subNode.AppendChild(subNodeValue);
					node.AppendChild(subNode);
				}
			}

			return node;
		}
	}
}
