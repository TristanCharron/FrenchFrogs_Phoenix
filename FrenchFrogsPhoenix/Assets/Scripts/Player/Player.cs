using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public MouseRotation mouseRotation = new MouseRotation();
    public StickingObjectEvent OnNewStickingObject = new StickingObjectEvent();

    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] Transform nullCore;
    [SerializeField] StickingObject stickingObject;
    [SerializeField] float cameraSensitivity = 2;
    [SerializeField] float rotateSensitivity = 0.01f;

    [SerializeField] float moveSpeed;

    float maxDistanceStickingObject;
    public ObjectStats playerStats;

    public BaseInput input;

    public string ID { private set; get; }

    void Start ()
    {
        playerStats = new ObjectStats();

        if (playerCamera != null)
        playerCamera.player = this;

        OnNewStickingObject.AddListener((newStickingObject) => CalculatePlayerStats(newStickingObject));
        OnNewStickingObject.AddListener((newStickingObject) => playerCamera.CalculateDistanceCamera(newStickingObject));

        stickingObject.SetFirstStickingchild(this);
    }

    public void Spawn(PlayerType type,string ID)
    {
        switch (type)
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

        input.SetActive(true);

        input.LeftStick.AddEvent(Move);
        input.RightStick.AddEvent(RightStickHandle);

        this.ID = ID;
    }

    void RightStickHandle(float x, float y)
    {
        Debug.Log("x = " + x + " / y = " + y);
        //Vector3 cameraTransform = playerCamera.transform.InverseTransformDirection(new Vector3(-y, -x, 0));
        Vector3 cameraTransform = new Vector3(-y, x, 0);

        if (Input.GetMouseButton(1))
            mouseRotation.LookRotation(nullCore.transform, rotateSensitivity, cameraTransform);
        else
            mouseRotation.LookRotation(transform, cameraSensitivity, cameraTransform);
    
    }

    void CalculatePlayerStats(StickingObject newStickingObject)
    {
        playerStats.Reset();
        stickingObject.RecrusiveCalculateStats(playerStats);
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
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}

[System.Serializable]
public class StickingObjectEvent : UnityEvent<StickingObject> {}
