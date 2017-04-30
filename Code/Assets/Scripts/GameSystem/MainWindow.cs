using UnityEngine;
using System.Collections;

public class MainWindow : WindowBase {

    public override void OnOpen(object[] args)
    {
        base.OnOpen(args);
    }

    public void OnClick()
    {
        Debug.LogError("MainWindow OnClick");
        WindowManager.OpenWindow((int)WindowType.TipsWindow);
    }

    public void ToTest()
    {
        Main.Instance.SceneStateManager.FireAction(SceneAction.ToTest);
    }

    public void ToGame1()
    {
        Main.Instance.SceneStateManager.FireAction(SceneAction.ToGame_1);
    }

    public void ToBack()
    {
        Main.Instance.SceneStateManager.FireAction(SceneAction.BackButton);
    }
}
