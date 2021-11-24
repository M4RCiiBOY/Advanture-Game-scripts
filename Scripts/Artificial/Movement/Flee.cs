using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour, IMovement
{
    float MaxAcceleration;
    GameObject[] players;
    Vector3 fleeTarget;

    private void Start()
    {
        MaxAcceleration = gameObject.GetComponent<AIBody>().MaxAcceleration;
        players = gameObject.GetComponent<AIBody>().Players;
    }
    public Vector3 Accelerate()
    {
        fleeTarget = Vector3.zero;
        for (int i = 0; i < players.Length; i++)
        {
            fleeTarget += transform.position - players[i].transform.position;
        }
        fleeTarget /= players.Length;
        return fleeTarget.normalized * MaxAcceleration;
    }
}
