using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isTriggered;
    public int id;
    public string connection;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" || col.tag == "Player2")
        {
            Debug.Log("Enter");
            isTriggered = true;
            anim.SetBool("IsTriggered", true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Player2")
        {
            Debug.Log("Exit");
            isTriggered = false;
            anim.SetBool("IsTriggered", false);

        }
    }
}
