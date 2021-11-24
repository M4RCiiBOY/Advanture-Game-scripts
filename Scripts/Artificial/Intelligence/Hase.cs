using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
[RequireComponent(typeof(Flee))]
[RequireComponent(typeof(AIBody))]

public class Hase : MonoBehaviour
{
    public float PanicRange;

    IMovement wander;
    IMovement flee;
    AIBody body;

    private void Awake()
    {
        wander = gameObject.GetComponent<Wander>();
        flee = gameObject.GetComponent<Flee>();
        body = gameObject.GetComponent<AIBody>();
    }

    private void Start()
    {
        body.Move = wander;
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, body.ClosestPlayer) < PanicRange)
        {
            body.Move = flee;
        }
    }
}
