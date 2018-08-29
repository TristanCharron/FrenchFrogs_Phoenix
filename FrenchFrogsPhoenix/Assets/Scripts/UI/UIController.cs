using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour {

    private static UIController instance;

    public Canvas canvas;

    //Maybe mettre des animations quand tu aim dequoi de different
    [SerializeField] Image mouseCursor;

    [Header("Fuel")]
    [SerializeField] Image fuelFillImgBg;
    [SerializeField] Image fuelFillImg;
    [SerializeField] Text energyRatioText;

    [Header("Stats")]
    [SerializeField] Text powerTxt;
    [SerializeField] Text energyTxt;
    [SerializeField] Text massTxt;

    [Header("Canvas Fader")]
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

        EventManager.Subscribe<ObjectStats>("UpdatePlayerStats", (currentStats) => ShowStats(currentStats));
        EventManager.Subscribe<float>("UpdatePlayerFuel", (fuel) => UpdateFuel(fuel));
        EventManager.Subscribe<Vector2>("UpdateCenterMousePosition", (mPos) => UpdateMousePosition(mPos));
        
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

    void ActivateInGameUI()
    {
        leftUIFader.GlitchIn();
        RightUIFader.GlitchIn();
        startGameFader.FadeOut();
    }

    void DeactivateInGameUI()
    {
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

    void UpdateMousePosition(Vector2 newPosition)
    {
        mouseCursor.transform.localPosition = newPosition;// (newPosition*2) - new Vector2(Screen.width, Screen.height);
    }

    void UpdateFuel(float fuelValue)
    {
        //Si le fuel monte automatiquement, on met pas de delay avec le background
        if (fuelValue > fuelFillImg.fillAmount)
            fuelFillImgBg.fillAmount = fuelValue;
        else
            timerBeforeUpdateFuel = 0;

        fuelFillImg.fillAmount = fuelValue;
        UpdateEnergyRatioText(fuelValue);
    }

    void UpdateEnergyRatioText(float fuelValue)
    {
        int roundValue = Mathf.RoundToInt(fuelValue * 100);
    
        string stringValue = roundValue.ToString() + "%";
        if (roundValue < 100)
            stringValue = "0" + stringValue;
        if (roundValue < 10)
            stringValue = "0" + stringValue;

        energyRatioText.text = stringValue;
    }
}
