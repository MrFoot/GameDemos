using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindowTemplate : WindowBase
{
    #region override
    protected override void Awake()
    {
        base.Awake();
        WindowInit();
    }

    void WindowInit()
    {

    }

    public override void OnOpen(object[] args)
    {
        base.OnOpen(args);
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {

    }
}
