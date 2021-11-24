using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpell : MonoBehaviour
{
    public float Windforce = 3;
    public bool Left;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            if (Left) collision.gameObject.transform.position -= transform.right * Windforce * Time.deltaTime;
            else collision.gameObject.transform.position += transform.right * Windforce * Time.deltaTime;
        }
    }
}
