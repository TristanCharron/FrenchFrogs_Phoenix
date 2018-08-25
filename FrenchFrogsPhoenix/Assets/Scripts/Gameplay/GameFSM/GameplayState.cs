using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : FSMState {

    protected override void Awake()
    {
        enumID = GameFSMStates.GAMEPLAY;
    }

    protected override void Start()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("Gameplay");
    }

}
