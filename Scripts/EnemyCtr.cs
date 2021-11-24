using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtr : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rg;
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private Direction dir;

    void Start ()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dir = Direction.Down;
	}
	
	void Update ()
    {
        Enemy_Move();
	}

    void Enemy_Move()
    {
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("DownWalk", false);
            anim.SetBool("LeftWalk", false);
            anim.SetBool("RightWalk", false);
            anim.SetBool("UpWalk", false);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // GOING RIGHT
            rg.MovePosition(transform.position + new Vector3(.02f, 0));
            anim.SetBool("DownWalk", false);
            anim.SetBool("LeftWalk", false);
            anim.SetBool("RightWalk", true);
            anim.SetBool("UpWalk", false);
            dir = Direction.Right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // GOING LEFT
            rg.MovePosition(transform.position + new Vector3(-.02f, 0));
            anim.SetBool("DownWalk", false);
            anim.SetBool("LeftWalk", true);
            anim.SetBool("RightWalk", false);
            anim.SetBool("UpWalk", false);
            dir = Direction.Left;
        }

        if (Input.GetKey(KeyCode.W))
        {
            // GOING UP
            rg.MovePosition(transform.position + new Vector3(0, .02f));
            anim.SetBool("DownWalk", false);
            anim.SetBool("LeftWalk", false);
            anim.SetBool("RightWalk", false);
            anim.SetBool("UpWalk", true);
            dir = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // GOING DOWN
            rg.MovePosition(transform.position + new Vector3(0, -.02f));
            anim.SetBool("DownWalk", true);
            anim.SetBool("LeftWalk", false);
            anim.SetBool("RightWalk", false);
            anim.SetBool("UpWalk", false);
            dir = Direction.Down;
        }

    }
}
