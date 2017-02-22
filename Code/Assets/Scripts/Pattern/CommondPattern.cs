using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommondPattern : MonoBehaviour {

    JoyStick stick;
    CommondHandler handler;

	// Use this for initialization
	void Start () {
        handler = new CommondHandler();
        stick = new JoyStick(handler);
	}
	
	// Update is called once per frame
	void Update () {
        stick.HandleInput();
        handler.Excute();
	}
}

public class Unit
{
    private Transform trans;
    private float speed = 3.0f;

    public Vector3 Pos {
        get
        {
            return trans != null ? trans.position : Vector3.zero;
        }
    }

    public Unit(Transform transfrom,float spd)
    {
        trans = transfrom;
        speed = spd;
    }

    public void MoveTo(Vector3 target)
    {
        trans.position = target;
    }

    public void MoveForward() {
        trans.position += Vector3.forward * speed;
    }

    public void MoveLeft()
    {
        trans.position += Vector3.left * speed;
    }

    public void MoveRight()
    {
        trans.position += Vector3.right * speed;
    }

    public void MoveBack()
    {
        trans.position += Vector3.back * speed;
    }
}

public interface Commond
{
    void Excute();
    void Undo();
}

public class CommondMoveTo : Commond
{
    private Vector3 before;
    private Vector3 target;
    private Unit unit;

    public CommondMoveTo(Unit u, Vector3 t)
    {
        unit = u;
        target = t;
        before = unit.Pos;
    }

    public void Excute()
    {
        Debug.Log("Move to : " + target);
        unit.MoveTo(target);
    }

    public void Undo()
    {
        Debug.Log("Move back to : " + before);
        unit.MoveTo(before);
    }
}

public class CommondMoveForward : Commond
{
    private Vector3 before;
    private Unit unit;

    public CommondMoveForward(Unit u)
    {
        unit = u;
        before = unit.Pos;
    }

    public void Excute()
    {
        unit.MoveForward();
    }

    public void Undo()
    {
        Debug.Log("Move back to : " + before);
        unit.MoveTo(before);
    }
}

public class CommondMoveBack : Commond
{
    private Vector3 before;
    private Unit unit;

    public CommondMoveBack(Unit u)
    {
        unit = u;
        before = unit.Pos;
    }

    public void Excute()
    {
        unit.MoveBack();
    }

    public void Undo()
    {
        Debug.Log("Move back to : " + before);
        unit.MoveTo(before);
    }
}

public class CommondMoveLeft : Commond
{
    private Vector3 before;
    private Unit unit;

    public CommondMoveLeft(Unit u)
    {
        unit = u;
        before = unit.Pos;
    }

    public void Excute()
    {
        unit.MoveLeft();
    }

    public void Undo()
    {
        Debug.Log("Move back to : " + before);
        unit.MoveTo(before);
    }
}

public class CommondMoveRight : Commond
{
    private Vector3 before;
    private Unit unit;

    public CommondMoveRight(Unit u)
    {
        unit = u;
        before = unit.Pos;
    }

    public void Excute()
    {
        unit.MoveRight();
    }

    public void Undo()
    {
        Debug.Log("Move back to : " + before);
        unit.MoveTo(before);
    }
}

class JoyStick
{
    private Camera camera;
    CommondHandler handler;

    private ActorUnit selectedUnit;

    public JoyStick(CommondHandler h)
    {
        camera = Camera.main;
        handler = h;
    }

    public Commond HandleInput(){

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Transform tran = hit.transform;

                if (tran.tag == "plane")
                {
                    if (selectedUnit != null)
                    {
                        Commond cmd = new CommondMoveTo(selectedUnit.unit, hit.point);
                        handler.AddCommond(cmd);
                    }
                }
                else if (tran.tag == "Player")
                {
                    selectedUnit = tran.GetComponent<ActorUnit>();
                    Debug.Log("selectedUnit = " + tran.name);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (selectedUnit != null)
            {
                handler.Undo();
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            if (selectedUnit != null)
            {
                handler.Redo();
            }
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (selectedUnit != null)
            {
                Commond cmd = new CommondMoveForward(selectedUnit.unit);
                handler.AddCommond(cmd);
            }
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (selectedUnit != null)
            {
                Commond cmd = new CommondMoveBack(selectedUnit.unit);
                handler.AddCommond(cmd);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (selectedUnit != null)
            {
                Commond cmd = new CommondMoveLeft(selectedUnit.unit);
                handler.AddCommond(cmd);
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (selectedUnit != null)
            {
                Commond cmd = new CommondMoveRight(selectedUnit.unit);
                handler.AddCommond(cmd);
            }
        }

        return null;
    }
}

class CommondHandler
{
    private List<Commond> Commonds;
    private int curCommond;
    bool undoLock = false;

    public CommondHandler()
    {
        Commonds = new List<Commond>();
        curCommond = -1;
    }

    public void AddCommond(Commond cmd)
    {
        if(undoLock)
        {
            //一旦有新的输入，将回撤后当前步骤之后的所有步骤清空
            Commonds.RemoveRange(curCommond + 1, Commonds.Count - curCommond - 1);
            undoLock = false;
        }
        Commonds.Add(cmd);
    }

    public void Undo()
    {
        if (curCommond >= 0)
        {
            Commonds[curCommond--].Undo();
            undoLock = true;
        }
    }

    public void Redo()
    {
        undoLock = false;
        Excute();
        undoLock = true;
    }

    public void Excute()
    {
        if (!undoLock && curCommond < Commonds.Count - 1)
        {
            Commonds[++curCommond].Excute();
        }
    }


}