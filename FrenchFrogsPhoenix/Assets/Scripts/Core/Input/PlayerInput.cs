using Rewired;


public class PlayerInput : BaseInput
{
    int ID = 0;

    public override void Update()
    {

    }

    public PlayerInput(int ID) : base()
    {



        EventManager.Subscribe<InputActionEventData>(RewiredInputProvider.EVT_INPUT_PRESS_DOWN, (input) =>
        {


            if (isActive)
            {
                switch (input.actionId)
                {
                    case RewiredConsts.Action.Fire:
                        FireButton.Press();
                        break;
                }

            }

        });
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
