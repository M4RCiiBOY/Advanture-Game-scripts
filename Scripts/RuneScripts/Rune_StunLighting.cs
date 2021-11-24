using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_StunLighting : MonoBehaviour, IRune
{
    public ParticleSystem StunLightingParticle;
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
        ParticleSystem ParticleSystemP = Instantiate(StunLightingParticle, transform.position + offset, transform.rotation) as ParticleSystem;
        ParticleSystemP.transform.SetParent(this.gameObject.transform);
        ParticleSystemP.Play();
        if (sword)
            Destroy(ParticleSystemP.gameObject, 0.4f);
    }
}
