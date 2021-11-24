using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour, IMovement
{
    float MaxAcceleration;
    Vector3 target;
    Vector3 attackTarget;
    AIBody body;

    private void Start()
    {
        body = gameObject.GetComponent<AIBody>();
        MaxAcceleration = body.MaxAcceleration;
    }
    public Vector3 Accelerate()
    {
        target = body.ClosestPlayer;
        attackTarget = Quaternion.Euler(0, 0, 90) * (transform.position - target).normalized + target - transform.position;
        return attackTarget.normalized * MaxAcceleration;
    }
}