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

    float delayBeforeUpdateFuel = .5f;
    float timerBeforeUpdateFuel = 0;


    // Use this for initialization
    void Start() {
        instance = this;
        //DeactivateInGameUI();

        ToggleInGameUI(false);
        SubscribeToEvents();

        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFuelDelay();
    }

    void UpdateFuelDelay()
    {
        timerBeforeUpdateFuel += Time.deltaTime;
        if (timerBeforeUpdateFuel > delayBeforeUpdateFuel)
        {
            fuelFillImgBg.DOKill();
            fuelFillImgBg.DOFillAmount(fuelFillImg.fillAmount, 0.3f).SetEase(Ease.InQuint);

            timerBeforeUpdateFuel = 0;
        }
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

        EventManager.Subscribe<ObjectStats>("UpdatePlayerStats", (currentStats) =>
        {
            ShowStats(currentStats);
        });

        EventManager.Subscribe<float>("UpdatePlayerFuel", (fuel) =>
        {
            UpdateFuel(fuel);
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
       // ParticleController.GetInstance().DeactivateStartGameParticles();
        leftUIFader.GlitchIn();
        RightUIFader.GlitchIn();
        startGameFader.FadeOut();
    }

    public void DeactivateInGameUI()
    {
       // ParticleController.GetInstance().ActivateStartGameParticles();
        leftUIFader.Hide();
        RightUIFader.Hide();
        startGameFader.GlitchIn();
    }

    public static UIController GetInstance()
    {
        return instance;
    }

    public void ShowStats(ObjectStats stats)
    {
        powerTxt.text = Mathf.RoundToInt(stats.power).ToString();
        energyTxt.text = Mathf.RoundToInt(stats.energy).ToString();
        massTxt.text = Mathf.RoundToInt(stats.mass).ToString();
    }


    public void UpdateFuel(float fuelValue)
    {
        timerBeforeUpdateFuel = 0;
        fuelFillImg.fillAmount = fuelValue;
    }
}
