using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class JsonSerialization<T> {

	[SerializeField]//强迫unity去序列化一个私有字段  
	List<T> Data;  
	/// <summary>  
	///返回创建的list  
	/// </summary>  
	/// <returns></returns>  
	public List<T> ToList()  
	{  
		return Data; 
	}  
	public JsonSerialization(List<T> target) 
	{  
		this.Data = target;
	}  
}
