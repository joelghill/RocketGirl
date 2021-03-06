﻿/**
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

    public float rotationSpeed = 140f;

	private float[] rotationPoints;

	private bool transitionFlag = false;
	private int currentPoint = 0;
	private int currentGoal;
    private int direction;

    private float lastAngle;

	private Player playerComponent;

	// Use this for initialization
	void Start () {

		rotationPoints = new float[4];
		rotationPoints [0] = 0;
		rotationPoints [1] = 90;
		rotationPoints [2] = 180;
		rotationPoints [3] = 270;
		playerComponent = player.GetComponent<Player> ();

	}

	void Update () {

		//if (Input.GetKeyDown(KeyCode.B)) {
		if (Input.GetButtonDown("Fire1")) {
			if(!transitionFlag && playerComponent.isGrounded())
            {
				transitionFlag = true;
                this.direction = -1;
                player.SendMessage("onPause");
			}
		}

		if (Input.GetButtonDown("Fire2") && playerComponent.isGrounded())
        {
            if (!transitionFlag)
            {
                transitionFlag = true;
                this.direction = 1;
                player.SendMessage("onPause");
            }
        }

        if (transitionFlag) {
            rotateLevel(this.direction);
		}
	}

    /// <summary>
    /// Rotates the "Level object" to it's next roational point (0, 90, 180 or 270 degrees) in the direction specified.
    /// </summary>
    /// <param name="dir">Direction of rotation. 1 or 0 are only valid paramters.</param>
    public void rotateLevel(int dir)
    {
        if (dir != -1 && dir != 1) return;


        //save current angle before any changes...
        lastAngle = level.transform.rotation.eulerAngles.y;

        Vector3 pos = player.transform.position;
        //Debug.Log("Rotating around point:  " + pos.ToString());
        level.transform.RotateAround(pos, transform.up, rotationSpeed * Time.deltaTime * dir);
        player.transform.RotateAround(pos, transform.up, -1 * rotationSpeed * Time.deltaTime * dir);

        if (hasReachedGoal(dir))
        {
            transitionFlag = false;
            snapToGoal(dir);
            incrementCurrentIndex(dir);
            player.SendMessage("onResume");
            return;
        }
    }

    /// <summary>
    /// Returns the next rotational goal in the direction specified.
    /// </summary>
    /// <param name="dir"> Can be 1 or 0</param>
    /// <returns> Next rotational goal in the direction specified.</returns>
	float getRotationGoal(int dir){
		float goal =  rotationPoints [getGoalIndex(dir)];
		return goal;
	}

    /// <summary>
    /// If the current 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns> True if close to goal rotatoin, false otherise</returns>
	bool hasReachedGoal(int direction){
        //Debug.Log ("Goal check");
        //Debug.Log ("Current Rotation:  " + level.transform.rotation.eulerAngles.y);
        //Debug.Log ("Goal Rotation:  " + getRotationGoal(direction).ToString());
        float deltaAngle = Mathf.Abs(lastAngle - level.transform.rotation.eulerAngles.y);
        float minSnap = 5.00f;
        float snapshot = Mathf.Abs(level.transform.rotation.eulerAngles.y - getRotationGoal(direction));
        
		return snapshot < deltaAngle && snapshot < minSnap || hasGonePastGoal();
	}

    /// <summary>
    /// Takes current object rotation and sets it to goal rotation.
    /// </summary>
    /// <param name="direction">Direction of rotation. 1 or 0</param>
	void snapToGoal(int direction){
		//Debug.Log("Snap to goal called");
		//Debug.Log ("Rotation goal is:  " + getRotationGoal ());
		float levelsnap = getRotationGoal (direction) - this.transform.rotation.eulerAngles.y;
		this.transform.RotateAround (player.transform.position, transform.up, levelsnap);
		player.transform.RotateAround(player.transform.position, transform.up, (-1*levelsnap));
	}


	void incrementCurrentIndex(int direction){
        currentPoint = currentPoint + direction;
        currentPoint = rollPoint(currentPoint);
	}

	int getGoalIndex(int dir){
        int point = currentPoint + dir;
        return rollPoint(point);
	}

    /// <summary>
    /// If the number is outside of the array bounds, rolls over to start or end of list.
    /// </summary>
    /// <param name="point">The point to check if out of bounds</param>
    /// <returns>Index of begining or end of rotational points list</returns>
    int rollPoint(int point)
    {
        if (point < 0) return 3;
        if (point >= 4) return 0;

        return point;
    }

	private bool hasGonePastGoal(){
		float goal = getRotationGoal(direction);

		// 0 is a special case
		if(goal == 0){
			//if counting down from 90...
			if(direction < 0){
				return lastAngle < 0 || lastAngle > 355;
			}
			//if going up from 270
			else{
				return lastAngle < 260;
			}
		//if goal is not rolling over 
		}else{
			//if counting down
			if(direction < 0){
				return lastAngle < goal && lastAngle > (goal - 10);
			}
			//if counting up
			else{
				return lastAngle > goal;
			}
		}
		return false;
	}
}
