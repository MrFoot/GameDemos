using UnityEngine;
using System.Collections;

public class TankA : BaseTank {

    public TankA(Vector3 size)
        : base(size)
    {
    }

    public TankA(int x, int y, int z)
        : base(x, y, z)
    {
    }

    public override void Init()
    {
        base.Init();
        Add();
    }

    

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
