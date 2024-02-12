using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem.Emit(1);
        //StartCoroutine(EmitParticles());
    }

    IEnumerator EmitParticles()
    {
        float totalDelay = 0.3f;

        for(int i = 0; i < 3 ; i++)
        {
            particleSystem.Emit(1);
            yield return new WaitForSeconds(totalDelay);
            totalDelay += 0.3f;
        }
    }
}
