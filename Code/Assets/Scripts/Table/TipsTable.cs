using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FootStudio.Util;


[Serializable]
public class TipsTable : TableBaseData
{
	public int Id;	//提示ID
	public int Type;	//类型
	public string Titlename;	//标题
	public string Contentone;	//正文文字1
	public string Contenttwo;	//正文文字2
	public int Tipstype;	//提示形态
	public string Buttoncontent;	//按钮文字
}


public struct TipsTableStruct
{
		public static string SheetName = "提示配置";
		public static string KeyWords = "提示ID";
		public static string DataTablePath = "/Table/提示配置.xls";
}
