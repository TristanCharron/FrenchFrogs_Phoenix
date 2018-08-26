using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public const string EVT_ON_PLAYER_DEATH = "OnPlayerDeath";

    public MouseRotation mouseRotation = new MouseRotation();
    public StickingObjectEvent OnNewStickingObject = new StickingObjectEvent();
    public StickingObjectEvent OnDestroyStickingObject = new StickingObjectEvent();

    public UnityEvent OnWarpAcceleration = new UnityEvent();
    public UnityEvent OnWarpStopAcceleration = new UnityEvent();

    [SerializeField] WorldPlayerStats worldPlayerStats;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform nullCore;
    [SerializeField] StickingObject stickingObject;
    [SerializeField] float cameraSensitivity = 2;
    [SerializeField] float rotateSensitivity = 0.01f;

    [SerializeField] float baseMoveSpeed = 5;
    [SerializeField] float baseAcceleration = 2;
    Vector3 currentVelocity = Vector3.zero;

    float maxDistanceStickingObject;
    public ObjectStats playerStats;
    public PlayerType currentType;

    public BaseInput input;

    public PlayerFuel Fuel { get; protected set; }
    public PlayerType Type { get; private set; }

    public string ID { private set; get; }

    void Start ()
    {
        playerStats = new ObjectStats();

        if (playerCamera != null)
            playerCamera.player = this;

        OnNewStickingObject.AddListener((newStickingObject) => CalculatePlayerStats());
        OnNewStickingObject.AddListener((newStickingObject) => playerCamera.CalculateDistanceCamera(newStickingObject));
        OnDestroyStickingObject.AddListener((newStickingObject) => DestroyStickingObject(newStickingObject));

        stickingObject.SetObjectStats(new ObjectStats());
        stickingObject.SetFirstStickingchild(this);
        stickingObject.SetMeshChild(nullCore);
        
        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            if(input != null)
                input.SetActive(CurrentState == GameFSMStates.GAMEPLAY);
        });

        if(currentType == PlayerType.HUMAN)
        {
            UIController.GetInstance().canvas.worldCamera = playerCamera.cameraRef;
        }
    }

    private void Update()
    {
        input.Update();
    }

    public void Spawn(PlayerType type,string ID)
    {
        currentType = type;

        switch (currentType)
        {
            case PlayerType.AI:             
                //Add AI Input
                input = new AIInput();
                break;
            case PlayerType.HUMAN:
                input = new PlayerInput(0);
                break;
            default:
                break;
        }

        input.SetActive(false);

        input.LeftStick.AddEvent(Move);
        input.RightStick.AddEvent(RightStickHandle);

        this.ID = ID;
        this.Type = type;
    }

    void RightStickHandle(float x, float y)
    {
        //Vector3 cameraTransform = playerCamera.transform.InverseTransformDirection(new Vector3(-y, -x, 0));
        Vector3 cameraTransform = new Vector3(-y, x, 0);

        if (Input.GetMouseButton(1))
            mouseRotation.LookRotation(nullCore.transform, rotateSensitivity, cameraTransform);
        else
            mouseRotation.LookRotation(transform, cameraSensitivity, cameraTransform);
    }

    void CalculatePlayerStats()
    {
        playerStats.Reset();
        stickingObject.RecrusiveCalculateStats(playerStats);
        EventManager.Invoke<ObjectStats>("UpdatePlayerStats", playerStats);
    }

    private void Move(float x, float y)
    {
        float upFactor = 0;
        if (Input.GetKey(KeyCode.Q))
            upFactor = 1;
        else if (Input.GetKey(KeyCode.E))
            upFactor = -1;

        Vector3 input = new Vector3(x, upFactor, y);

        if (input.magnitude > 1)
            input.Normalize();

        Vector3 direction = transform.TransformDirection(input);
        if (input.magnitude == 0)
            currentVelocity -= direction * baseAcceleration * Time.deltaTime;
        else
            currentVelocity += direction * baseAcceleration * Time.deltaTime;

        if (currentVelocity.magnitude > baseMoveSpeed)
            currentVelocity = currentVelocity.normalized * baseMoveSpeed;

        transform.position += currentVelocity * Time.deltaTime;
    }

    void WarpAcceleration()
    {
        OnWarpAcceleration.Invoke();
    }

    void StopWarpAcceleration()
    {
        OnWarpStopAcceleration.Invoke();
    }

    void DestroyStickingObject(StickingObject stickingObject)
    {
        if(this.stickingObject == stickingObject)
        {
            Debug.Log("I DIE OH NON");
            EventManager.Invoke<Player>(EVT_ON_PLAYER_DEATH,this);
            gameObject.SetActive(false);
        }
    }

    public PlayerCamera GetPlayerCamera()
    {
        return playerCamera;
    }

    public void OnTriggerEnter(Collider other)
    {
        Player playerRef = other.GetComponent<Player>();
        if (playerRef != null)
        {
            if (playerRef.currentType != PlayerType.HUMAN)
            {
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
