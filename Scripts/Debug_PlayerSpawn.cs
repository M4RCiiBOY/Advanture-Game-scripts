using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_PlayerSpawn : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length);
        if (GameObject.FindGameObjectsWithTag("Player").Length<1)
        {
            Instantiate(player, new Vector2(8, 8), Quaternion.identity);
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
