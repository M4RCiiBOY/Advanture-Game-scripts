using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rg;
    public float speed;
    public float destroyTime;
    public ParticleSystem[] specialInteractionParticles;

    private bool twoRunes = false;
    private Animator anim;
    private IRune script;
    private GameObject firingPlayer;

    void Start ()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	void Update ()
    {
        rg.velocity = -transform.up * speed;
        Destroy(this.gameObject, destroyTime);
    }

    //Checks which Arrow is used and plays the ParticleEffect.
    public void RuneSelect(int ID)
    {
        switch (ID)
        {
            case 1:
                Rune_Fire tmpScript1 = gameObject.GetComponent<Rune_Fire>();
                tmpScript1.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript1;
                break;
            case 2:
                Rune_Water tmpScript2 = gameObject.GetComponent<Rune_Water>();
                tmpScript2.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript2;
                break;
            case 3:
                Rune_Thunder tmpScript3 = gameObject.GetComponent<Rune_Thunder>();
                tmpScript3.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript3;
                break;
            case 4:
                Rune_Steam tmpScript4 = gameObject.GetComponent<Rune_Steam>();
                tmpScript4.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript4;
                break;
            case 5:
                Rune_Explosion tmpScript5 = gameObject.GetComponent<Rune_Explosion>();
                tmpScript5.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript5;
                break;
            case 6:
                Rune_StunLighting tmpScript6 = gameObject.GetComponent<Rune_StunLighting>();
                tmpScript6.ParticleEffect((new Vector3(0, 0, 0)), false);
                script = (IRune)tmpScript6;
                break;
            default:
                break;
        }
    }

    public void AssignPlayer(GameObject obj)
    {
        firingPlayer = obj;
    }

    public void Damage(Collider2D col, int dmg)
    {
        //script.SpecialDamage();
        //script.SpecialEffect();
        if (col.tag == "Player") col.GetComponent<PlayerCtrl>().RecieveDamage(dmg);
        else col.gameObject.GetComponent<AIBody>().Damage(dmg);
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        //excludes the player that shot the arrow
        if (col.tag != firingPlayer.tag)
        {
            StartCoroutine(StopArrow());
            anim.SetBool("ArrowHit", true);
            Destroy(this.gameObject, 1f);
            Debug.Log("hit");
            Damage(col, 1);
        }
    }

    private IEnumerator StopArrow()
    {
        yield return new WaitForSeconds(.03f);
        speed = 0;
    }
}
