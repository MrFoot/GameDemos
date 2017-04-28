using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum WindowType : int
{
    #region 界面
    
    //主界面
    MainNormalWin = 3000,
    TipsWindow = 3001,
    #endregion
}

public class PrefabPathInfo 
{
    public static void InitPrefabPath()
    {

        //主界面
        WindowManager.SetWindowPrefabInfo((int)WindowType.MainNormalWin, "UI/MainWindow", "MainWindow", false);

        WindowManager.SetWindowPrefabInfo((int)WindowType.TipsWindow, "UI/TipsWindow", "TipsWindow", false);
    }
}
