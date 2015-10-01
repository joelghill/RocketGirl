/**
 * Joel Hill 
 * Prototype rotation script
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interpolation;

public class rotate : MonoBehaviour {

	public GameObject level;
	//public double TransitionTime;
	public GameObject[] entities;
	public Camera camera;
	public double cameraMovementY;

	public float[] rotationPoints;

	bool transitionFlag = false;
	int currenttPoint = 0;
	
	// Use this for initialization
	void Start () {
		//initialize dictionaries
		level = this.gameObject;

		rotationPoints = new float[4];
		rotationPoints [0] = level.transform.rotation.y;
		rotationPoints [1] = level.transform.rotation.y+90;
		rotationPoints [2] = level.transform.rotation.y+180;
		rotationPoints [3] = level.transform.rotation.y+270;
	}

	void Update () {
		if (Input.GetMouseButton (0)) {
			Quaternion newRotation 		= Quaternion.AngleAxis(getRotationGoal(), Vector3.up);
			level.transform.rotation	= Quaternion.Slerp(transform.rotation, newRotation, .1f);
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
		return Mathf.Abs ((level.transform.rotation.y - getRotationGoal ())) <= 0.001;
	}
}
