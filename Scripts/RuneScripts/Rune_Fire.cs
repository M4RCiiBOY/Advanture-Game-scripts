using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Fire : MonoBehaviour, IRune
{
    public ParticleSystem FireParticle;

    public void SpecialDamage()
    {
        //Fire Damage
    }

    public void SpecialEffect()
    {
        //Set on fire
    }

    //Instantiate and Play the ParticleEffect.
    public void ParticleEffect(Vector3 offset, bool sword)
    {
        ParticleSystem ParticleSystemP = Instantiate(FireParticle, transform.position + offset, transform.rotation) as ParticleSystem;
        ParticleSystemP.transform.SetParent(this.gameObject.transform);
        ParticleSystemP.Play();
        if (sword)
            Destroy(ParticleSystemP.gameObject, 0.4f);

    }
}
