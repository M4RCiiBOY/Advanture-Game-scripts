using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Thunder : MonoBehaviour, IRune
{
    public ParticleSystem ThunderParticle;

    public void SpecialDamage()
    {

    }

    public void SpecialEffect()
    {
        //Lightning Jumps
    }

    //Instantiate and Play the ParticleEffect.
    public void ParticleEffect(Vector3 offset, bool sword)
    {
        ParticleSystem ParticleSystemP = Instantiate(ThunderParticle, transform.position + offset, transform.rotation) as ParticleSystem;
        ParticleSystemP.transform.SetParent(this.gameObject.transform);
        ParticleSystemP.Play();
        if (sword)
            Destroy(ParticleSystemP.gameObject, 0.4f);
    }
}

