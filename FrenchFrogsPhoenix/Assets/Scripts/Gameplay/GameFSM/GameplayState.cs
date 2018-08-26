using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : FSMState {

    protected override void Awake()
    {
        enumID = GameFSMStates.GAMEPLAY;

        EventManager.Subscribe("OnEndGame", () =>
        {
            if(isCurrentState)
                EventManager.Invoke<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, GameFSMStates.GAMEOVER);
        });

       
    }

    protected override void Start()
    {
        
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            EventManager.Invoke("OnEndGame");
        }
        //Debug.Log("Gameplay");
    }

}
