using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour {

    private static UIController instance;

    public Canvas canvas;
    public Image fuelFillImgBg;
    public Image fuelFillImg;
    [SerializeField] Text powerTxt;
    [SerializeField] Text energyTxt;
    [SerializeField] Text massTxt;
    [SerializeField] CanvasFader leftUIFader;
    [SerializeField] CanvasFader RightUIFader;
    [SerializeField] CanvasFader startGameFader;

    // Use this for initialization
    void Start() {
        instance = this;
        //DeactivateInGameUI();

        ToggleInGameUI(false);
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update() {

    }

    void SubscribeToEvents()
    {
        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleInGameUI(CurrentState == GameFSMStates.MAINMENU);
        });

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleInGameUI(CurrentState == GameFSMStates.GAMEOVER);
        });

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            ToggleInGameUI(CurrentState != GameFSMStates.MAINMENU || CurrentState != GameFSMStates.GAMEOVER);
        });

        EventManager.Subscribe<ObjectStats>("UpdatePlayerStats", (CurrentState) =>
        {
            ReceiveData(CurrentState.power,CurrentState.energy,CurrentState.mass);
        });
    }

    public void ActivateStartMenuUI()
    {

    }

    public void DeactivateStartMenuUI()
    {

    }

    public void ToggleInGameUI(bool activate)
    {
        if (activate)
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

    public void ReceiveData(float power,float speed, float mass)
    {
        SetPower(power);
        SetEnergy(speed);
        SetMass(mass);
    }

    public void SetPower(float powerValue)
    {
        powerTxt.text = powerValue.ToString();
    }

    public void SetEnergy(float energyValue)
    {
        energyTxt.text = energyValue.ToString();
    }

    public void SetMass(float massValue)
    {
        massTxt.text =  massValue.ToString();
    }

    public void UpdateFuel(float fuelValue)
    {
        fuelFillImgBg.fillAmount = fuelValue;
        DOTween.Kill(fuelFillImg);
        fuelFillImg.DOFillAmount(fuelValue,0.5f).SetEase(Ease.InQuint);
    }
}
