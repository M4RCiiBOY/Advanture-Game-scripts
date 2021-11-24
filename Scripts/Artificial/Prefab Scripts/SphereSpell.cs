using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpell : MonoBehaviour
{
    public GameObject Target;
    public int Damage;
    public float Speed;

    void Update()
    {
        transform.position += (Target.transform.position - transform.position).normalized * Speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            if (col.gameObject.GetComponent<PlayerCtrl>().gadgetType != PlayerCtrl.GadgetType.Shield)
                col.gameObject.GetComponent<PlayerCtrl>().RecieveDamage(Damage);
            Destroy(this.gameObject);
        }
    }
}
