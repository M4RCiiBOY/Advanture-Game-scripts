using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSpell : MonoBehaviour
{
    public GameObject Target;
    public float Speed;
    public float WebTime;
    private float webCountdown;
    private Vector3 webPosi;
    private bool targetWebbed;

    void Start()
    {
        targetWebbed = false;
        webCountdown = 0;
    }

    void Update()
    {
        if (webCountdown >= WebTime)
        {
            Destroy(this.gameObject);
            return;
        }
        if (targetWebbed)
        {
            webCountdown += Time.deltaTime;
            Target.transform.position = webPosi;
        }
        else transform.position += (Target.transform.position - transform.position).normalized * Speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject == Target)
            {
                targetWebbed = true;
                webPosi = transform.position;
            }
        }
    }
}
