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
    public InputBase input { get; protected set; }

    public CameraFlightFollow CameraFlight { get; set; }
    public HitScanner hitScanner { get; set; }

    public HealthComponent Health { get; protected set; }
    public PlayerFlightControl Control { get; protected set; }
    public PlayerFuel Fuel { get; protected set; }
    public PlayerType Type { get; private set; }

    public int ID { private set; get; }

    void Start ()
    {
        Fuel = GetComponent<PlayerFuel>();
        playerStats = new ObjectStats();
        Control = GetComponent<PlayerFlightControl>();
        Health = GetComponent<HealthComponent>();

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

    public void InitializePlayer(PlayerType type, int ID, InputBase input)
    {
        this.input = input;
        this.ID = ID;
        Type = type;
    }

    void CalculatePlayerStats()
    {
        playerStats.Reset();
      //  stickingObject.RecrusiveCalculateStats(playerStats);
        EventManager.Invoke<ObjectStats>(EventConst.GetUpdatePlayerStats(ID), playerStats);
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

    public void OnDamage(float healthRatio)
    {
        Debug.Log(healthRatio);
        EventManager.Invoke<float>(EventConst.GetUpdatePlayerHealth(ID), healthRatio);
    }

    public void OnDestroy()
    {
        EventManager.Invoke<Player>(EVT_ON_PLAYER_DEATH, this);
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        Player playerRef = other.GetComponent<Player>();

        if (playerRef != null)
        {
            if (playerRef.Type != PlayerType.HUMAN)
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
