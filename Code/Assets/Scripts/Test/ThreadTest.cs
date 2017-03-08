using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class ThreadTest : MonoBehaviour {

    private static object locka = new object();
    private static object lockb = new object();
    private static int counter = 0;
    int max = 20;

	// Use this for initialization
	void Start () {


        Thread thread = new Thread(Thread1);
        thread.Name = "___AAA";
        thread.Start(max);

        Thread thread2 = new Thread(Thread2);
        thread2.Name = "BBB___";
        thread2.Start(max);

        Thread thread3 = new Thread(Thread2);
        thread3.Name = "CCC___";
        thread3.Start(max);

	}
	
	// Update is called once per frame
	void Update () {
	}

    static void Thread1(object param)
    {
        int max = (int) param;

        Monitor.Enter(locka);

        while (counter < max)
        {
            Monitor.Wait(locka);
            //Debug.Log(Thread.CurrentThread.Name + " : " + counter++);
            Debug.Log(Thread.CurrentThread.Name + " counter = " + counter);
            Thread.Sleep(500);
            counter++;
            Monitor.Pulse(locka);
        }

        Monitor.Exit(locka);
    }

    static void Thread2(object param)
    {
        Thread.Sleep(1000);
        int max = (int)param;

        Monitor.Enter(locka);

        while (counter < max)
        {
            Monitor.Pulse(locka);
            if (Monitor.Wait(locka, 10))
            {
                //Debug.Log(Thread.CurrentThread.Name + " : " + counter++);
                Debug.Log(Thread.CurrentThread.Name + " counter = " + counter);
                Thread.Sleep(500);
                counter++;
            }
        }

        Monitor.Exit(locka);
    }


}
