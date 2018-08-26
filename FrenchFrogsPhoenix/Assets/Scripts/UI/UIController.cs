using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private static UIController instance;

    public Canvas canvas;
    [SerializeField] Text powerTxt;
    [SerializeField] Text speedTxt;
    [SerializeField] Text massTxt;
    [SerializeField] CanvasFader leftUIFader;
    [SerializeField] CanvasFader RightUIFader;
    [SerializeField] CanvasFader startGameFader;

    // Use this for initialization
    void Start() {
        instance = this;
        //DeactivateInGameUI();

        ToggleInGameUI(false);

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleInGameUI(CurrentState == GameFSMStates.GAMEPLAY);
            //ToggleInGameUI(CurrentState == GameFSMStates.MAINMENU);
        });
    }

    // Update is called once per frame
    void Update() {

    }

    public void ActivateStartMenuUI()
    {

    }

    public void DeactivateStartMenuUI()
    {

    }

    public void ToggleInGameUI(bool activate)
    {
        if(activate)
        {
            ActivateInGameUI();
        }
        else
        {
            DeactivateInGameUI();
        }
    }

    public void ActivateInGameUI()
    {
        ParticleController.GetInstance().DeactivateStartGameParticles();
        leftUIFader.GlitchIn();
        RightUIFader.GlitchIn();
        startGameFader.FadeOut();
    }

    public void DeactivateInGameUI()
    {
        ParticleController.GetInstance().ActivateStartGameParticles();
        leftUIFader.Hide();
        RightUIFader.Hide();
        startGameFader.GlitchIn();
    }

    public static UIController GetInstance()
    {
        return instance;
    }

    public void SetPower(float powerValue)
    {
        powerTxt.text = "Power: " + powerValue.ToString();
    }

    public void SetSpeed(float speedValue)
    {
        speedTxt.text = "Speed: " + speedValue.ToString();
    }

    public void SetMass(float massValue)
    {
        massTxt.text = "Mass: " + massValue.ToString();
    }
}
