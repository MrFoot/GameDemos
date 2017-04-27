using System;
using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using UnityEditor;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;

#region Help class
public class FieldInfoTable
{
    private bool m_isComArray;
    private int m_comArrLen;
    private string m_dataType;
    private string m_varyName;
    private string m_defaultNum;
    private string m_fieldNote;
    private bool m_isStruct;
    private int m_strArrLen;

    public FieldInfoTable(){  }

    public FieldInfoTable(bool isComArray,int comArrLen,string dataType,string varyName,string defaultNum,string fieldNote,bool isStruct,int strArrLen)
    {
        m_isComArray = isComArray;
        m_comArrLen = comArrLen;
        m_dataType = dataType;
        m_varyName = varyName;
        m_defaultNum = defaultNum;
        m_fieldNote = fieldNote;
        m_isStruct = isStruct;
        m_strArrLen = strArrLen;
    }

    public bool IsComArray
    {
        get
        {
            return m_isComArray;
        }
        set
        {
            m_isComArray = value;
        }
    }

    public int ComArrLen
    {
        get
        {
            return m_comArrLen;
        }
        set
        {
            m_comArrLen = value;
        }
    }

    public string DataType
    {
        get
        {
            return m_dataType;
        }
        set 
        {
            m_dataType = value;
        }
    }

    public string VaryName
    {
        get
        {
            return m_varyName; 
        }
        set
        {
            m_varyName = value;
        }
    }

    public string DefaultNum
    {
        get
        {
            return m_defaultNum;
        }
        set
        {
            m_defaultNum = value;
        }
    }

    public string FieldNote
    {
        get
        {
            return m_fieldNote;
        }
        set
        {
            m_fieldNote = value;
        }
    }

    public bool IsStruct
    {
        get
        {
            return m_isStruct;
        }
        set
        {
            m_isStruct = value;
        }
    }

    public int StrArrLen
    {
        get
        {
            return m_strArrLen;
        }
        set
        {
            m_strArrLen = value;
        }
    }
}

public class NestClassInfo
{
    private string m_nestName;
    private Dictionary<int, FieldInfoTable> m_nestDict = new Dictionary<int,FieldInfoTable>();

    public NestClassInfo() { }

    public NestClassInfo(string nestName, Dictionary<int, FieldInfoTable> nestDict)
    {
        m_nestName = nestName;
        m_nestDict = nestDict;
    }

    public string NestName
    {
        get
        {
            return m_nestName;
        }
        set
        {
            m_nestName = value;
        }
    }

    public Dictionary<int, FieldInfoTable> NestDict
    {
        get
        {
            return m_nestDict;
        }
        set
        {
            m_nestDict = value;
        }
    }

}

public class AssistInfo
{
    private string m_keyWords;
    private string m_sheetName;
    private string m_tablePath;

    public AssistInfo() { }

    public AssistInfo(string keyWords, string sheetName, string tablePath)
    {
        m_keyWords = keyWords;
        m_sheetName = sheetName;
        m_tablePath = tablePath;
    }

    public string KeyWords
    {
        get
        {
            return m_keyWords;
        }
        set
        {
            m_keyWords = value;
        }
    }

    public string SheetName
    {
        get
        {
            return m_sheetName;
        }
        set
        {
            m_sheetName = value;
        }
    }

    public string TablePath
    {
        get
        {
            return m_tablePath;
        }
        set
        {
            m_tablePath = value;
        }
    }
}
#endregion


class CreateUnityClass
{
    #region Table Topic
    public static string ResConfResourceName = "资源名称";
    public static string ResConfHeaderName = "头文件";
    public static string ResConfStructName = "结构名称";
    public static string ResConfStructExplain = "结构说明";
    public static string ResConfSortField = "排序字段";
    public static string ResConfResourceFile = "资源文件";


    public static string SubTableTopic = "标题";
    public static string SubTableSubTopic = "二级标题";
    public static string SubTableFieldName = "字段名称";
    public static string SubTableType = "类型";
    public static string SubTableMaxLength = "最大长度";
    public static string SubTableDefaultNum = "默认值";
    public static string SubTableKeyWord = "关键字";

    public static string AssetPath = Application.dataPath;
    public static string ACCESS_LIMIT = "public";
    public static string[] ClassNameSpace = {  "using System;",
                                                "using UnityEngine;",
                                                "using System.IO;",
                                                "using System.Collections;",
                                                "using System.Collections.Generic;",
												"using FootStudio.Util;",
                                             };
    #endregion

    public static Dictionary<string, AssistInfo> AllStructInfo = new Dictionary<string, AssistInfo>();
    public static Dictionary<string, List<NestClassInfo>> AllNestInfo = new Dictionary<string, List<NestClassInfo>>();
    public static Dictionary<string, Dictionary<int, FieldInfoTable>> AllTableInfo = new Dictionary<string, Dictionary<int, FieldInfoTable>>();

    public static string ResConfPath = Application.dataPath;
    public static List<DataRow> ResConfDataRow = new List<DataRow>();

    [MenuItem("AssetBundles/CreateUnityClass(With Excel)")]
    public static void Execute()
    {
        List<DataTable> resConfList = GetResConfInfo();

        List<DataTable> subTableInfo = new List<DataTable>();
        if (resConfList != null && resConfList.Count > 0)
        {
            subTableInfo = GetSubTableInfo(resConfList);
        }
        
        CreateLuaOrEnum();
        GetClassOrStrutOrNestInfo(subTableInfo);
        CreateTableClass();
        CreateDataTableFile();
        CreateTableAsset();
        CreateTableManager();
        Debug.Log("Create is Succed !!!");
    }
    
    #region Read/Save table information 
    /// <summary>
    /// Get "res_contf" table information
    /// </summary>
    /// <returns>List<DataTable> => "res_conf" Information </returns>
    public static List<DataTable> GetResConfInfo()
    {
        string pathPrefix = @"Assets/Table/";

        UnityEngine.Object[] selects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);

        if (selects.Length < 0)
        {
            Debug.LogError("Please select somthing");
            return null;
        }


