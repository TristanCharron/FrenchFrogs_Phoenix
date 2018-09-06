using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour {

    private static UIController instance;

    public Canvas inclinedCanvas;
    public Canvas canvas;

    //Maybe mettre des animations quand tu aim dequoi de different
    [SerializeField] Image mouseCursor;

    [Header("Health")]
    [SerializeField] RadialBar healthBar;

    [Header("Fuel")]
    [SerializeField] RadialBar fuelBar;

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
    void Start()
    {
        instance = this;
        //DeactivateInGameUI();

        ToggleInGameUI(false);
        SubscribeToEvents();

        inclinedCanvas.worldCamera = Camera.main;
    }

    void Update()
    {
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

        //0 for player ID
        EventManager.Subscribe<ObjectStats>(EventConst.GetUpdatePlayerStats(0), (currentStats) => ShowStats(currentStats));
        EventManager.Subscribe<float>(EventConst.GetUpdatePlayerHealth(0), (health) => healthBar.UpdateFill(health));
        EventManager.Subscribe<float>(EventConst.GetUpdatePlayerFuel(0), (fuel) => fuelBar.UpdateFill(fuel));
        EventManager.Subscribe<Vector2>(EventConst.GetUpdateUIPosAim(0), (mPos) => UpdateCursorPosition(mPos));
        EventManager.Subscribe<bool>(EventConst.GetUpdateAimTargetInSight(0), (isInSight) => UpdateTargetInSight(isInSight));
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
        mouseCursor.gameObject.SetActive(true);

        leftUIFader.GlitchIn();
        RightUIFader.GlitchIn();
        startGameFader.FadeOut();
    }

    void DeactivateInGameUI()
    {
        mouseCursor.gameObject.SetActive(false);

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

    void UpdateCursorPosition(Vector3 uiPosition)
    {
        mouseCursor.transform.position = uiPosition;
    }

    void UpdateTargetInSight(bool isInSight)
    {
        if(isInSight)
        {
            mouseCursor.transform.DOScale(.2f, .1f);
        }
        else
        {
            mouseCursor.transform.DOScale(.5f, .1f);
        }
    }
}
