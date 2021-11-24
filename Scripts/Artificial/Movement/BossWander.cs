using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWander : MonoBehaviour, IMovement
{
    public float MaxDistance = 1;

    float dist2Players;

    float MaxAcceleration;
    GameObject[] players;
    AIBody body;

    Vector3 midPos;
    Vector3 acceleration;


    private void Start()
    {
        body = gameObject.GetComponent<AIBody>();
        MaxAcceleration = body.MaxAcceleration;
        players = body.Players;
    }

    public Vector3 Accelerate()
    {
        midPos = Vector3.zero;
        for (int i = 0; i < players.Length; i++)
        {
            midPos += players[i].transform.position;
        }
        midPos /= players.Length;
        acceleration = Quaternion.Euler(0, 0, 90) * (transform.position - midPos).normalized;
        dist2Players = (transform.position - midPos).magnitude;
        if (dist2Players >= Mathf.Abs(MaxDistance)) MaxDistance *= -1;
        acceleration *= MaxDistance;
        acceleration += midPos - transform.position;
        return acceleration.normalized * MaxAcceleration;
    }

    /// <summary>
    /// Call if player array needs to be changed.
    /// </summary>
    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(midPos, 0.25f);
        Gizmos.DrawWireSphere(transform.position + acceleration, 0.25f);
    }
}
