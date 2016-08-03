using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEditor;
using System.Data;


public class CreateTableAsset
{
	[MenuItem("AssetBundles/Create Table Asset策划打表用")]
	public static void Execute()
	{
		ItemTableDataFile ItemTableData = ScriptableObject.CreateInstance<ItemTableDataFile>();

		List<int> ItemTableID = new List<int>();
		List<ItemTable> ItemTableAttr = new List<ItemTable>();

		string ItemTableSheetName = "道具表";
		string ItemTableFilePath = Application.dataPath+"/Table/道具表.xls";

		DataTable ItemTableDataTable = CreateUnityClass.ExcelToDataTable(ItemTableFilePath,ItemTableSheetName, false);
		foreach(DataRow dr in ItemTableDataTable.Rows)
		{
			ItemTable mainItemTable = new ItemTable();
			if(!dr.IsNull("道具ID"))
			{
				mainItemTable.Id = int.Parse(dr["道具ID"].ToString());
			}
			if(!dr.IsNull("名称"))
			{
				mainItemTable.Name = dr["名称"].ToString();
			}
			if(!dr.IsNull("描述"))
			{
				mainItemTable.Description = dr["描述"].ToString();
			}

			if(!dr.IsNull("道具ID"))
			{
				ItemTableID.Add(int.Parse(dr["道具ID"].ToString()));
				ItemTableAttr.Add(mainItemTable);
			}
		}
		ItemTableData.IDs = ItemTableID;
		ItemTableData.ItemTableList = ItemTableAttr;
		AssetDatabase.CreateAsset(ItemTableData,"Assets/Resources/TableAsset/ItemTable.asset");




		FarmLandTableDataFile FarmLandTableData = ScriptableObject.CreateInstance<FarmLandTableDataFile>();

		List<int> FarmLandTableID = new List<int>();
		List<FarmLandTable> FarmLandTableAttr = new List<FarmLandTable>();

		string FarmLandTableSheetName = "地块配置";
		string FarmLandTableFilePath = Application.dataPath+"/Table/农场配置.xls";

		DataTable FarmLandTableDataTable = CreateUnityClass.ExcelToDataTable(FarmLandTableFilePath,FarmLandTableSheetName, false);
		foreach(DataRow dr in FarmLandTableDataTable.Rows)
		{
			FarmLandTable mainFarmLandTable = new FarmLandTable();
			if(!dr.IsNull("地块编号"))
			{
				mainFarmLandTable.Id = int.Parse(dr["地块编号"].ToString());
			}
			if(!dr.IsNull("开放需求等级"))
			{
				mainFarmLandTable.Needlv = int.Parse(dr["开放需求等级"].ToString());
			}
			if(!dr.IsNull("直接开放消耗类型"))
			{
				mainFarmLandTable.Costtype = int.Parse(dr["直接开放消耗类型"].ToString());
			}
			if(!dr.IsNull("消耗数量"))
			{
				mainFarmLandTable.Num = int.Parse(dr["消耗数量"].ToString());
			}

			if(!dr.IsNull("地块编号"))
			{
				FarmLandTableID.Add(int.Parse(dr["地块编号"].ToString()));
				FarmLandTableAttr.Add(mainFarmLandTable);
			}
		}
		FarmLandTableData.IDs = FarmLandTableID;
		FarmLandTableData.FarmLandTableList = FarmLandTableAttr;
		AssetDatabase.CreateAsset(FarmLandTableData,"Assets/Resources/TableAsset/FarmLandTable.asset");




		FarmSeedTableDataFile FarmSeedTableData = ScriptableObject.CreateInstance<FarmSeedTableDataFile>();

		List<int> FarmSeedTableID = new List<int>();
		List<FarmSeedTable> FarmSeedTableAttr = new List<FarmSeedTable>();

		string FarmSeedTableSheetName = "种子配置";
		string FarmSeedTableFilePath = Application.dataPath+"/Table/农场配置.xls";

		DataTable FarmSeedTableDataTable = CreateUnityClass.ExcelToDataTable(FarmSeedTableFilePath,FarmSeedTableSheetName, false);
		foreach(DataRow dr in FarmSeedTableDataTable.Rows)
		{
			FarmSeedTable mainFarmSeedTable = new FarmSeedTable();
			if(!dr.IsNull("种子ID"))
			{
				mainFarmSeedTable.Id = int.Parse(dr["种子ID"].ToString());
			}
			if(!dr.IsNull("类型"))
			{
				mainFarmSeedTable.Seedtype = int.Parse(dr["类型"].ToString());
			}
			if(!dr.IsNull("名称"))
			{
				mainFarmSeedTable.Name = dr["名称"].ToString();
			}
			if(!dr.IsNull("图标"))
			{
				mainFarmSeedTable.Icon = dr["图标"].ToString();
			}
			if(!dr.IsNull("消耗类型"))
			{
				mainFarmSeedTable.Costtype = int.Parse(dr["消耗类型"].ToString());
			}
			if(!dr.IsNull("价格"))
			{
				mainFarmSeedTable.Price = int.Parse(dr["价格"].ToString());
			}
			if(!dr.IsNull("成熟时间"))
			{
				mainFarmSeedTable.Needtime = int.Parse(dr["成熟时间"].ToString());
			}
			if(!dr.IsNull("获得类型"))
			{
				mainFarmSeedTable.Gaintype = int.Parse(dr["获得类型"].ToString());
			}
			if(!dr.IsNull("获得金币"))
			{
				mainFarmSeedTable.Gaingold = int.Parse(dr["获得金币"].ToString());
			}
			if(!dr.IsNull("中期形态图"))
			{
				mainFarmSeedTable.Midtype = dr["中期形态图"].ToString();
			}
			if(!dr.IsNull("成熟形态图"))
			{
				mainFarmSeedTable.Finaltype = dr["成熟形态图"].ToString();
			}

			if(!dr.IsNull("种子ID"))
			{
				FarmSeedTableID.Add(int.Parse(dr["种子ID"].ToString()));
				FarmSeedTableAttr.Add(mainFarmSeedTable);
			}
		}
		FarmSeedTableData.IDs = FarmSeedTableID;
		FarmSeedTableData.FarmSeedTableList = FarmSeedTableAttr;
		AssetDatabase.CreateAsset(FarmSeedTableData,"Assets/Resources/TableAsset/FarmSeedTable.asset");





		EditorUtility.DisplayDialog("提示", "打表完成", "OK");
	}
}
