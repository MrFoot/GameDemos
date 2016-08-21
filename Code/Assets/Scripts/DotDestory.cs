using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;
using UnityEngine.SceneManagement;
using System;

public class DotDestory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public struct NameStruct
    {
        public string Text;
    }

	public class Name
	{
        private string _name;
		public string MyName{
            set{_name = value;}
            get{return _name;}
        }

        public NameStruct Hi = new NameStruct();

        public DateTime MyTime;

        public string Foo()
        {
            return "";
        }

	}

	public void Test() {
        //Main.Instance.BearCharacter.SetRun(true);
        //Debug.Log(Main.Instance.TableManager.GetTable((int)TableID.ItemTableID).TableDatas[1001100]);

        //Name n = new Name();
        //n.MyName = "XCX";
        //n.MyTime = DateTime.Now;
        //n.Hi.Text = "Hi";
        //NameStruct nHi = n.Hi;
        //nHi.Text = "new Hi";

        //UserPrefs.SetXml<Name>("tt", n);

	}

	public void GoFarm() {
        gameObject.SetActive(false);

        SceneManager.LoadScene("Farm");


        //Name nn = UserPrefs.GetXml<Name>("tt",null);
        //Debug.Log("MyName = " + nn.MyName);
        //Debug.Log("MyTime = " + nn.MyTime);
        //Debug.Log("MyStruct = " + nn.Hi.Text);
        
	}

	public void GoDress() {
		Main.Instance.GameStateManager.FireAction (GameAction.OpenWardrobe);
	}
}
