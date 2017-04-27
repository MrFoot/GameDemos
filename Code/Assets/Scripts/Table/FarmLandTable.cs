using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;


[Serializable]
public class FarmLandTable : TableBaseData
{
	public int Id;	//地块编号
	public int Needlv;	//开放需求等级
	public int Costtype;	//直接开放消耗类型
	public int Num;	//消耗数量
}


public struct FarmLandTableStruct
{
		public static string SheetName = "地块配置";
		public static string KeyWords = "地块编号";
		public static string DataTablePath = "/Table/农场配置.xls";
}
