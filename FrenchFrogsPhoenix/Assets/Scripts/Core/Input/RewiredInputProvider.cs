using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RewiredInputProvider : MonoBehaviour
{
    [SerializeField]
    private int playerID;

    private Rewired.Player player;

    public const string EVT_INPUT_PRESS_DOWN = "OnInputPressDown";
    public const string EVT_INPUT_PRESS = "OnInputPressDown";

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);

        for(int i = 0; i < ReInput.mapping.Actions.Count; i++)
        {
           player.AddInputEventDelegate(OnButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonLongPressed, ReInput.mapping.Actions[i].name);
           player.AddInputEventDelegate(OnButton, UpdateLoopType.Update, InputActionEventType.Update, ReInput.mapping.Actions[i].name);
        }
    }

    private void OnButtonDown(InputActionEventData obj)
    {
        EventManager.Invoke<InputActionEventData>(EVT_INPUT_PRESS_DOWN, obj);
    }

    private void OnButton(InputActionEventData obj)
    {
        EventManager.Invoke<InputActionEventData>(EVT_INPUT_PRESS, obj);
    }
}
