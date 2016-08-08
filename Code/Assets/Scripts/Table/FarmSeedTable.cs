using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;


[Serializable]
public class FarmSeedTable : TableBaseData
{
	public int Id;	//种子ID
	public int Seedtype;	//类型
	public string Name;	//名称
	public string Icon;	//图标
	public int Costtype;	//消耗类型
	public int Price;	//价格
	public int Needtime;	//成熟时间
	public int Gaintype;	//获得类型
	public int Gaingold;	//获得金币
	public string Midtype;	//中期形态图
	public string Finaltype;	//成熟形态图
}


public struct FarmSeedTableStruct
{
		public static string SheetName = "种子配置";
		public static string KeyWords = "种子ID";
		public static string DataTablePath = "/Table/农场配置.xls";
}
