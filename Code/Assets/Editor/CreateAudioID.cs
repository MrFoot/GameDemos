using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using FootStudio.Framework;

public class CreateAudioID : MonoBehaviour {

    private const string ExcelPath = "Assets/Table/音效配置.xls";

    private const string SheetName = "Sheet1";

    public static string AssetPath = Application.dataPath;

    private const string AudioManagerPrefPath = @"Assets/Resources/Sound/AudioManager.prefab";

    public static string[] ClassNameSpace = {  "using System;",
                                                //"using UnityEngine;",
                                                //"using System.IO;",
                                                //"using System.Collections;",
                                                //"using System.Collections.Generic;",
                                                //"using Soulgame.Util;",
                                             };

    [MenuItem("AssetBundles/导出音效配置")]
    public static void Execute() {

        //string pathPrefix = @"Assets/Table/";

        //UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        //if (selection == null || selection.Length != 1 )
        //{
        //    Debug.LogError("请选择音效配置Excel");
        //    return;
        //}

        //UnityEngine.Object o = (UnityEngine.Object)selection[0];

        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(ExcelPath,typeof(UnityEngine.Object)) as UnityEngine.Object;

        string oriPath = AssetDatabase.GetAssetPath(o);

        if (oriPath.EndsWith("音效配置.xls"))
        {
            DataTable table = GetDatatable(oriPath, SheetName);

            if(table != null)
            {
                Export(table);

                UpdatePrefab(table);

                Debug.Log("导出成功，请即时上传配置");
            }
            else
            {
                Debug.LogError("请查看配置表是否打开状态");
            }
        }
        else
        {
            Debug.LogError("请选择音效配置Excel");
            return;
        }

    }

    static DataTable GetDatatable(string filePath, string sheetName) {
        DataTable table = CreateUnityClass.ExcelToDataTable(filePath,sheetName,false);
        return table;
    }

    static void Export(DataTable table) {

        string filePath = AssetPath + "/Scripts/Framework/Sound/AudioResource.cs";

        FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        fs.SetLength(0);
        StreamWriter sw = new StreamWriter(fs);

        byte[] utf8Byte = { 0xef, 0xbb, 0xbf };

        sw.Write(System.Text.Encoding.UTF8.GetString(utf8Byte));

        for (int f = 0 ; f < ClassNameSpace.Length ; f++)
        {
            sw.WriteLine(ClassNameSpace[f]);
        }

        sw.WriteLine('\n');

        sw.WriteLine("public enum AudioEnum {");

        sw.Write("\n");

        sw.Write("\t");

        foreach (DataRow dr in table.Rows)
        {
            int id = -1;
            
            if(int.TryParse(dr["ID"].ToString(),out id))
            {
                sw.Write("\t");

                string name = dr["常量名"].ToString();

                if(name.Equals(""))
                {
                    name = "No_Const_Name_" + id;
                }

                string desc = dr["类型"].ToString() + " | " + 
                              dr["音效"].ToString() + " | " + 
                              dr["描述"].ToString() + " | " +  
                              dr["可否循环"].ToString();

                sw.WriteLine(name + " = " + id + "," + "\t// " + desc);

                sw.Write("\n");
            }
        }

        sw.WriteLine("}");

        sw.Flush();
        sw.Close();

        AssetDatabase.Refresh();

    }

    static void UpdatePrefab(DataTable table) {

        GameObject prefab = AssetDatabase.LoadAssetAtPath(AudioManagerPrefPath, typeof(GameObject)) as GameObject;

        SoundManager script = prefab.GetComponent<SoundManager>();

        if (script == null)
        {
            Debug.LogError("No Script AudioManager");
            return;
        }

        script.PreLoadMusics = new List<AudioClip>();

        foreach (DataRow dr in table.Rows)
        {
            int id = -1;

            if (int.TryParse(dr["ID"].ToString(), out id))
            {
                int isPreload = -1;

                if(int.TryParse(dr["预加载"].ToString(),out isPreload))
                {
                    if (isPreload == 1)
                    {
                        string path = AudioManagerPrefPath.Remove(AudioManagerPrefPath.LastIndexOf("/") + 1) + id + ".mp3";
                        AudioClip clip = AssetDatabase.LoadAssetAtPath(path,typeof(AudioClip)) as AudioClip;

                        if(clip == null)
                        {
                            path = AudioManagerPrefPath.Remove(AudioManagerPrefPath.LastIndexOf("/") + 1) + id + ".wav";
                            clip = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;
                        }

                        if (clip == null)
                        {
                            path = AudioManagerPrefPath.Remove(AudioManagerPrefPath.LastIndexOf("/") + 1) + id + ".ogg";
                            clip = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;
                        }

                        if(clip != null)
                        {
                            script.PreLoadMusics.Add(clip);
                            Debug.Log("预加载 : " + id);
                        }
                    }
                }
            }
        }

        AssetDatabase.Refresh();

    }
}