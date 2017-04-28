using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public delegate void UpdatePerFrame(float deltaTime);

public class GameMain : MonoBehaviour 
{
	private static UpdatePerFrame m_updateDelg = null;

	public static void RegisterUpdate(UpdatePerFrame update)
	{
		if(update != null)
		{
			m_updateDelg += update;
		}
	}

    public static void UnRegisterUpdate(UpdatePerFrame update)
    {
        if (update != null)
        {
            m_updateDelg -= update;
        }
    }

    void Start()
    {
        HttpLite.Init();
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(m_updateDelg != null)
		{
			m_updateDelg(Time.deltaTime);
		}
	}

}
