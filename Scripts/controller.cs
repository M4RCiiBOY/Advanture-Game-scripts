using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{

    public string[] controllers;
    void Start () {
        controllers = new string[10];
        controllers = Input.GetJoystickNames();
        foreach (var item in controllers)
        {
            Debug.Log(item);
        }
    }
	
	

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {

        }
	}
}
