using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Steam : MonoBehaviour, IRune
{
    public ParticleSystem SteamParticle;

    public void SpecialDamage()
    {

    }

    public void SpecialEffect()
    {
        //Blind enemy
    }

    //Instantiate and Play the ParticleEffect.
    public void ParticleEffect(Vector3 offset, bool sword)
    {
        ParticleSystem ParticleSystemP = Instantiate(SteamParticle, transform.position + offset, transform.rotation) as ParticleSystem;
        ParticleSystemP.transform.SetParent(this.gameObject.transform);
        ParticleSystemP.Play();
        if (sword)
            Destroy(ParticleSystemP.gameObject, 0.4f);
    }
}
