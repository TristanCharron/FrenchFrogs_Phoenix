﻿using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class CustomPointer : MonoBehaviour {

    CameraFlightFollow camera;
    InputBase input;
	
	public bool use_mouse_input = false; //Pointer will be controlled by the mouse.
	public bool use_gamepad_input = false; //Pointer will be controlled by a joystick
	//public bool use_accelerometer_input = false;	//Pointer will be controlled by accelerometer
	public bool pointer_returns_to_center = false; //Pointer will drift to the center of the screen (Use this for joysticks)
	public bool instant_snapping = false; //If the pointer returns to the center, this will make it return to the center instantly when input is idle. Only works for joysticks
	public float center_speed = 5f; //How fast the pointer returns to the center.

	public bool center_lock = false; //Pointer graphic will be locked to the center. Also affects shooting raycast (always shoots to the center of the screen)

	public bool invert_y_axis = false; //Inverts the y axis.
	

	public float deadzone_radius = 0f; //Deadzone in the center of the screen where the pointer can move without affecting the ship's movement.

	public float thumbstick_speed_modifier = 1f; //Speed multiplier for joysticks.
	public float mouse_sensitivity_modifier = 15f; //Speed multiplier for the mouse.
	
	public static Vector2 pointerPosition; //Position of the pointer in screen coordinates.
	
	[HideInInspector]
	public Rect deadzone_rect; //Rect representation of the deadzone.
	
	public static CustomPointer instance; //The instance of this class (Should only be one)
	// Use this for initialization
	
	void Awake() {	
		pointerPosition = new Vector2 (Screen.width / 2, Screen.height / 2); //Set pointer position to center of screen
		instance = this;
        camera = GetComponent<CameraFlightFollow>();
    }
	
	void Start () {
	
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

        input = camera.player.input;


        //Screen.lockCursor = true;
        deadzone_rect = new Rect((Screen.width / 2) - (deadzone_radius), (Screen.height / 2) - (deadzone_radius), deadzone_radius * 2, deadzone_radius * 2);
	}
	
	// Update is called once per frame
	void Update () {

        if(input == null)
        {
            Debug.Log("still bug");
            input = camera.player.input;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
           // Screen.lockCursor = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
           // Screen.lockCursor = false;
        }


        if (use_mouse_input)
        {

            float x_axis = input.GetAxis(RewiredConsts.Action.CameraHorizontal); 
            float y_axis = input.GetAxis(RewiredConsts.Action.CameraVertical);
		
            if (invert_y_axis)
	            y_axis = -y_axis;
		
            //Add the input to the pointer's position
            pointerPosition += new Vector2(x_axis * mouse_sensitivity_modifier,
			                            y_axis * mouse_sensitivity_modifier);
	
		}
        else if (use_gamepad_input)
        {
            //float x_axis = Input.GetAxis("Horizontal");
            //float y_axis = Input.GetAxis("Vertical");

            float x_axis = input.GetAxis(RewiredConsts.Action.CameraHorizontal);
            float y_axis = input.GetAxis(RewiredConsts.Action.CameraVertical);

            if (invert_y_axis)
				y_axis = -y_axis;
			
		
			pointerPosition += new Vector2(x_axis * thumbstick_speed_modifier * x_axis * x_axis,
				                           y_axis * thumbstick_speed_modifier * y_axis * y_axis);

		}
		//If the pointer returns to the center of the screen and it's not in the deadzone...
		if (pointer_returns_to_center && !deadzone_rect.Contains(pointerPosition)) {
			//If there's no input and instant snapping is on...
			if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && instant_snapping) {
				pointerPosition = new Vector2 (Screen.width / 2, Screen.height / 2); //Place pointer at the center.
			
			
			} else {
				//Move pointer to the center (Will stop when it hits the deadzone)
				pointerPosition.x = Mathf.Lerp (pointerPosition.x, Screen.width / 2, center_speed * Time.deltaTime);
				pointerPosition.y = Mathf.Lerp (pointerPosition.y, Screen.height / 2, center_speed * Time.deltaTime);
			}
		}
		
		//Keep the pointer within the bounds of the screen.
		pointerPosition.x = Mathf.Clamp (pointerPosition.x, 0, Screen.width);
		pointerPosition.y = Mathf.Clamp (pointerPosition.y, 0, Screen.height);


        Vector2 centeredMousePosition = (pointerPosition * 2) - new Vector2(Screen.width, Screen.height);
        EventManager.Invoke<Vector2>("UpdateMousePosition", pointerPosition);
        EventManager.Invoke<Vector2>("UpdateCenterMousePosition", centeredMousePosition);
    }



    //void OnGUI() {
    //       //Draw the pointer texture.
    //       if (pointerTexture != null && !center_lock)
    //       {
    //           GUI.DrawTexture(new Rect(pointerPosition.x - (pointerTexture.width / 2), Screen.height - pointerPosition.y - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);
    //       }
    //       else
    //       {
    //           GUI.DrawTexture(new Rect((Screen.width / 2f) - (pointerTexture.width / 2), (Screen.height / 2f) - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);

    //       }
    //}

}
