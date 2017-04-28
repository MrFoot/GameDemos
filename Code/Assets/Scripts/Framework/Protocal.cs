using UnityEngine;
using System.Collections;
using System.IO;
using System;

//function：network protocol(proto-buffer) basic function
public static class Protocol
{
	static CmdSerializer m_builder = new CmdSerializer();

	public static byte[] StreamToBytes(Stream stream)
	{
		byte[] bytes = new byte[stream.Length];
		stream.Seek(0, SeekOrigin.Begin); 
		stream.Read(bytes, 0, (int)stream.Length);
		stream.Seek(0, SeekOrigin.Begin); 
		return bytes;
	}
	
	public static Stream BytesToStream(byte[] bytes)
	{
		return new MemoryStream(bytes);
	}

	/// Proto-buffer serialize.
    public static byte[] ProtoBufSerialize<T>(T protobuf)
	{
		MemoryStream stream = new MemoryStream();
		
		//ProtoBuf.Serializer.Serialize<T>(stream, protobuf);

		//ProtoBuf.Meta.TypeModel builder = new ProtoBuf.Meta.TypeModel();
		//builder.Serialize(stream, protobuf);

		//ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(stream, protobuf);

		//CmdSerializer builder = new CmdSerializer();
		//builder.Serialize(stream, protobuf);

		m_builder.Serialize(stream, protobuf);
		
		byte[] bytes = new byte[stream.Length];
		stream.Seek(0, SeekOrigin.Begin);
		stream.Read(bytes, 0, (int)stream.Length);
		return bytes;
	}

	// Proto-buffer de-serialize.
	public static T ProtoBufDeserialize<T>(byte[] bytes) where T:new()
	{
		MemoryStream stream = new MemoryStream(bytes);
		
        T instance = new T();
        Type type = instance.GetType();
		m_builder.Deserialize(stream, instance, type);
        return instance;
	}

}
