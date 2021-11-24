using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune_Water : MonoBehaviour , IRune
{
    public ParticleSystem WaterParticle;

    public void SpecialDamage()
    {
        //Water Damage
    }

    public void SpecialEffect()
    {
        //Slow Attacks
    }

    //Instantiate and Play the ParticleEffect.
    public void ParticleEffect(Vector3 offset, bool sword)
    {
        ParticleSystem ParticleSystemP = Instantiate(WaterParticle, transform.position + offset, transform.rotation) as ParticleSystem;
        ParticleSystemP.transform.SetParent(this.gameObject.transform);
        ParticleSystemP.Play();
        if(sword)
            Destroy(ParticleSystemP.gameObject, 0.4f);
    }
}
