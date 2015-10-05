/**
 * Joel Hill 
 * Prototype rotation script
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interpolation;

public class rotate : MonoBehaviour {

	public GameObject player;
	//public double TransitionTime;
	public GameObject[] entities;
	public Camera camera;
	public double cameraMovementY;

	public float[] rotationPoints;

	bool transitionFlag = false;
	int currenttPoint = 0;
	
	// Use this for initialization
	void Start () {

		rotationPoints = new float[4];
		rotationPoints [0] = player.transform.rotation.y;
		rotationPoints [1] = player.transform.rotation.y+90;
		rotationPoints [2] = player.transform.rotation.y+180;
		rotationPoints [3] = player.transform.rotation.y+270;
	}

	void Update () {
		if (Input.GetMouseButton (0)) {
			Vector3 pos = player.transform.position;
			transform.RotateAround(player.transform.position, transform.up, 30 * Time.deltaTime);
			transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			player.transform.LookAt (new Vector3(transform.position.x, player.transform.position.y, transform.position.z));

			//Quaternion newRotation 		= Quaternion.AngleAxis(getRotationGoal(), Vector3.up);
			//level.transform.rotation	= Quaternion.Slerp(transform.rotation, newRotation, .1f);
		}

	}

	Quaternion CloneQuat(Quaternion q){
		Quaternion n = new Quaternion(q.x, q.y, q.z, q.w);
		return n;
	}

	float getRotationGoal(){
		if (currenttPoint == 3)
			return rotationPoints [0];
		return rotationPoints [currenttPoint];
	}

	bool hasReachedGoal(){
		return Mathf.Abs ((player.transform.rotation.y - getRotationGoal ())) <= 0.001;
	}
}
