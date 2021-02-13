using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvent : MonoBehaviour
{
    ParticleSystem myParticleSystem;

    // Start is called before the first frame update
    void Awake()
    {
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    public void CheckPlay()
    {
        if (!myParticleSystem.isPlaying)
            myParticleSystem.Play();
    }

    public void CheckStop()
    {
        if (myParticleSystem.isPlaying)
            myParticleSystem.Stop();
    }
}
