using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAttack : MonoBehaviour, IMovement
{
    AIBody body;
    float MaxAcceleration;
    float MaxSpeed;
    public Vector3 Target;
    Vector3 attackTarget;

    public float MeleeRange;
    float brakeDistance;
    Vector3 directionToTarget;

    private void Awake()
    {
        body = gameObject.GetComponent<AIBody>();
    }
    public Vector3 Accelerate()
    {
        MaxAcceleration = gameObject.GetComponent<AIBody>().MaxAcceleration;
        MaxSpeed = gameObject.GetComponent<AIBody>().MaxSpeed;
        directionToTarget = Target - transform.position;

        //TODO: Deaccelerate Agent. Current tries too buggy
        brakeDistance = (3 * MaxSpeed * MaxSpeed) / (2 * MaxAcceleration);

        //Stop Agent
        if (directionToTarget.magnitude < MeleeRange)
        {
            return -body.Velocity/Time.deltaTime;
        }
        return directionToTarget.normalized * MaxAcceleration;
    }
}