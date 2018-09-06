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
    public const string EVT_ON_ENDGAME = "OnEndGame";

    [SerializeField] private FSMState[] GameStates;

    protected void Awake()
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

        EventManager.Subscribe(PlayerFactory.EVT_ONLOCALPLAYERDEATH, () =>
         {
             if(CurrentFSMState.EnumID.ToString() == GameFSMStates.GAMEPLAY.ToString())
                ChangeFSMState(GameFSMStates.GAMEOVER);
         });


    }
}


