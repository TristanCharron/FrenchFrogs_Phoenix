using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIPlayerFSMState
{

    public override void UpdateState()
    {
        Debug.Log("CHASE");
    }

    protected override void Awake()
    {
        enumID = AIPlayerStates.CHASE;
    }

    protected override void Start()
    {

    }

}
