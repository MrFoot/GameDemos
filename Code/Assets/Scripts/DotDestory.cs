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


	}

	public void GoFarm() {

        
	}

	public void GoDress() {

	}
}
