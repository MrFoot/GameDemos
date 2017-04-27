using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;


[Serializable]
public class ItemTable : TableBaseData
{
	public int Id;	//道具ID
	public string Name;	//名称
	public string Description;	//描述
}


public struct ItemTableStruct
{
		public static string SheetName = "道具表";
		public static string KeyWords = "道具ID";
		public static string DataTablePath = "/Table/道具表.xls";
}
