using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    private static ParticleController instance;

    public ParticleSystem startGameParticleSystem;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static ParticleController GetInstance()
    {
        return instance;
    }

    public void ActivateStartGameParticles()
    {
        startGameParticleSystem.Play();
    }

    public void DeactivateStartGameParticles()
    {
        startGameParticleSystem.Stop();
    }
}
