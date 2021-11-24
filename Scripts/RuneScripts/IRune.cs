using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public interface IRune
    { 
        void SpecialDamage();
        void SpecialEffect();
        void ParticleEffect(Vector3 offest, bool sword);
    }

