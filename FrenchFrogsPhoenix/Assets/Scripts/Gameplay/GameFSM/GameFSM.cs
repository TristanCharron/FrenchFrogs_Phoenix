using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameFSMStates
{
    MAINMENU = 0,
    GAMEPLAY = 1,
    GAMEOVER = 2,
}

[System.Serializable]
public class GameFSM : FiniteStateMachine
{

    public const string EVT_ON_CHANGE_GAME_STATE = "OnChangeGameState";

    [SerializeField]
    private FSMState[] GameStates;


    protected override void Start()
    {
        for(int i = 0; i < GameStates.Length; i++)
        {
            AddFSMState(GameStates[i]);
        }

        ChangeFSMState(GameFSMStates.MAINMENU);

        EventManager.Subscribe<GameFSMStates>(EVT_ON_CHANGE_GAME_STATE, (state) =>
         {
             ChangeFSMState(state);
         });


    }

 




}


