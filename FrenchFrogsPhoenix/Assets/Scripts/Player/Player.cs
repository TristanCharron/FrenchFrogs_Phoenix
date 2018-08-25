using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public MouseRotation mouseRotation = new MouseRotation();
    public StickingObjectEvent OnNewStickingObject = new StickingObjectEvent();

    [SerializeField] Camera cam;
    [SerializeField] StickingObject stickingObject;
    [SerializeField] float cameraSensitivity = 2;
    [SerializeField] float rotateSensitivity = 0.01f;

    [SerializeField] float moveSpeed;

    public ObjectStats playerStats;

	void Start ()
    {
        stickingObject.SetParent(stickingObject, this);
        playerStats = new ObjectStats();

        OnNewStickingObject.AddListener((newStickingObject) => CalculatePlayerStats(newStickingObject));

        CalculatePlayerStats(stickingObject);
    }

    private void Update()
    {
        Move();

        if(Input.GetMouseButton(1))
            mouseRotation.LookRotation(stickingObject.transform, rotateSensitivity);
        else
            mouseRotation.LookRotation(transform, cameraSensitivity);
    }

    public void CalculatePlayerStats(StickingObject newStickingObject)
    {
        playerStats.Reset();
        stickingObject.RecrusiveCalculateStats(playerStats);
    }

    private void Move()
    {
        float upFactor = 0;
        if (Input.GetKey(KeyCode.Q))
            upFactor = 1;
        else if (Input.GetKey(KeyCode.E))
            upFactor = -1;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), upFactor, Input.GetAxis("Vertical"));

        if (input.magnitude > 1)
            input.Normalize();

        Vector3 direction = transform.TransformDirection(input);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}

[System.Serializable]
public class StickingObjectEvent : UnityEvent<StickingObject> {}
