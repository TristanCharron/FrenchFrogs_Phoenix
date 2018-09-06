using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : FSMState {

    InputPlayer input;

    bool isActive = false;


    // Use this for initialization
    protected override void Awake()
    {
        enumID = GameFSMStates.MAINMENU;
        input = new InputPlayer();
        input.Init(0);
        input.SetActive(true);

        EventManager.Subscribe<System.Enum>("OnChangeGameFSM", (nextState) => {
            bool isActive = nextState.ToString() == GameFSMStates.MAINMENU.ToString();
            SetActive(isActive);
        });

        input.SubscribeButtonDown(RewiredConsts.Action.Fire, () =>
        {
            if (isActive)
            {
                Debug.Log("START GAME");
                SetActive(false);
                EventManager.Invoke<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, GameFSMStates.GAMEPLAY);
            }
        });
    }


    protected override void Start()
    {
        SetActive(true);
    }

    private void SetActive(bool active)
    {
        isActive = active;
    }


    public override void UpdateState()
    {
        base.UpdateState();
        input.Update();
    }
}
