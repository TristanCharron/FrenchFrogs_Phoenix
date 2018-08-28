using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerFlightControl : MonoBehaviour
{
    public UnityEvent OnWarpAcceleration = new UnityEvent();
    public UnityEvent OnStopWarpAcceleration = new UnityEvent();

    [SerializeField] GameObject core;
    [SerializeField] MouvementSettings moveSettings;
    [SerializeField] BankingSettings blankSettings;

    public float warpFuelConsumption;
    public float screen_clamp = 500; //"Screen Clamp (Pixels)", "Once the pointer is more than this many pixels from the center, the input in that direction(s) will be treated as the maximum value."
    public float roll, yaw, pitch; //Inputs for roll, yaw, and pitch, taken from Unity's input system.

    float distFromVertical;
    float distFromHorizontal;

    Vector2 mousePos = new Vector2(0, 0); //Pointer position from CustomPointer

    float DZ = 0; //Deadzone, taken from CustomPointer.
    float currentMag = 0f;

    bool thrust_exists = true;
    bool roll_exists = true;

    Rigidbody rigidBody;
    Player player;

    public bool IsWarpSpeed { get; protected set; }

	void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

        mousePos = new Vector2(0,0);	
		DZ = CustomPointer.instance.deadzone_radius;
		
		roll = 0; //Setting this equal to 0 here as a failsafe in case the roll axis is not set up.
	}

    void FixedUpdate ()
    {
        UpdateCursorPosition();

        SetPitchYawRoll();

        //Getting the current speed.
        currentMag = rigidBody.velocity.magnitude;
        ApplyTrust();
        ApplyTorque();

        rigidBody.velocity = transform.forward * currentMag;

        if (blankSettings.use_banking)
            UpdateBanking();
    }

    private void ApplyTorque()
    {
        rigidBody.AddRelativeTorque(
            (pitch * moveSettings.turnspeed * Time.deltaTime),
            (yaw * moveSettings.turnspeed * Time.deltaTime),
            (roll * moveSettings.turnspeed * (moveSettings.rollSpeedModifier / 2) * Time.deltaTime));
    }

    private void ApplyTrust()
    {
        if (thrust_exists)
        {
            if (Input.GetKey(KeyCode.LeftShift) && player.Fuel.CurrentFuel > 0)
            {
                currentMag = Mathf.Lerp(currentMag, moveSettings.warpSpeed, moveSettings.thrust_transition_speed * Time.deltaTime);
                if (!IsWarpSpeed)
                    OnWarpAcceleration.Invoke();

                player.Fuel.RemoveFuel(warpFuelConsumption * Time.deltaTime);
                IsWarpSpeed = true;
            }
            else
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    currentMag = Mathf.Lerp(currentMag, moveSettings.upInputSpeed, moveSettings.thrust_transition_speed * Time.deltaTime);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    currentMag = Mathf.Lerp(currentMag, moveSettings.downInputSeed, moveSettings.thrust_transition_speed * Time.deltaTime);
                }
                else
                {
                    currentMag = Mathf.Lerp(currentMag, moveSettings.noInputSpeed, moveSettings.thrust_transition_speed * Time.deltaTime);
                }

                if (IsWarpSpeed)
                    OnStopWarpAcceleration.Invoke();

                IsWarpSpeed = false;
            }
        }
    }

    private void SetPitchYawRoll()
    {
        //Clamping the pitch and yaw values, and taking in the roll input.
        pitch = Mathf.Clamp(distFromVertical, -screen_clamp - DZ, screen_clamp + DZ) * moveSettings.pitchYaw_strength;
        yaw = Mathf.Clamp(distFromHorizontal, -screen_clamp - DZ, screen_clamp + DZ) * moveSettings.pitchYaw_strength;
        if (roll_exists)
            roll = (Input.GetAxis("Horizontal") * -moveSettings.rollSpeedModifier);
    }

    void UpdateCursorPosition()
    {
		mousePos = CustomPointer.pointerPosition;
		
		//Calculate distances from the center of the screen.
		float distV = Vector2.Distance(mousePos, new Vector2(mousePos.x, Screen.height / 2));
		float distH = Vector2.Distance(mousePos, new Vector2(Screen.width / 2, mousePos.y));
		
		//If the distances are less than the deadzone, then we want it to default to 0 so that no movements will occur.
		if (Mathf.Abs(distV) < DZ)
			distV = 0;
		else 
			distV -= DZ; 
			//Subtracting the deadzone from the distance. If we didn't do this, there would be a snap as it tries to go to from 0 to the end of the deadzone, resulting in jerky movement.
			
		if (Mathf.Abs(distH) < DZ)
			distH = 0;	
		else 
			distH -= DZ;
			
		//Clamping distances to the screen bounds.	
		distFromVertical = Mathf.Clamp(distV, 0, (Screen.height));
		distFromHorizontal = Mathf.Clamp(distH,	0, (Screen.width));	
	
		//If the mouse position is to the left, then we want the distance to go negative so it'll move left.
		if (mousePos.x < Screen.width / 2 && distFromHorizontal != 0) {
			distFromHorizontal *= -1;
		}
		//If the mouse position is above the center, then we want the distance to go negative so it'll move upwards.
		if (mousePos.y >= Screen.height / 2 && distFromVertical != 0) {
			distFromVertical *= -1;
		}
	}

	void UpdateBanking()
    {
		Quaternion newRotation = transform.rotation;
		Vector3 newEulerAngles = newRotation.eulerAngles;
		
		//Basically, we're just making it bank a little in the direction that it's turning.
		newEulerAngles.z += Mathf.Clamp((-yaw * moveSettings.turnspeed * Time.deltaTime ) * blankSettings.bank_rotation_multiplier, -blankSettings.bank_angle_clamp, blankSettings.bank_angle_clamp);
		newRotation.eulerAngles = newEulerAngles;
		
		//Apply the rotation to the gameobject that contains the model.
		core.transform.rotation = Quaternion.Slerp(core.transform.rotation, newRotation, blankSettings.bank_rotation_speed * Time.deltaTime);
	}

    [System.Serializable]
    public class MouvementSettings
    {
        public float warpSpeed = 35;
        public float noInputSpeed = 15.0f; 
        public float upInputSpeed = 10f; 
        public float downInputSeed = 2f; 

        public float thrust_transition_speed = 5f; //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
        public float turnspeed = 15.0f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
        public float rollSpeedModifier = 7; //"Roll Speed", "Multiplier for roll speed. Base roll is determined by turn speed"
        public float pitchYaw_strength = 0.5f; //"Pitch/Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"
    }

    [System.Serializable]
    public class BankingSettings
    {
        //"Banking", "Visuals only--has no effect on actual movement"
        public bool use_banking = true; //Will bank during turns. Disable for first-person mode, otherwise should generally be kept on because it looks cool. Your call, though.
        public float bank_angle_clamp = 360; //"Bank Angle Clamp", "Maximum angle the spacecraft can rotate along the Z axis."
        public float bank_rotation_speed = 3f; //"Bank Rotation Speed", "Rotation speed along the Z axis when yaw is applied. Higher values will result in snappier banking."
        public float bank_rotation_multiplier = 1f; //"Bank Rotation Multiplier", "Bank amount along the Z axis when yaw is applied."
    }
}
