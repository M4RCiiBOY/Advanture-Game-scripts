using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Strike!");
            collision.gameObject.GetComponent<AIBody>().Damage(1);
        }
    }
}
