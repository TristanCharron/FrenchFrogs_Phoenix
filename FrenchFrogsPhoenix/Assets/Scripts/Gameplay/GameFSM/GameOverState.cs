using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : FSMState {

    protected override void Awake()
    {
        enumID = GameFSMStates.GAMEOVER;
    }


    public override void UpdateState()
    {
        base.UpdateState();
        if(TimeElapsed > 1)
        {
            Owner.ChangeFSMState(GameFSMStates.MAINMENU);
        }
    }

    protected override void Start()
    {
        TimeElapsed = 0;
    }
}
