using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public enum AIPlayerStates
{
    PATROL = 0,
    CHASE = 1,
}

public class AIPlayerFSMState : FSMState {

    public Player AIPlayer { private set; get; }
    protected InputAI Input {private set; get;}
    public AIPlayerFSM Owner { private set; get; }

    protected float currentX = 0;
    protected float destX = 0;
    protected float currentY = 0;
    protected float destY = 0;

    protected AIPatrolInputPattern[] AIPatrolPatternsArray;

    protected Transform CachedTransform;

    public override void UpdateState()
    {
        base.UpdateState();
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

        Input = (InputAI)AIPlayer.input;
    }

    public void SetOwner(AIPlayerFSM owner)
    {
        Owner = owner;
    }
}


[RequireComponent(typeof(Collider))]
public class AIPlayerFSM : FiniteStateMachine {

    public const string EVT_ON_CHANGE_AI_STATE = "OnChangeGameState";

    [SerializeField]
    Player player;

    public GameObject ChasedObject { private set; get; }
    public void SetChasedObject(GameObject Object)
    {
        ChasedObject = Object;
    }

    public void StartFSM(Player player)
    {
        this.player = player;
        AIPatrolState patrolState = gameObject.AddComponent<AIPatrolState>();
        AIChaseState chaseState = gameObject.AddComponent<AIChaseState>();
        patrolState.SetPlayer(player);
        patrolState.SetOwner(this);
        chaseState.SetPlayer(player);
        chaseState.SetOwner(this);
        AddFSMState(patrolState);
        AddFSMState(chaseState);
        StartCoroutine(StartFSMCoroutine());   
    }

    IEnumerator StartFSMCoroutine()
    {
        yield return new WaitForEndOfFrame();
        ChangeFSMState(AIPlayerStates.PATROL);
    }
}
