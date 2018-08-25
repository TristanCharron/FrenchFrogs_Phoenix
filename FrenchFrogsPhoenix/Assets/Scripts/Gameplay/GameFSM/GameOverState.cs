using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : FSMState {

    protected override void Awake()
    {
        enumID = GameFSMStates.GAMEOVER;
    }

    protected override void Start()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("GameOver");
    }
}
