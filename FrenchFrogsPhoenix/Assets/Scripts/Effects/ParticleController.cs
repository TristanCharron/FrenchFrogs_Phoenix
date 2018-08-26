using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public static ParticleController instance;
    public ParticleSystem startGameParticleSystem;

	// Use this for initialization
	void Awake () {
        instance = this;
        ToggleStartGameParticles(true);
        SubscribeToEvents();
    }

    public static ParticleController GetInstance()
    {
        return instance;
    }

    void SubscribeToEvents()
    {
        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleStartGameParticles(CurrentState == GameFSMStates.MAINMENU);
        });

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleStartGameParticles(CurrentState == GameFSMStates.GAMEOVER);
        });
    }

    public void ToggleStartGameParticles(bool activate)
    {
        if(activate)
        {
            ActivateStartGameParticles();
        }
        else
        {
            DeactivateStartGameParticles();
        }
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
