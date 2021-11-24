using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour, IMovement
{
    Vector3 acceleration;
    float MaxAcceleration;
    AIBody body;
    float timer;
    int rotateAngle;

    private void Awake()
    {
        MaxAcceleration = gameObject.GetComponent<AIBody>().MaxAcceleration;
        body = gameObject.GetComponent<AIBody>();
        acceleration = new Vector3();
        timer = 0.3f;
    }

    public Vector3 Accelerate()
    {
        if (body.Velocity.magnitude == 0)
        {
            return Quaternion.Euler(0, 0, Random.Range(0, 361)) * new Vector3(0, 1, 0) / Time.deltaTime;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.3f;
            rotateAngle = Random.Range(-90, 91);
        }
        acceleration = Quaternion.Euler(0, 0, rotateAngle) * body.Velocity;
        if (acceleration.magnitude > MaxAcceleration) acceleration = acceleration.normalized * MaxAcceleration;
        return acceleration;
    }
}
