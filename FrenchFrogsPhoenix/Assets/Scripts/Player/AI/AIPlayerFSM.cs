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

    public AIPlayerFSM Owner { private set; get; }


    protected float timeElapsed = 0;
    protected float currentX = 0;
    protected float destX = 0;
    protected float currentY = 0;
    protected float destY = 0;

    protected AIPatrolInputPattern[] AIPatrolPatternsArray;

    protected Transform CachedTransform;

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

    public void SetOwner(AIPlayerFSM owner)
    {
        Owner = owner;
    }

 
}


[RequireComponent(typeof(Collider))]

public class AIPlayerFSM : FiniteStateMachine {

    public const string EVT_ON_CHANGE_AI_STATE = "OnChangeGameState";

    [SerializeField]
    private AIPlayerFSMState[] AiPlayerStates;

    [SerializeField]
    Player player;

    public void SetChasedObject(GameObject Object)
    {
        ChasedObject = Object;
    }

    public GameObject ChasedObject { private set; get; }


    // Use this for initialization
    protected override void Start () {

        for (int i = 0; i < AiPlayerStates.Length; i++)
        {
            AiPlayerStates[i].SetPlayer(player);
            AiPlayerStates[i].SetOwner(this);
            AddFSMState(AiPlayerStates[i]);
        }

        ChangeFSMState(AIPlayerStates.PATROL);

  
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }



}
