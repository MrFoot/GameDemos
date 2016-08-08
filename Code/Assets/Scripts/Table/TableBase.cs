using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class TableBaseData
{
}


public class TableDataDict
{
	public Dictionary<int, TableBaseData> TableDatas = new Dictionary<int, TableBaseData>();

	public bool Add(int key, TableBaseData value)
	{

        if (!TableDatas.ContainsKey(key))
        {
            TableDatas.Add(key, value);
            return true;
        }
        else
        {
            Debug.LogError("Exception Have the Same Id :" + key);
            return false;
        }
	}

	public void Remove(int key)
	{
		TableDatas.Remove(key);
	}

	public TableBaseData GetValue(int key)
	{
		TableBaseData data = null;
		TableDatas.TryGetValue(key, out data);

		return data;
	}
}