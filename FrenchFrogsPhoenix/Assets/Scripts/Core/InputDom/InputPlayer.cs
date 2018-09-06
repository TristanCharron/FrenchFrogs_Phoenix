using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputPlayer : InputBase
{
    Rewired.Player input;

    public override void Init(int id)
    {
        input = Rewired.ReInput.players.GetPlayer(id);
        ID = id;
    }

    public override float GetAxis(int axis)
    {
        if (!isEnabled)
            return 0;

        return input.GetAxisRaw(axis);
    }

    public override bool GetButton(int button)
    {
        if (!isEnabled)
            return false;

        return input.GetButton(button);
    }

    public override bool GetButtonDown(int button)
    {
        if (!isEnabled)
            return false;

        return input.GetButtonDown(button);
    }

    public override bool GetButtonUp(int button)
    {
        if (!isEnabled)
            return false;

        return input.GetButtonUp(button);
    }
}
