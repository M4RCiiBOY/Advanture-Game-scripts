using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Teleport : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
            {
                GameObject.FindGameObjectsWithTag("Player")[i].transform.position = this.transform.parent.GetComponent<Effect_Chunk>().effect_target.position + new Vector3(8, -12);
            }
        }
    }
}
