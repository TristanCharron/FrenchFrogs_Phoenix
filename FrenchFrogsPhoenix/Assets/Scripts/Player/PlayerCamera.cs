using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] float paddingY = 4;
    [SerializeField] float offYbyDistance = 1;
    [SerializeField] float offZbyDistance = 1;

    [SerializeField] float paddingZ = 25;
    [SerializeField] float ajustCameraTime = 1;
    [SerializeField] Ease ajustCameraEase;

    public Camera cameraRef;
    public Player player;

    float maxDistanceStickingObject;

    private void Awake()
    {
        if(cameraRef == null)
        {
            cameraRef = GetComponent<Camera>();
        }
    }

    public void CalculateDistanceCamera(StickingObject newStickingObject)
    {
        float newDistance = (newStickingObject.transform.position - player.transform.position).magnitude;
        if (newDistance > maxDistanceStickingObject)
        {
            SetNewDistance(newDistance);
        }
    }

    void SetNewDistance(float newDistance)
    {
        maxDistanceStickingObject = newDistance;

        transform.DOKill();
        Vector3 destination = new Vector3(0, (offYbyDistance * maxDistanceStickingObject) + paddingY, -(maxDistanceStickingObject * offZbyDistance) - paddingZ);
        transform.DOLocalMove(destination, ajustCameraTime).SetEase(ajustCameraEase);
    }
}
