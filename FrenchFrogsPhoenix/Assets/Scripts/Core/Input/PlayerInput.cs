using Rewired;
using UnityEngine;


public class PlayerInput : BaseInput
{
    int ID = 0;

    public Rewired.Player player { protected set; get; }

    public override void Update()
    {
        if (!isActive)
            return;

        if(player.GetButton("Fire"))
            FireButton.Press();

 
    }

    public PlayerInput(int ID) : base()
    {

        player = ReInput.players.GetPlayer(ID);

       
        EventManager.Subscribe<InputActionEventData>(RewiredInputProvider.EVT_INPUT_PRESS, (input) =>
        {
            if (isActive)
            {
                switch (input.actionId)
                {
                    case RewiredConsts.Action.MoveHorizontal:
                        LeftStick.SetX(input.GetAxis());
                        LeftStick.Press();
                        break;
                    case RewiredConsts.Action.MoveVertical:
                        LeftStick.SetY(input.GetAxis());
                        LeftStick.Press();
                        break;
                    case RewiredConsts.Action.CameraHorizontal:
                        RightStick.SetX(input.GetAxis());
                        RightStick.Press();
                        break;
                    case RewiredConsts.Action.CameraVertical:
                        RightStick.SetY(input.GetAxis());
                        RightStick.Press();
                        break;
                }

            }

        });
    }




}