        List<DataTable> resConf = new List<DataTable>();
        for (int i = 0; i < selects.Length; i++)
        {
            UnityEngine.Object o = (UnityEngine.Object)selects[i];
            string oriPath = AssetDatabase.GetAssetPath(o);

            if (oriPath.StartsWith(pathPrefix))
            {
                ResConfPath = Application.dataPath;
                ResConfPath = ResConfPath + oriPath.TrimStart("Assets".ToCharArray());

                DataTable tempTable = ExcelToDataTable(ResConfPath, null, true);
                if (tempTable != null)
                {
                    resConf.Add(tempTable);
                }
            }
            else
            {
                Debug.LogError("Table storage path error!!");
                return null;
            }
        }
        
        return resConf;
    }

    /// <summary>
    /// Get "res_conf" table sheet information and save list
    /// </summary>
    /// <param name="resConfList"></param>
    /// <returns>list save sub information </returns>
    public static List<DataTable> GetSubTableInfo(List<DataTable> resConfList)
    {
		ResConfDataRow.Clear();
        List<DataTable> subTableInfo = new List<DataTable>();

        foreach (DataTable resConf in resConfList)
        {
            bool first = true;
            foreach (DataRow resDr in resConf.Rows)
            {
                if (first)
                {
                    int resNumber = 0;
                    for (int i = 0; i < resConf.Columns.Count; i++)
                    {
                        if (ResConfResourceName.Equals(resDr[i].ToString()) || ResConfHeaderName.Equals( resDr[i].ToString()) || ResConfStructName.Equals( resDr[i].ToString()) ||
                            ResConfStructExplain.Equals(resDr[i].ToString()) || ResConfSortField.Equals(resDr[i].ToString()) ||  ResConfResourceFile.Equals(resDr[i].ToString()))
                        {
                           
                            resNumber++;
                        }
                    }
                  
                    if (resNumber == resConf.Columns.Count)
                    {
                        first = false;
                        continue;
                    }
                    else
                    {
                        Debug.LogError("Main excel table no have filed name!!!");
                        return null;
                    }
                }

                ResConfDataRow.Add(resDr);
                DataTable subTempTable = ExcelToDataTable(ResConfPath, resDr[ResConfStructName].ToString(), true);

                bool flag = true;
                foreach (DataRow subDr in subTempTable.Rows)
                {
                    if (flag)
                    {
                        int subNumber = 0;
                        for (int j = 0; j < subTempTable.Columns.Count; j++)
                        {
                            if (subDr[SubTableTopic].ToString() == SubTableTopic || subDr[SubTableSubTopic].ToString() == SubTableSubTopic || subDr[SubTableFieldName].ToString() == SubTableFieldName ||
                                subDr[SubTableType].ToString() == SubTableType || subDr[SubTableMaxLength].ToString() == SubTableMaxLength || subDr[SubTableDefaultNum].ToString() == SubTableDefaultNum || subDr[SubTableKeyWord].ToString() == SubTableKeyWord)
                            {
                                subNumber++;
                            }
                        }

                        if (subNumber == subTempTable.Columns.Count)
                        {
                            flag = false;
                            break;
                        }
                        else
                        {
                            Debug.LogError("Sub excel table no have filed name!!!");
                            return null;
                        }
                    }
                }
                subTableInfo.Add(subTempTable);
            }
        }
        return subTableInfo;
    }

    /// <summary>
    /// Delal with class name string and return real name
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static string DealWithHeader(string className)
    {
        className = className.Remove(className.IndexOf('.'));
        if (className.IndexOf('_') >= 0)
        {
            string[] str = className.Split('_');
            className = null;
            for (int i = 0; i < str.Length; i++)
            {
                char[] ch = str[i].ToCharArray();
                ch[0] = (char)(ch[0] - 32);
                className += new string(ch);
            }
        }
        else
        {
            char[] ch = className.ToCharArray();
            ch[0] = (char)(ch[0] - 32);
            className = new string(ch);
        }

        className += "Table";

        return className;
    }

    /// <summary>
    /// User "res_conf" table information and  the information will be stored in three dictionary inside 
    /// </summary>
    /// <param name="DataTableInfo"></param>
    public static void GetClassOrStrutOrNestInfo(List<DataTable> DataTableInfo)
    {
		AllStructInfo.Clear();
		AllTableInfo.Clear();
        for (int i = 0; i < DataTableInfo.Count; ++i)
        {
            string className = DealWithHeader(ResConfDataRow[i][ResConfHeaderName].ToString());
    
            int comIndex = 0;
           
            List<NestClassInfo> nestClassInfo = new List<NestClassInfo>();
            Dictionary<int, FieldInfoTable> commonFieldInfo = new Dictionary<int, FieldInfoTable>();
            Dictionary<int, AssistInfo> AssistInfo = new Dictionary<int, AssistInfo>();

            string InstancePath = ResConfDataRow[i][ResConfResourceFile].ToString();
            string sheet = InstancePath.Substring(InstancePath.LastIndexOf(":") + 1);
            string structPath = InstancePath.Remove(InstancePath.LastIndexOf(":"));

            AssistInfo assistInfo = new AssistInfo();

            for (int j = 1; j < DataTableInfo[i].Rows.Count; ++j)
            {
                if (!DataTableInfo[i].Rows[j].IsNull(SubTableType) && !DataTableInfo[i].Rows[j].IsNull(SubTableFieldName))
                {
                    if (DataTableInfo[i].Rows[j][SubTableKeyWord].ToString() == "是")
                    {
                        assistInfo.KeyWords = DataTableInfo[i].Rows[j][SubTableTopic].ToString();
                    }

                    if (DataTableInfo[i].Rows[j][SubTableType].ToString() == "struct") //处理有嵌套类表格的信息
                    {
                        int nestIndex = 0;
                        Dictionary<int, FieldInfoTable> nestFieldInfo = new Dictionary<int, FieldInfoTable>();

                        int k = 0;
                        for (k = j + 1; k < DataTableInfo[i].Rows.Count+1; ++k)
                        {
                            FieldInfoTable fieldInfo = new FieldInfoTable();

                            /*当嵌套类里面的字段完成时，把所有的嵌套信息存起来*/
                            if (k >= DataTableInfo[i].Rows.Count || !DataTableInfo[i].Rows[k].IsNull(SubTableTopic))
                            {
                                NestClassInfo tempNest = new NestClassInfo();
                                tempNest.NestName = DataTableInfo[i].Rows[j][SubTableSubTopic].ToString();
                                tempNest.NestDict = nestFieldInfo;

                                nestClassInfo.Add(tempNest);

                                fieldInfo.IsComArray = false;
                                fieldInfo.ComArrLen = 0;
                                fieldInfo.DataType = DataTableInfo[i].Rows[j][SubTableSubTopic].ToString();
                                fieldInfo.VaryName = DataTableInfo[i].Rows[j][SubTableFieldName].ToString();
                                fieldInfo.DefaultNum = "0";
                                fieldInfo.FieldNote = DataTableInfo[i].Rows[j][SubTableTopic].ToString();
                                fieldInfo.IsStruct = true;
                                
                                int strLen = 0;
                                if(!DataTableInfo[i].Rows[j].IsNull(SubTableMaxLength))
                                {
                                    strLen = Convert.ToInt32(DataTableInfo[i].Rows[j][SubTableMaxLength].ToString());
                                }
                                
                                if (strLen > 1)
                                {
                                   
                                    fieldInfo.StrArrLen =  strLen;
                                }
                                else
                                {
                                    fieldInfo.StrArrLen = 0;
                                }
                                commonFieldInfo.Add(comIndex++, fieldInfo);
                            
                                break;
                                /*处理嵌套类信息结束*/
                            }
                            else
                            {
                              
                                /*处理嵌套内里面的字段信息，并存在nestFieldInfo这个字典里面*/

                                if (DataTableInfo[i].Rows[k][SubTableType].ToString() == "char")
                                {
                                    fieldInfo.IsComArray = false;
                                    fieldInfo.ComArrLen = 0;
                                    fieldInfo.DataType = "string";
                                    fieldInfo.VaryName = DataTableInfo[i].Rows[k][SubTableFieldName].ToString();
                                    fieldInfo.DefaultNum = "0";
                                    fieldInfo.FieldNote = DataTableInfo[i].Rows[k][SubTableSubTopic].ToString();  
                                    fieldInfo.IsStruct = false;
                                    fieldInfo.StrArrLen = 0;
                                    nestFieldInfo.Add(nestIndex++, fieldInfo);
                                    continue;
                                }

                                
                                fieldInfo.IsComArray = false;
                                fieldInfo.ComArrLen = 0;
 
                                fieldInfo.DataType = DataTableInfo[i].Rows[k][SubTableType].ToString();
                                fieldInfo.VaryName = DataTableInfo[i].Rows[k][SubTableFieldName].ToString();
                                fieldInfo.DefaultNum = "0";
                                fieldInfo.FieldNote = DataTableInfo[i].Rows[k][SubTableSubTopic].ToString();
                                fieldInfo.IsStruct = false;
                                fieldInfo.StrArrLen = 0;

                                int maxLen = 0;
                                if (!DataTableInfo[i].Rows[k].IsNull(SubTableMaxLength))
                                {
                                    maxLen = Convert.ToInt32(DataTableInfo[i].Rows[k][SubTableMaxLength].ToString());
                                }

                                if (maxLen > 1)
                                {
                                    fieldInfo.IsComArray = true;
                                    fieldInfo.ComArrLen = 0;
                                }

                                nestFieldInfo.Add(nestIndex++, fieldInfo);
                                /*处理嵌套内里面的字段信息结束*/
                            }

                        }
                        j = k - 1;
                    }
                    else //处理没有嵌套类表格的信息
                    {

                        FieldInfoTable fieldInfo = new FieldInfoTable();
                        if(DataTableInfo[i].Rows[j][SubTableType].ToString() == "char")
                        {
                            fieldInfo.DataType = "string";
                            fieldInfo.VaryName = DataTableInfo[i].Rows[j][SubTableFieldName].ToString();
                            fieldInfo.DefaultNum = "0";
                            fieldInfo.FieldNote = DataTableInfo[i].Rows[j][SubTableTopic].ToString();
                            fieldInfo.IsStruct = false;
                            fieldInfo.StrArrLen = 0;
                            fieldInfo.IsComArray = false;
                            fieldInfo.ComArrLen = 0;
                            commonFieldInfo.Add(comIndex++, fieldInfo);
                            continue;
                        }
                        
                        fieldInfo.DataType = DataTableInfo[i].Rows[j][SubTableType].ToString();
                        fieldInfo.VaryName = DataTableInfo[i].Rows[j][SubTableFieldName].ToString();
                        fieldInfo.DefaultNum = "0";
                        fieldInfo.FieldNote = DataTableInfo[i].Rows[j][SubTableTopic].ToString();
                        fieldInfo.IsStruct = false;
                        fieldInfo.StrArrLen = 0;

                        string topicStr = DataTableInfo[i].Rows[j][SubTableTopic].ToString();

                        int maxLen = 0;
                        if (!DataTableInfo[i].Rows[j].IsNull(SubTableMaxLength))
                        {
                             maxLen = Convert.ToInt32(DataTableInfo[i].Rows[j][SubTableMaxLength].ToString());
                        }

                        /*处理是不是数组的信息，根据字段的结尾是不是以“*”介绍判断是不是数组*/
                        if (topicStr[topicStr.Length - 1] == '*')
                        {
                            int len = Convert.ToInt32(DataTableInfo[i].Rows[j][SubTableDefaultNum]);
                            fieldInfo.IsComArray = true;
                            fieldInfo.ComArrLen = len;

                            if (string.IsNullOrEmpty(DataTableInfo[i].Rows[j][SubTableDefaultNum].ToString()) || len <= 1)
                            {
                                //Debug.LogError("Mark * is not Array ,Please si CheckOut");
                                Debug.LogErrorFormat("Mark * is not Array ,Please si CheckOut.表:{0},path:{1}", className, structPath);
                            }

                            j += len;
                        }
                        else if(maxLen > 1)
                        {
                            fieldInfo.IsComArray = true;
                            fieldInfo.ComArrLen = 0;
                        }
                        else
                        {
                            fieldInfo.IsComArray = false;
                            fieldInfo.ComArrLen = 0;
                        }
                        /*处理是不是数组的信息结束*/
                        commonFieldInfo.Add(comIndex++, fieldInfo);
                    }
                }
            }

           
            

            assistInfo.SheetName = sheet;
            assistInfo.TablePath ="/Table/" + structPath;
            if (AllStructInfo.ContainsKey(className))
                Debug.LogError("重复的表：" + className + ",路径：" + structPath);
            AllStructInfo.Add(className, assistInfo);
            AllTableInfo.Add(className, commonFieldInfo);
            if (nestClassInfo.Count > 0)
            {
                AllNestInfo.Add(className, nestClassInfo);
            }
        }
    }

    #endregion

    #region Create Class
    /// <summary>
    /// Create Lua script ans lua script
    /// </summary>
    public static void CreateLuaOrEnum()
    {
		string filepath = AssetPath + "/Scripts" + "/Table/" + "TableEnum" + ".cs";

//		if(File.Exists(filepath))
//		{
//			Debug.Log(filepath + "is Deleted!!!!!!!!!!!!!");
//			File.Delete(filepath);
//		}
		FileStream fsEnum = new FileStream(filepath,  FileMode.Create,FileAccess.Write);
		fsEnum.SetLength(0);

        StreamWriter swEnum = new StreamWriter(fsEnum);

		FileStream fsLua = new FileStream(AssetPath + "/Scripts" + "/Table/" + "TablePath" + ".cs", FileMode.Create,FileAccess.Write );
        StreamWriter swLua = new StreamWriter(fsLua);

        for (int i = 0; i < ClassNameSpace.Length; i++)
        {
            swEnum.WriteLine(ClassNameSpace[i]);
			swLua.WriteLine(ClassNameSpace[i]);
        }

        int AssetID = 30001;
        swEnum.WriteLine('\n');
        swEnum.WriteLine(ACCESS_LIMIT + " " + "enum" + " " + "TableID");
        swEnum.WriteLine("{");

		swLua.WriteLine('\n');
		swLua.WriteLine(ACCESS_LIMIT + " static class TablePath");
		swLua.WriteLine("{");

		swLua.Write('\t');
		swLua.WriteLine("public static Dictionary<int, string> Paths = new Dictionary<int, string> {");

		Debug.Log("RowCount is " + ResConfDataRow.Count);
        for (int i = 0; i < ResConfDataRow.Count; ++i)
        {
            string className = DealWithHeader(ResConfDataRow[i][ResConfHeaderName].ToString());

            swEnum.Write('\t');
            swEnum.WriteLine(className + "ID" + "=" + (AssetID + i) + ",");

			swLua.Write("\t\t");
			swLua.WriteLine("{"+(AssetID + i) + ", \"TableAsset/" + className + "\"},");
        }

		swLua.WriteLine("\t};");
		swLua.WriteLine("}");
        swLua.Flush();
        swLua.Close();

        swEnum.WriteLine("}");
        swEnum.Flush();
        swEnum.Close();
    }

    /// <summary>
    /// According three Dictionary create base class
    /// </summary>
    public static void CreateTableClass()
    {
        foreach (KeyValuePair<string, Dictionary<int, FieldInfoTable>> kvp in AllTableInfo)
        {
            string filePath = Application.dataPath + "/Scripts" + "/Table/" + kvp.Key;
            FileStream fs = new FileStream(filePath + ".cs", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            byte[] arryByte = { 0xef, 0xbb, 0xbf };
            sw.Write(System.Text.Encoding.UTF8.GetString(arryByte));

            for (int f = 0; f < ClassNameSpace.Length; f++)
            {
                sw.WriteLine(ClassNameSpace[f]);
            }

            sw.WriteLine('\n');
            sw.WriteLine("[Serializable]");
            sw.WriteLine("public" + " " + "class" + " " + kvp.Key + " " + ":" + " " + "TableBaseData");
            sw.WriteLine("{");

            /*生成嵌套类*/
            if (AllNestInfo.ContainsKey(kvp.Key))
            {
                List<NestClassInfo> nestClassInfo = AllNestInfo[kvp.Key];
          
                for (int i = 0; i < nestClassInfo.Count; ++i)
                {
                    sw.Write('\n');
                    sw.Write("\t");
                    sw.WriteLine("[Serializable]");

                    sw.Write("\t");
                    sw.WriteLine("public" + " " + "class" + " " + nestClassInfo[i].NestName);

                    sw.Write("\t");
                    sw.WriteLine("{");

                    Dictionary<int, FieldInfoTable> FiledInfo = nestClassInfo[i].NestDict;
                    foreach (KeyValuePair<int, FieldInfoTable> fit in FiledInfo)
                    {
                        if (fit.Value.IsComArray)
                        {
                            sw.Write("\t\t");
                            sw.WriteLine("public" + " " + fit.Value.DataType + " " + "[ ]" + " " + fit.Value.VaryName + ";" + "\t//" + fit.Value.FieldNote);
                        }
                        else
                        {
                            sw.Write("\t\t");
                            sw.WriteLine("public" + " " + fit.Value.DataType + " " + fit.Value.VaryName + ";");
                        }
                    }

                    sw.Write("\t");
                    sw.WriteLine("}");
                }
            }
            /*完成生成嵌套类*/


            /*生成基本类的信息*/
            Dictionary<int, FieldInfoTable> fieldInfoTable = kvp.Value;
            foreach (KeyValuePair<int, FieldInfoTable> kvpFit in fieldInfoTable)
            {
                FieldInfoTable singleField = kvpFit.Value;
                if (singleField.IsStruct)
                {
                    if (singleField.StrArrLen > 1)
                    {
                        sw.Write("\t");
                        sw.WriteLine("public"+ " " + singleField.DataType + " "  + "[ ]" + " " + singleField.VaryName  +" = " + "new" + " " + singleField.DataType + "[" + singleField.StrArrLen + "]" + ";" + "\t//" + singleField.FieldNote);
       
                    }
                    else
                    {
                        sw.Write("\t");
                        sw.WriteLine("public" + " " + singleField.DataType + " " + singleField.VaryName + ";"   +"\t//" + singleField.FieldNote);
                    }
                }
                else
                {
                    if (singleField.IsComArray)
                    {
                        if (singleField.ComArrLen != 0)
                        {
                            sw.Write("\t");
                            sw.WriteLine("public" + " " + singleField.DataType + " " + "[ ]" + " " + singleField.VaryName + " = " + "new" + " " + singleField.DataType + "[" + singleField.ComArrLen + "]" + ";" + "\t//" + singleField.FieldNote);
                        }
                        else if(singleField.ComArrLen == 0)
                        {
                            sw.Write("\t");
                            sw.WriteLine("public" + " " + singleField.DataType + " " + "[ ]" + " " + singleField.VaryName  + ";" + "\t//" + singleField.FieldNote);
                        }
                    }
                    else
                    {
                        sw.Write("\t");
                        sw.WriteLine("public"+ " " + singleField.DataType + " " + singleField.VaryName + ";" + "\t//" + singleField.FieldNote);
                    }
                }
            }
            /*处理完成生成基类的信息*/

            sw.WriteLine("}");
            sw.Write("\n");
            sw.WriteLine();

            /*处理生成类下面的结构体信息,辅助类*/
            
            sw.WriteLine("public" + " " + "struct" + " " + kvp.Key + "Struct");

            sw.WriteLine("{");

            AssistInfo assistInfo = AllStructInfo[kvp.Key];
            sw.Write("\t\t");
            sw.WriteLine("public" + " " + "static" + " " + "string" + " " + "SheetName" + " = " + "\"" +assistInfo.SheetName + "\"" + ";");
            sw.Write("\t\t");
            sw.WriteLine("public" + " " + "static" + " " + "string" + " " + "KeyWords" + " = " + "\"" + assistInfo.KeyWords + "\"" + ";");
            sw.Write("\t\t");
            sw.WriteLine("public" + " " + "static" + " " + "string" + " " + "DataTablePath" + " = " + "\"" + assistInfo.TablePath + "\"" + ";");

            sw.WriteLine("}");
            /*处理生成结构体信息完成*/

            sw.Flush();
            sw.Close();
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Create DataTable File calss
    /// </summary>
    public static void CreateDataTableFile()
    {
        foreach (KeyValuePair<string, Dictionary<int, FieldInfoTable>> kvp in AllTableInfo)
        {
            string filePath = Application.dataPath + "/Scripts" + "/Table/" + kvp.Key + "DataFile";
            FileStream fs = new FileStream(filePath + ".cs", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int f = 0; f < ClassNameSpace.Length; f++)
            {
                sw.WriteLine(ClassNameSpace[f]);
            }

            sw.WriteLine("public" + " " + "class" + " " + kvp.Key + "DataFile" + " " + ":" + " " + "TableDataFile");
            sw.WriteLine("{");

            sw.Write('\t');
            sw.WriteLine("public" + " " + "List" + "<" + kvp.Key + ">" + " " + kvp.Key + "List" + " = " + "new" + " " + "List" + "<" + kvp.Key + ">" + "()" + ";");

            sw.WriteLine("}");
            sw.Flush();
            sw.Close();
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Create table asset class
    /// </summary>
    public static void CreateTableAsset()
    {
        string filePath = AssetPath + "/Editor/" + "CreateTableAsset";
        FileStream fs = new FileStream(filePath + ".cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        byte[] arryByte = { 0xef, 0xbb, 0xbf };
        sw.Write(System.Text.Encoding.UTF8.GetString(arryByte));

        for (int j = 0; j < ClassNameSpace.Length; j++)
        {
            sw.WriteLine(ClassNameSpace[j]);
        }
        sw.WriteLine("using UnityEditor;");
        sw.WriteLine("using System.Data;");

        sw.WriteLine('\n');
        sw.WriteLine("public" + " " + "class CreateTableAsset");

        sw.WriteLine("{");

        sw.Write("\t");
        sw.WriteLine("[MenuItem(" + "\"AssetBundles/Create Table Asset策划打表用\"" + ")]");

        sw.Write("\t");
        sw.WriteLine("public static void Execute()");

        sw.Write("\t");
        sw.WriteLine("{");

        foreach (KeyValuePair<string, Dictionary<int, FieldInfoTable>> kvp in AllTableInfo)
        {
            string dataFileInfoStr = kvp.Key + "Data";
            sw.Write("\t\t");
            sw.WriteLine((kvp.Key + "DataFile") + " " + dataFileInfoStr + " = " + "ScriptableObject.CreateInstance<" + (kvp.Key + "DataFile") + ">();");

            sw.Write('\n');
            string IDStr = kvp.Key + "ID";
            string AttrStr = kvp.Key + "Attr";
            sw.Write("\t\t");
            sw.WriteLine("List<int>" + " " + IDStr + " = " + "new List<int>();");
            sw.Write("\t\t");
            sw.WriteLine("List<" + kvp.Key + ">" + " " + AttrStr + " = " + "new List<" + kvp.Key + ">();");

            AssistInfo assistInfo = AllStructInfo[kvp.Key];
           
            sw.Write('\n');
            string sheetName = kvp.Key + "SheetName";
            sw.Write("\t\t");
            sw.WriteLine("string" + " " + sheetName + " = " + "\"" + assistInfo.SheetName + "\"" + ";");

            string filePathName = kvp.Key + "FilePath";
            sw.Write("\t\t");
            sw.WriteLine("string" + " " + filePathName + " = " + "Application.dataPath" + "+" + "\"" + assistInfo.TablePath + "\"" + ";");

            sw.Write('\n');
            string dataTableStr = kvp.Key + "DataTable";
            sw.Write("\t\t");
            sw.WriteLine("DataTable" + " " + dataTableStr + " = " + "CreateUnityClass.ExcelToDataTable(" + filePathName + "," + sheetName + "," + " false);");

            string drStr = "dr";
            sw.Write("\t\t");
            sw.WriteLine("foreach" + "(" + "DataRow" + " " + drStr + " in " + dataTableStr + ".Rows" + ")");
            sw.Write("\t\t");
            sw.WriteLine("{");

            string objectStr = "main" + kvp.Key;
            sw.Write("\t\t\t");
            sw.WriteLine(kvp.Key + " " + objectStr + " = " + "new" + " " + kvp.Key + "();");

            Dictionary<int, FieldInfoTable> fieldInfoDict = AllTableInfo[kvp.Key];
            foreach (KeyValuePair<int, FieldInfoTable> kvpFIT in fieldInfoDict)
            {
                FieldInfoTable field = kvpFIT.Value;

                if (field.IsStruct)
                {
                    if (field.StrArrLen > 1)
                    {
                        string subStr = field.FieldNote;

                        List<NestClassInfo> nestClassList = AllNestInfo[kvp.Key];

                        for (int i = 0; i < nestClassList.Count; ++i)
                        {
                            if (nestClassList[i].NestName == field.DataType)
                            {
                                for (int j = 1; j <= field.StrArrLen; ++j)
                                {
                                    sw.Write("\t\t\t");
                                    sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j-1).ToString() +"]" + " = " + "new " + " " + kvp.Key + "." + field.DataType + "()" + ";");
                                    sw.WriteLine();
                                    
                                    Dictionary<int, FieldInfoTable> nestFieldInfo = nestClassList[i].NestDict;
                                    for (int nest = 0; nest < nestFieldInfo.Count; ++nest)
                                    {
                                        //sw.Write("\t\t\t");
                                        //sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote+ j.ToString()+ nestFieldInfo[nest].FieldNote) + "\"" + ")" + ")");

                                        //sw.Write("\t\t\t");
                                        //sw.WriteLine("{");
                                        //sw.Write("\t\t\t\t");

                                        //if (nestFieldInfo[nest].DataType == "string")
                                        //{
                                        //    sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + " = "  + drStr + "[" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ";");
                                     
                                        //}
                                        //else
                                        //{
                                        //    sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + drStr + "[" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ")" + ";");
                                        //}

                                        //sw.Write("\t\t\t");
                                        //sw.WriteLine("}");

                                        if (nestFieldInfo[nest].IsComArray)
                                        {
                                            sw.Write("\t\t\t");
                                            sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + ")" + ")");

                                            sw.Write("\t\t\t");
                                            sw.Write("{");

                                            sw.Write("\t\t\t\t");
                                            sw.WriteLine("string [] strNum" + " = " + "CreateUnityClass.GetIntArray" + "(" + drStr + "[" + "\"" + field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote + "\"" + "]" + ".ToString()" + ")" + ";");

                                            sw.Write("\t\t\t\t");
                                            sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + " = " + "new" + " " + nestFieldInfo[nest].DataType + "[" + "strNum.Length" + "];");

                                            sw.Write("\t\t\t\t");
                                            sw.WriteLine("for(int j = 0;j<strNum.Length;j++)");

                                            sw.Write("\t\t\t\t");
                                            sw.WriteLine("{");

                                            if (nestFieldInfo[nest].DataType == "string")
                                            {
                                                sw.Write("\t\t\t\t\t");
                                                sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + "[j]" + " = " + " strNum[j]  " + ";");
                                            }
                                            else
                                            {
                                                sw.Write("\t\t\t\t\t");
                                                sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + "[j]" + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + " strNum[j]  " + ")" + ";");
                                            }

                                            sw.Write("\t\t\t\t");
                                            sw.WriteLine("}");

                                            sw.Write("\t\t\t");
                                            sw.Write("}");
                                        }
                                        else
                                        {
                                            sw.Write("\t\t\t");
                                            sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + ")" + ")");

                                            sw.Write("\t\t\t");
                                            sw.WriteLine("{");
                                            sw.Write("\t\t\t\t");

                                            if (nestFieldInfo[nest].DataType == "string")
                                            {
                                                sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + " = " + drStr + "[" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ";");

                                            }
                                            else
                                            {
                                                sw.WriteLine(objectStr + "." + field.VaryName + "[" + (j - 1).ToString() + "]" + "." + nestFieldInfo[nest].VaryName + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + drStr + "[" + "\"" + (field.FieldNote + j.ToString() + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ")" + ";");
                                            }

                                            sw.Write("\t\t\t");
                                            sw.WriteLine("}");
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        string subStr = field.FieldNote;
  
                         List<NestClassInfo> nestClassList = AllNestInfo[kvp.Key];
                        for (int i = 0; i < nestClassList.Count; ++i)
                        {
                            if (nestClassList[i].NestName == field.DataType)
                            {
                                sw.Write("\t\t\t");
                                sw.WriteLine(objectStr + "." + field.VaryName + " = " + "new " + " " + kvp.Key + "." + field.DataType  +"()" + ";");
                                sw.WriteLine();

                                Dictionary<int, FieldInfoTable> nestFieldInfo = nestClassList[i].NestDict;

                                for (int nest = 0; nest < nestFieldInfo.Count; ++nest)
                                {
                                    //sw.Write("\t\t\t");
                                    //sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote)+ "\"" + ")" + ")");
                                    
                                    //sw.Write("\t\t\t");
                                    //sw.WriteLine("{");
                                    //sw.Write("\t\t\t\t");
                                    //if (nestFieldInfo[nest].DataType == "string")
                                    //{
                                    //    sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + " = "+ drStr + "[" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()"  + ";");

                                    //}
                                    //else
                                    //{
                                    //    sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + drStr + "[" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ")" + ";");
                                    //}

                                    //sw.Write("\t\t\t");
                                    //sw.WriteLine("}");

                                    if (nestFieldInfo[nest].IsComArray)
                                    {
                                        sw.Write("\t\t\t");
                                        sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + ")" + ")");

                                        sw.Write("\t\t\t");
                                        sw.Write("{");

                                        sw.Write("\t\t\t\t");
                                        sw.WriteLine("string [] strNum" + " = " + "CreateUnityClass.GetIntArray" + "(" + drStr + "[" + "\"" + field.FieldNote + nestFieldInfo[nest].FieldNote + "\"" + "]" + ".ToString()" + ")" + ";");

                                        sw.Write("\t\t\t\t");
                                        sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + " = " + "new" + " " + nestFieldInfo[nest].DataType + "[" + "strNum.Length" + "];");

                                        sw.Write("\t\t\t\t");
                                        sw.WriteLine("for(int j = 0;j<strNum.Length;j++)");

                                        sw.Write("\t\t\t\t");
                                        sw.WriteLine("{");

                                        if (nestFieldInfo[nest].DataType == "string")
                                        {
                                            sw.Write("\t\t\t\t\t");
                                            sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + "[j]" + " = " + " strNum[j]  " + ";");
                                        }
                                        else
                                        {
                                            sw.Write("\t\t\t\t\t");
                                            sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + "[j]" + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + " strNum[j]  " + ")" + ";");
                                        }

                                        sw.Write("\t\t\t\t");
                                        sw.WriteLine("}");

                                        sw.Write("\t\t\t");
                                        sw.Write("}");
                                    }
                                    else
                                    {
                                        sw.Write("\t\t\t");
                                        sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + ")" + ")");

                                        sw.Write("\t\t\t");
                                        sw.WriteLine("{");
                                        sw.Write("\t\t\t\t");
                                        if (nestFieldInfo[nest].DataType == "string")
                                        {
                                            sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + " = " + drStr + "[" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ";");
                                                                                    }
                                        else
                                        {
                                            sw.WriteLine(objectStr + "." + field.VaryName + "." + nestFieldInfo[nest].VaryName + " = " + nestFieldInfo[nest].DataType + ".Parse" + "(" + drStr + "[" + "\"" + (field.FieldNote + nestFieldInfo[nest].FieldNote) + "\"" + "]" + ".ToString()" + ")" + ";");
                                        }

                                        sw.Write("\t\t\t");
                                        sw.WriteLine("}");
    
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (field.IsComArray)
                    {
                        string topStr = field.FieldNote;
                        topStr = topStr.TrimEnd("*".ToCharArray());

                        if (field.ComArrLen != 0)
                        {
                            for (int i = 1; i <= field.ComArrLen; ++i)
                            {
                                sw.Write("\t\t\t");
                                sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + (topStr + i.ToString()) + "\"" + ")" + ")");

                                sw.Write("\t\t\t");
                                sw.WriteLine("{");

                                if (field.DataType == "string")
                                {
                                    sw.Write("\t\t\t\t");
                                    sw.WriteLine(objectStr + "." + field.VaryName + "[" + (i - 1) + "]" + " = " + 
                                        drStr + "[" + "\"" + (topStr + i.ToString()) + "\"" + "]" + ".ToString()"+ ";");

                                }
                                else
                                {
                                    sw.Write("\t\t\t\t");
                                    sw.WriteLine(objectStr + "." + field.VaryName + "[" + (i - 1) + "]" + " = " + 
                                        field.DataType + ".Parse" + "(" + drStr + "[" + "\"" + (topStr + i.ToString()) + "\"" + "]" + ".ToString()" + ")" + ";");

                                }


                                sw.Write("\t\t\t");
                                sw.WriteLine("}");
                            }
                        }
                        else if (field.ComArrLen == 0)
                        {
                            sw.Write("\t\t\t");
                            sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + field.FieldNote + "\"" + ")" + ")");

                            sw.Write("\t\t\t");
                            sw.Write("{");

                            sw.Write("\t\t\t\t");
                            sw.WriteLine("string [] strNum" + " = " + "CreateUnityClass.GetIntArray" + "(" + drStr + "[" + "\"" + field.FieldNote + "\"" + "]" + ".ToString()" + ")" + ";");

                            sw.Write("\t\t\t\t");
                            sw.WriteLine(objectStr + "." + field.VaryName + " = " + "new" + " " + field.DataType + "[" + "strNum.Length" + "];");

                            sw.Write("\t\t\t\t");
                            sw.WriteLine("for(int j = 0;j<strNum.Length;j++)");

                            sw.Write("\t\t\t\t");
                            sw.WriteLine("{");

                            if (field.DataType == "string")
                            {
                                sw.Write("\t\t\t\t\t");
                                sw.WriteLine(objectStr + "." + field.VaryName + "[j]" + " = " + " strNum[j]  " + ";");
                            }
                            else
                            {
                                sw.Write("\t\t\t\t\t");
                                sw.WriteLine(objectStr + "." + field.VaryName + "[j]" + " = " + field.DataType + ".Parse" + "(" + " strNum[j]  " + ")" + ";");
                            }

                            sw.Write("\t\t\t\t");
                            sw.WriteLine("}");

                            sw.Write("\t\t\t");
                            sw.Write("}");
                        }
                    }
                    else
                    {
                        sw.Write("\t\t\t");
                        sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + field.FieldNote + "\"" + ")" + ")");

                        sw.Write("\t\t\t");
                        sw.WriteLine("{");

                        if (field.DataType == "string")
                        {
                            sw.Write("\t\t\t\t");
                            sw.WriteLine(objectStr + "." + field.VaryName + " = " + drStr + "[" + "\"" + field.FieldNote + "\"" + "]" + ".ToString()" + ";");
                        }
                        else
                        {
                            sw.Write("\t\t\t\t");
                            sw.WriteLine(objectStr + "." + field.VaryName + " = " + field.DataType + ".Parse" + "(" + drStr + "[" + "\"" + field.FieldNote + "\"" + "]" + ".ToString()" + ")" + ";");
                        }
                        

                        sw.Write("\t\t\t");
                        sw.WriteLine("}");
                    }
                }

            }

            sw.Write('\n');

            sw.Write("\t\t\t");
            sw.WriteLine("if" + "(!" + drStr + "." + "IsNull" + "(" + "\"" + assistInfo.KeyWords + "\"" + ")" + ")");

            sw.Write("\t\t\t");
            sw.WriteLine("{");

            sw.Write("\t\t\t\t");
            sw.WriteLine(IDStr + ".Add" + "(" + "int" + ".Parse" + "(" + drStr + "[" + "\"" + assistInfo.KeyWords + "\"" + "]" + ".ToString()" + ")" + ")" + ";");

            sw.Write("\t\t\t\t");
            sw.WriteLine(AttrStr + ".Add" + "(" + objectStr + ");");

            sw.Write("\t\t\t");
            sw.WriteLine("}");

            sw.Write("\t\t");
            sw.WriteLine("}");

            sw.Write("\t\t");
            sw.WriteLine(dataFileInfoStr + "." + "IDs" + " = " + IDStr + ";");

            sw.Write("\t\t");
            sw.WriteLine(dataFileInfoStr + "." + kvp.Key + "List" + " = " + AttrStr + ";");

            sw.Write("\t\t");
            sw.WriteLine("AssetDatabase.CreateAsset" + "(" + dataFileInfoStr + "," + "\"" + "Assets/Resources/TableAsset/" + kvp.Key + ".asset" + "\"" + ");");


            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine();
        }


        sw.WriteLine();
        sw.Write("\t\t");
        sw.WriteLine("EditorUtility.DisplayDialog(\"提示\", \"打表完成\", \"OK\");");


        sw.Write("\t");
        sw.WriteLine("}");

        sw.WriteLine("}");
        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();

    }
    #endregion

    public static void CreateTableManager()
    {
        string filePath = Application.dataPath + "/Scripts" + "/Table/" + "TableManager";
        FileStream fs = new FileStream(filePath + ".cs", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        byte[] arryByte = { 0xef, 0xbb, 0xbf };
        sw.Write(System.Text.Encoding.UTF8.GetString(arryByte));

        for (int f = 0; f < ClassNameSpace.Length; f++)
        {
            sw.WriteLine(ClassNameSpace[f]);
        }

        sw.WriteLine('\n');
        sw.WriteLine("public" + " " + "class" + " " + "TableManager");

        sw.WriteLine("{");

        sw.Write("\t");
        sw.WriteLine("private Dictionary<int, TableDataDict> TableData = new Dictionary<int, TableDataDict>();");
        
        sw.WriteLine();
       
        sw.Write("\t");
        sw.WriteLine("public void Init()");

        sw.Write("\t");
        sw.WriteLine("{");

        foreach (KeyValuePair<string, Dictionary<int, FieldInfoTable>> kvp in AllTableInfo)
        {
            sw.Write("\t\t");
			sw.WriteLine( (kvp.Key) + "DataFile" + " " + (kvp.Key + "Asset" + "DataFile") + " = " + "ResourceManager.Load<" + (kvp.Key) + "DataFile>" +
				"(TablePath.Paths[(int)TableID." + (kvp.Key + "ID") + "]);");


            sw.WriteLine();
            sw.Write("\t\t");
            sw.WriteLine("if" + "(" + (kvp.Key+ "Asset" + "DataFile") + ".IDs" + " != null" + ")");

            sw.Write("\t\t");
            sw.WriteLine("{");

            sw.Write("\t\t\t");
            sw.WriteLine("TableDataDict dataDict = new TableDataDict();");

            sw.Write("\t\t\t");
            sw.WriteLine("for(int i = 0, max = " + (kvp.Key + "Asset" + "DataFile") + ".IDs.Count; i < max; ++i)");
            
            sw.Write("\t\t\t");
            sw.WriteLine("{");

            sw.Write("\t\t\t\t");

            sw.WriteLine("if(!dataDict.Add(" + (kvp.Key +"Asset" +"DataFile") + "." + "IDs[i]" + "," +(kvp.Key+"Asset" +"DataFile") + "."+ (kvp.Key + "List"+"[i]") + "))");

            sw.Write("\t\t\t\t");
            sw.WriteLine("{");

            sw.Write("\t\t\t\t\t");
            sw.WriteLine(" Debug.LogError(" + "\"" + "Exception :" + "\"" + " +" + kvp.Key + "Asset" + "DataFile.name"+");");

            sw.Write("\t\t\t\t");
            sw.WriteLine("}");


            sw.Write("\t\t\t");
            sw.WriteLine("}");

            //sw.Write("\t\t\t");
            //sw.WriteLine("ResourceMgr.UnloadAsset((int)TableID" + "." + (kvp.Key + "ID") + ");");

            sw.Write("\t\t\t");
            sw.WriteLine("TableData.Add" + "(" + "(int)TableID." + (kvp.Key + "ID") + "," + "dataDict" + ");");   
    
            sw.Write("\t\t");
            sw.WriteLine("}");

            sw.WriteLine();
            sw.WriteLine();
            sw.WriteLine();
            
        }

        sw.Write("\t\t");
		sw.WriteLine("ResourceManager.UnloadUnusedResources();");
		sw.Write("\t");
        sw.WriteLine("}");

        sw.WriteLine();
        sw.WriteLine();

        sw.Write("\t");
        sw.WriteLine("public TableDataDict GetTable(int tableID)");

        sw.Write("\t");
        sw.WriteLine("{");

        sw.Write("\t\t");
		sw.WriteLine("return TableData[tableID];");

        sw.Write("\t");
        sw.WriteLine("}");


        sw.Write("\t");
        sw.WriteLine("public TableBaseData GetTableData(int tableID, int dataID)");

        sw.Write("\t");
        sw.WriteLine("{");

        sw.Write("\t\t");
        sw.WriteLine("TableDataDict dataDict = TableData[tableID];");

        sw.Write("\t\t");
        sw.WriteLine("return dataDict.GetValue(dataID);");

        sw.Write("\t");
        sw.WriteLine("}");


        sw.WriteLine("}");

        sw.Flush();
        sw.Close();
    }

    /// <summary>
    /// To storage the data of excle table in dataTable
    /// </summary>
    /// <param name="filePath">file path</param>
    /// <param name="sheetName">sheet name</param>
    /// <param name="isFirstRow">Whether from the first line read</param>
    /// <returns>dataTable</returns>
    public static DataTable ExcelToDataTable(string filePath, string sheetName, bool isFirstRow)
    {
        int startRow = 0;
        ISheet sheet = null;
        IWorkbook workbook = null;
        DataTable data = new DataTable();

        try
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            if (filePath.IndexOf(".xlsx") > 0)
            {
                workbook = new XSSFWorkbook(fs);
            }
            else if (filePath.IndexOf(".xls") > 0)
            {

                workbook = new HSSFWorkbook(fs);
            }

            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }

            if (sheet != null)
            {

                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum;

                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {

                    DataColumn column;
                    if (firstRow.GetCell(i) == null)
                    {
                        column = new DataColumn();
                    }
                    else
                    {
                        column = new DataColumn(firstRow.GetCell(i).StringCellValue);
                    }
                    data.Columns.Add(column);
                }

                if (!isFirstRow)
                {
                    startRow = sheet.FirstRowNum + 1;
                }

                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　  

                    bool mark = false;
                    DataRow dataRow = data.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {

                        //if (j == row.FirstCellNum)
                        //{
                        //    if (row.GetCell(j) == null || string.IsNullOrEmpty(row.GetCell(j).ToString().Trim()))
                        //    {
                        //        mark = true;
                        //        break;
                        //    }
                        //}

                        if (row.GetCell(j) != null && row.GetCell(j).ToString().Length > 0) //同理，没有数据的单元格都默认是null     
                        {
                            ICell cell = row.GetCell(j);
                            string strCell = null;

                            switch (cell.CellType)
                            {
                                case CellType.Formula:
                                    try
                                    {
                                        strCell = cell.NumericCellValue.ToString();
                                    }
                                    catch
                                    {
                                        strCell = cell.StringCellValue.ToString();
                                    }
                                    break;

                                case CellType.String:
                                    strCell = cell.ToString();
                                    break;

                                case CellType.Numeric:
                                    strCell = cell.ToString();
                                    break;

                                case CellType.Boolean:
                                    strCell = cell.ToString();
                                    break;

                                case CellType.Blank:
                                    strCell = "";
                                    break;
                            }

                            dataRow[j] = strCell;

                        }
                    }

                    data.Rows.Add(dataRow);
                }
            }
            return data;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("Exception: {0}. FileName:{1}", ex.Message, filePath);
            return null;
        }
    }

    public static string[] GetIntArray(string str)
    {
        string[] split = str.Split(',');
        return split;
    }
}

