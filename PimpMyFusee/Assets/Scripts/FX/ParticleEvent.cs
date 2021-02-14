using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvent : MonoBehaviour
{
    ParticleSystem myParticleSystem;
    public bool ConstantAudioSource;
    AudioSource myAudiosource;

    // Start is called before the first frame update
    void Awake()
    {
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myAudiosource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    public void CheckPlay()
    {
        if (myParticleSystem && !myParticleSystem.isPlaying)
            myParticleSystem.Play();

        if(myAudiosource && (!ConstantAudioSource || (ConstantAudioSource && !myAudiosource.isPlaying)))
        {
            Debug.Log("Allez");
            myAudiosource.Play();
        }
            
    }

    public void CheckStop()
    {
        if (myParticleSystem && myParticleSystem.isPlaying)
            myParticleSystem.Stop();

        if (myAudiosource && (ConstantAudioSource && myAudiosource.isPlaying))
            myAudiosource.Stop();
    }
}
