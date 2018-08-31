using UnityEngine;
using System.Collections;


[System.Serializable]
public class CameraFlightFollow : MonoBehaviour
{
    public Player player { get; protected set; }

    [SerializeField] public PlayerFlightControl control; 
	
	[SerializeField] float follow_distance = 3.0f;
	[SerializeField] float camera_elevation = 3.0f;
	[SerializeField] float follow_tightness = 5.0f;

	[SerializeField] float rotation_tightness = 10.0f;
	[SerializeField] float afterburner_Shake_Amount = 2f; 
    [SerializeField] float yawMultiplier = 0.005f;
    [SerializeField] float upOffset = 5;
    [SerializeField] bool shake_on_afterburn = true; 

    public float GetFollowDistance()
    {
        return follow_distance;
    }

    public void SetPlayerFlightControl(PlayerFlightControl control)
    {
        this.control = control;
        player = control.GetComponent<Player>();
    }

    void FixedUpdate()
    {
        //Calculate where we want the camera to be.
        Transform target = control.transform;

        Vector3 newPosition = target.TransformPoint(control.yaw * yawMultiplier, camera_elevation, -follow_distance);

        Vector3 positionDifference = (target.position - transform.position) + target.up * upOffset;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * follow_tightness);

        Quaternion newRotation;
        if (control.IsWarpSpeed && shake_on_afterburn)
        {
            //Shake the camera while looking towards the targeter.
            newRotation = Quaternion.LookRotation(positionDifference + new Vector3(
                Random.Range(-afterburner_Shake_Amount, afterburner_Shake_Amount),
                Random.Range(-afterburner_Shake_Amount, afterburner_Shake_Amount),
                Random.Range(-afterburner_Shake_Amount, afterburner_Shake_Amount)),
                target.up);
        }
        else
        {
            newRotation = Quaternion.LookRotation(positionDifference, target.up);
        }
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * rotation_tightness);
	}
}