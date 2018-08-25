using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyboardButton {

    [SerializeField]
    private KeyCode _Key;

    [SerializeField]
    private string _ActionName;

    public KeyCode Key { get { return _Key; } }

    public string ActionName { get { return _ActionName; } }


    public void SetActionName(string ActionName)
    {
        this._ActionName = ActionName;
    }


}








public class KeyboardInputController : MonoBehaviour
{
    public const string EVT_KEYBOARD_PRESS = "OnKeyboardPress";
    public const string EVT_MOUSE_LEFT_CLICK = "OnMouseLeftClick";
    public const string EVT_MOUSE_MIDDLE_CLICK = "OnMouseMiddleClick";
    public const string EVT_MOUSE_RIGHT_CLICK = "OnMouseRightClick";
    public const string EVT_MOUSE_SCROLL_FORWARD = "OnMouseScrollForward";
    public const string EVT_MOUSE_SCROLL_BACKWARD = "OnMouseScrollBackward";


    [SerializeField]
    List<KeyboardButton> KeyboardButtonsList;

    private Dictionary<string, KeyboardButton> KeyboardButtonsDictionary ;


    private void Awake()
    {
        KeyboardButtonsDictionary = new Dictionary<string, KeyboardButton>();

        for (int i = 0; i < KeyboardButtonsList.Count; i++)
        {
            KeyboardButtonsDictionary.Add(KeyboardButtonsList[i].ActionName, KeyboardButtonsList[i]);
        }

       
        EventManager.Subscribe<string>(EVT_KEYBOARD_PRESS, (actionName) =>
        {
            Debug.Log(actionName);
        });
    }



    private void Update()
    {

        foreach (KeyValuePair<string, KeyboardButton> currentKeyboardButton in KeyboardButtonsDictionary)
        {
            if (Input.GetKeyDown(currentKeyboardButton.Value.Key))
            {
                EventManager.Invoke<string>(EVT_KEYBOARD_PRESS, currentKeyboardButton.Value.ActionName);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            EventManager.Invoke<Vector2>(EVT_MOUSE_LEFT_CLICK, Input.mousePosition);
        }


        if (Input.GetMouseButtonDown(1))
        {
            EventManager.Invoke<Vector2>(EVT_MOUSE_MIDDLE_CLICK, Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(2))
        {
            EventManager.Invoke<Vector2>(EVT_MOUSE_RIGHT_CLICK, Input.mousePosition);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            EventManager.Invoke<Vector2>(EVT_MOUSE_SCROLL_FORWARD, Input.mouseScrollDelta);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            EventManager.Invoke<Vector2>(EVT_MOUSE_SCROLL_BACKWARD, Input.mouseScrollDelta);
        }

    }




}
