using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

    [SerializeField] float mouseSensitivity = 1;
    [SerializeField] Transform debugAim;
    [SerializeField] float aimAheadDistance = 100;
    [SerializeField] float offset;
    public float deadZone = 5;

    Player player;
    InputBase input;

    Vector2 pointerPosition;

    void Start()
    {
        player = GetComponent<Player>();
        input = player.input;

        pointerPosition = new Vector2();
    }


	void Update()
    {
        UpdateLockCursor();
        CalculateAimPosition();
    }

    void UpdateLockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void CalculateAimPosition()
    {
        float x_axis = input.GetAxis(RewiredConsts.Action.CameraHorizontal);
        float y_axis = input.GetAxis(RewiredConsts.Action.CameraVertical);

        pointerPosition += new Vector2(
                            x_axis * mouseSensitivity,
                            y_axis * mouseSensitivity);

        pointerPosition.x = Mathf.Clamp(pointerPosition.x, -Screen.width / 2, Screen.width / 2);
        pointerPosition.y = Mathf.Clamp(pointerPosition.y, -Screen.height / 2, Screen.height / 2);

        Transform cameraTr = Camera.main.transform;
        Vector3 forwardPosition = transform.position + cameraTr.forward * aimAheadDistance;
        Vector3 xyPosition = cameraTr.up * pointerPosition.y * offset + cameraTr.right * pointerPosition.x * offset;
        Vector3 worldPosition = forwardPosition + xyPosition;

        Vector2 uiPosition = Camera.main.WorldToScreenPoint(worldPosition);

        EventManager.Invoke<Vector3>(EventConst.GetUpdateWorldPosAim(player.ID), worldPosition);
        EventManager.Invoke<Vector2>(EventConst.GetUpdateUIPosAim(player.ID), uiPosition);

        if (debugAim != null)
        {
            debugAim.position = worldPosition;
        }
    }
}
