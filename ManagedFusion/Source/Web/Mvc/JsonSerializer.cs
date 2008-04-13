using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	internal class JsonSerializer : ManagedFusion.Serialization.JsonSerializer
	{
		private object _serializableObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer"/> class.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public JsonSerializer(object obj)
		{
			_serializableObject = obj;
		}

		/// <summary>
		/// Gets a value indicating whether to check for object name.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [check for object name]; otherwise, <see langword="false"/>.
		/// </value>
		public override bool CheckForObjectName
		{
			get { return true; }
		}

		/// <summary>
		/// Serializes to json.
		/// </summary>
		/// <param name="serialization">The serialization.</param>
		/// <returns></returns>
		public override string Serialize(Dictionary<string, object> serialization)
		{
			return base.Serialize(ServiceViewEngine.BuildResponse(_serializableObject, serialization));
		}
	}
}
