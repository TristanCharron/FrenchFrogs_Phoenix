using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameFSMStates
{
    MAINMENU = 0,
    GAMEPLAY = 1,
    GAMEOVER = 2,
}

[System.Serializable]
public class GameFSM : FiniteStateMachine
{
    [SerializeField]
    private FSMState[] GameStates;


    protected override void Start()
    {
        for(int i = 0; i < GameStates.Length; i++)
        {
            AddFSMState(this.GameStates[i]);
        }

        EventManager.Subscribe<FSMState>("OnChangeGameFSM", (s) =>
        {
            Debug.Log(s);
        });

        ChangeFSMState(GameFSMStates.MAINMENU);


    }

 




}


