using UnityEngine;
using System.Collections;

public class MainWindow : WindowBase {

    public override void OnOpen(object[] args)
    {
        base.OnOpen(args);
    }

    public void OnClick()
    {
        WindowManager.OpenWindow((int)WindowType.TipsWindow);
    }
}
