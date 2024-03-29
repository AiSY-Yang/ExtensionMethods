﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExtensionMethods.JsonSerializerConverts
{
	/// <summary>
	/// Type类型的转换器
	/// <a href="https://stackoverflow.com/questions/66919668/net-core-graphql-graphql-systemtextjson-serialization-and-deserialization-of/67001480">https://stackoverflow.com</a>
	/// </summary>
	class JsonConverterForType : JsonConverter<Type>
	{
		/// <inheritdoc/>
		public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// Caution: Deserialization of type instances like this 
			// is not recommended and should be avoided
			// since it can lead to potential security issues.

			// If you really want this supported (for instance if the JSON input is trusted):
			// string assemblyQualifiedName = reader.GetString();
			// return Type.GetType(assemblyQualifiedName);
			throw new NotSupportedException();
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
		{
			// Use this with caution, since you are disclosing type information.
			writer.WriteStringValue(value.AssemblyQualifiedName);
		}
	}
}
