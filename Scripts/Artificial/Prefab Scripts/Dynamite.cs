using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    //Design publics
    public float AirTime;
    public float FuseTime;
    public float ExplosionRadius;
    public int Damage;
    [Tooltip("Height of Dynamite at the midpoint.")]
    public float Height = 0.3f;

    private float fuse;
    private bool blown;
    private float heightConst;

    public Vector3 Target;
    private Vector3 velocity;

    void Start()
    {
        velocity = (Target - transform.position) / AirTime;
        fuse = 0;
        gameObject.GetComponent<CircleCollider2D>().radius = ExplosionRadius;
        heightConst = Height / (0.5f * AirTime * (0.5f * AirTime - AirTime));
    }

    void Update()
    {
        if (blown)
        {
            return;
        }
        fuse += Time.deltaTime;
        if (fuse >= FuseTime)
        {
            blown = true;
            Destroy(this.gameObject, 0.05f);
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
        if (fuse < AirTime)
        {
            transform.position += velocity * Time.deltaTime;
            gameObject.transform.GetChild(0).position = transform.position + new Vector3(0,
                heightConst * fuse * (fuse - AirTime));
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<PlayerCtrl>() != null)
        {
            col.gameObject.GetComponent<PlayerCtrl>().RecieveDamage(Damage);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(
                (col.transform.position - transform.position).normalized * (ExplosionRadius - (col.transform.position - transform.position).magnitude)
                , ForceMode2D.Force);  //sadly not doing much ...
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
