using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIPlayerStates
{
    PATROL = 0,
    CHASE = 1,
}

public class AIPlayerFSMState : FSMState {

    public Player AIPlayer { private set; get; }

    public override void UpdateState()
    {
    }

    protected override void Awake()
    {
    }

    protected override void Start()
    {
    }

    public void SetPlayer(Player p)
    {
        AIPlayer = p;
    }
}


[RequireComponent(typeof(Collider))]

public class AIPlayerFSM : FiniteStateMachine {

    public const string EVT_ON_CHANGE_AI_STATE = "OnChangeGameState";

    [SerializeField]
    private AIPlayerFSMState[] AiPlayerStates;

    [SerializeField]
    Player player;


    // Use this for initialization
    protected override void Start () {

        for (int i = 0; i < AiPlayerStates.Length; i++)
        {
            AiPlayerStates[i].SetPlayer(player);
            AddFSMState(AiPlayerStates[i]);
        }

        ChangeFSMState(AIPlayerStates.PATROL);

  
    }



}
