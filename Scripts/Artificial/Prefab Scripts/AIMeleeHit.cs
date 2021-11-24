using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AIMeleeHit : MonoBehaviour
{
    public int Damage;
    public float Radius;
    public GameObject[] Players;
    public bool Swipe;

    private void Start()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Vector2.Distance(Players[i].transform.position, transform.position) <= Radius)
            {
                Players[i].GetComponent<PlayerCtrl>().RecieveDamage(Damage);
                if (!Swipe) break;
            }
        }
        Destroy(this.gameObject);
    }
}
