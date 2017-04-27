using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;


public class TableManager
{
	private Dictionary<int, TableDataDict> TableData = new Dictionary<int, TableDataDict>();

	public void Init()
	{
		ItemTableDataFile ItemTableAssetDataFile = ResourceManager.Load<ItemTableDataFile>(TablePath.Paths[(int)TableID.ItemTableID]);

		if(ItemTableAssetDataFile.IDs != null)
		{
			TableDataDict dataDict = new TableDataDict();
			for(int i = 0, max = ItemTableAssetDataFile.IDs.Count; i < max; ++i)
			{
				if(!dataDict.Add(ItemTableAssetDataFile.IDs[i],ItemTableAssetDataFile.ItemTableList[i]))
				{
					 Debug.LogError("Exception :" +ItemTableAssetDataFile.name);
				}
			}
			TableData.Add((int)TableID.ItemTableID,dataDict);
		}



		FarmLandTableDataFile FarmLandTableAssetDataFile = ResourceManager.Load<FarmLandTableDataFile>(TablePath.Paths[(int)TableID.FarmLandTableID]);

		if(FarmLandTableAssetDataFile.IDs != null)
		{
			TableDataDict dataDict = new TableDataDict();
			for(int i = 0, max = FarmLandTableAssetDataFile.IDs.Count; i < max; ++i)
			{
				if(!dataDict.Add(FarmLandTableAssetDataFile.IDs[i],FarmLandTableAssetDataFile.FarmLandTableList[i]))
				{
					 Debug.LogError("Exception :" +FarmLandTableAssetDataFile.name);
				}
			}
			TableData.Add((int)TableID.FarmLandTableID,dataDict);
		}



		FarmSeedTableDataFile FarmSeedTableAssetDataFile = ResourceManager.Load<FarmSeedTableDataFile>(TablePath.Paths[(int)TableID.FarmSeedTableID]);

		if(FarmSeedTableAssetDataFile.IDs != null)
		{
			TableDataDict dataDict = new TableDataDict();
			for(int i = 0, max = FarmSeedTableAssetDataFile.IDs.Count; i < max; ++i)
			{
				if(!dataDict.Add(FarmSeedTableAssetDataFile.IDs[i],FarmSeedTableAssetDataFile.FarmSeedTableList[i]))
				{
					 Debug.LogError("Exception :" +FarmSeedTableAssetDataFile.name);
				}
			}
			TableData.Add((int)TableID.FarmSeedTableID,dataDict);
		}



		ResourceManager.UnloadUnusedResources();
	}


	public TableDataDict GetTable(int tableID)
	{
		return TableData[tableID];
	}
	public TableBaseData GetTableData(int tableID, int dataID)
	{
		TableDataDict dataDict = TableData[tableID];
		return dataDict.GetValue(dataID);
	}
}
