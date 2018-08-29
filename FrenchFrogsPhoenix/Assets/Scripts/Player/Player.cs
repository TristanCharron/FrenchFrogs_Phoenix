using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public const string EVT_ON_PLAYER_DEATH = "OnPlayerDeath";

    public MouseRotation mouseRotation = new MouseRotation();
    public StickingObjectEvent OnNewStickingObject = new StickingObjectEvent();
    public StickingObjectEvent OnDestroyStickingObject = new StickingObjectEvent();

    [SerializeField] WorldPlayerStats worldPlayerStats;

    [SerializeField] Transform nullCore;
    //[SerializeField] StickingObject stickingObject;
    [SerializeField] float cameraSensitivity = 2;
    [SerializeField] float rotateSensitivity = 0.01f;


    //float maxDistanceStickingObject;
    public ObjectStats playerStats;
    public PlayerType currentType;

    public InputBase input;

    public PlayerFlightControl Control { get; protected set; }
    public PlayerFuel Fuel { get; protected set; }
    public PlayerType Type { get; private set; }

    public int ID { private set; get; }

    void Start ()
    {
        Fuel = GetComponent<PlayerFuel>();
        playerStats = new ObjectStats();
        Control = GetComponent<PlayerFlightControl>();

       // OnNewStickingObject.AddListener((newStickingObject) => CalculatePlayerStats());   
       // OnDestroyStickingObject.AddListener((newStickingObject) => DestroyStickingObject(newStickingObject));

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            if(input != null)
                input.SetActive(CurrentState == GameFSMStates.GAMEPLAY);

            if (CurrentState == GameFSMStates.GAMEPLAY)
                Fuel.SetActive(true);
            else
                Fuel.SetActive(false);

        });
    }

    private void Update()
    {
        if(input != null)
            input.Update();
    }

    public void Spawn(PlayerType type,int ID)
    {
        currentType = type;

        switch (currentType)
        {
            case PlayerType.AI:
                //input = new AIInput();
                input = new InputAI();
                AIPlayerFSM fsm = gameObject.AddComponent<AIPlayerFSM>();
                fsm.StartFSM(this);
                break;
            case PlayerType.HUMAN:
                input = new InputPlayer();
                //input = new PlayerInput(0);
                break;
            default:
                break;
        }
        input.Init(ID);

        input.SetActive(false);

        //input.LeftStick.AddEvent(Move);
        //input.RightStick.AddEvent(RightStickHandle);

        this.ID = ID;
        this.Type = type;
    }

    void CalculatePlayerStats()
    {
        playerStats.Reset();
      //  stickingObject.RecrusiveCalculateStats(playerStats);
        EventManager.Invoke<ObjectStats>("UpdatePlayerStats", playerStats);
    }

    //void DestroyStickingObject(StickingObject stickingObject)
    //{
    //    if(this.stickingObject == stickingObject)
    //    {
    //        Debug.Log("I DIE OH NON");
    //        EventManager.Invoke<Player>(EVT_ON_PLAYER_DEATH,this);
    //        gameObject.SetActive(false);
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        Player playerRef = other.GetComponent<Player>();

        if (playerRef != null)
        {
            if (playerRef.currentType != PlayerType.HUMAN)
            {
                if (playerRef.worldPlayerStats != null)
                    playerRef.worldPlayerStats.ShowStats(this);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Player playerRef = other.GetComponent<Player>();
        if (playerRef != null)
        {
            if(playerRef.worldPlayerStats != null)
            {
                playerRef.worldPlayerStats.HideStats();
            }
        }
    }
}

[System.Serializable]
public class StickingObjectEvent : UnityEvent<StickingObject> {}
