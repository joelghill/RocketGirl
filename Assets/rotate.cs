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
	public GameObject player;
	//public double TransitionTime;
	public GameObject[] entities;
	public Camera camera;
	public double cameraMovementY;

	public float[] rotationPoints;

	public bool transitionFlag = false;
	public int currentPoint = 0;
	public int currentGoal;

	// Use this for initialization
	void Start () {

		rotationPoints = new float[4];
		rotationPoints [0] = 0;
		rotationPoints [1] = 90;
		rotationPoints [2] = 180;
		rotationPoints [3] = 270;
	}

	void Update () {
		/*if (Input.GetMouseButtonDown (0)) {
			if (!transitionFlag) {
				transitionFlag = true;
				incrementGoal ();
			}
		}
		if (transitionFlag) {
			if (hasReachedGoal ()) {
				snapToGoal ();
				transitionFlag = false;
			} else {
				Vector3 pos = player.transform.position;
				level.transform.RotateAround (player.transform.position, transform.up, 50 * Time.deltaTime);
				player.transform.RotateAround (player.transform.position, transform.up, -50 * Time.deltaTime);
			}
		}*/
		if (Input.GetMouseButtonDown (0)) {
			if(!transitionFlag){
				transitionFlag = true;
			}
		}
		if (transitionFlag) {
			Vector3 pos = player.transform.position;
			level.transform.RotateAround (player.transform.position, transform.up, 250 * Time.deltaTime);
			player.transform.RotateAround (player.transform.position, transform.up, -250 * Time.deltaTime);

			if(hasReachedGoal()){
				transitionFlag = false;
				snapToGoal();
				incrementCurrentIndex();
			}
		}
	}

	Quaternion CloneQuat(Quaternion q){
		Quaternion n = new Quaternion(q.x, q.y, q.z, q.w);
		return n;
	}

	float getRotationGoal(){
		return rotationPoints [getGoalIndex()];
	}

	bool hasReachedGoal(){
		Debug.Log ("Goal check");
		Debug.Log ("Current Rotation:  " + level.transform.rotation.eulerAngles.y);
		Debug.Log ("Goal Rotation:  " + getRotationGoal().ToString());
		return Mathf.Abs(level.transform.rotation.eulerAngles.y - getRotationGoal ()) < 10;
	}

	void snapToGoal(){
		Debug.Log("Snap to goal called");
		Debug.Log ("Rotation goal is:  " + getRotationGoal ());
		float levelsnap = getRotationGoal () - this.transform.rotation.eulerAngles.y;
		//float playerSnap = (-1*getRotationGoal ()) + this.player.transform.rotation.eulerAngles.y;
		this.transform.RotateAround (player.transform.position, transform.up, levelsnap);
		player.transform.RotateAround(player.transform.position, transform.up, (-1*levelsnap));
		//this.transform.Rotate (new Vector3 (0, 1, 0),level.transform.rotation.y - getRotationGoal(), Space.World);
		//this.player.transform.Rotate(new Vector3 (0, 1, 0),player.transform.rotation.y - (-1*getRotationGoal()), Space.World);
		//this.player.transform.rotation.Set(0, -1*getRotationGoal (), 0, player.transform.rotation.w);
	}
	void incrementCurrentIndex(){
		if (currentPoint == 3) {
			this.currentPoint = 0;
		}else{
			currentPoint ++;
		}
	}

	int getGoalIndex(){
		if (currentPoint == 3) {
			return 0;
		}else{
			return currentPoint + 1;
		}
	}
}
