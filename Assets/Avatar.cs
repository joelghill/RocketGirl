﻿using UnityEngine;
using System.Collections;

public class Avatar : MonoBehaviour {

	protected Rigidbody body;
	protected Animator anim;
	
	public float ySpeed = 8;
	public float xSpeed = 6;
	public float jumpDuration = 0.4f;
	
	//private float distToGround;
	protected int grounded;
	protected float jumping;
    protected bool jumpPressed;
	
	// Use this for initialization
	void Start () {
        //distToGround = transform.lossyScale.y;
        //jumping = 0;
        //grounded = 0;
        //body = GetComponent<Rigidbody> ();
        //anim = GetComponent<Animator> ();
        jumpPressed = false;
		
	}
	
	/*
	 * Adds one to grounded; to be called with the send message function
	 */ 
	void addGrounded(){
		grounded = grounded + 1;
	}
	
	/*
	 * Subtracts one to grounded; to be called with the send message function
	 */
	void minusGrounded(){
		grounded = grounded - 1;
	}
	
	/*
	 * Sets the jumping animation boolean to true.
	 */ 
	void setJumpingAnim(){
		anim.SetBool("jumping",true);
	}

	/*
	 * Object movement script. Speed is base of the set xSpeed, axis is the direction/scale 
	 * (negative axis means reverse movement, 1.0f will give full xSpeed)
	 */ 
	public void move (float axis){

		/*
		 * Checks whether the axis is positive or negative, sets the speed of the character. 
		 * Checks if the y rotation is correct, sets correct rotation with quaternion euler 
		 */
		if (axis > 0.1f) {
			
			body.velocity = new Vector3 (-axis * xSpeed, body.velocity.y,0);
            anim.SetBool("Running", true);

			anim.SetFloat ("runSpeed", axis);
			//transform.rotation.y = 180;
			if(transform.rotation.y == 0){
				transform.rotation = Quaternion.Euler(0, 180, 0);
				//Quaternion.Euler(0, 180, 0);
			}
			
		}else if (axis < -0.1f) {
			body.velocity = new Vector3 (-axis * xSpeed, body.velocity.y,0);
			
			anim.SetFloat("runSpeed", axis);
            anim.SetBool("Running", true);

            if (transform.rotation.y == 1){
				transform.rotation = Quaternion.Euler(0, 0, 0);
				//Quaternion.Euler(0, 180, 0);
			}
			
        } 
		/*
		 * If there is no input, sets the x velocity to 0, keeps y velocity the same
		 * Animation Running is set to false, no change in rotation
		 */ 
		else{
			body.velocity = new Vector3 (0, body.velocity.y, 0);
			anim.SetBool("Running", false);
		}
	}

	public void jump(bool jump,bool doneJump){
		
		/*
		 * Checks if the character is grounded. If yes, and the jump button is pressed, set animations accordingly
		 * and set the y velocity upwards.
		 */ 
		if (grounded>0 && jump) {
			
			//print (grounded.ToString());
			//anim.SetBool("Running", false);
			jumping = 0;
			body.velocity = new Vector3 (body.velocity.x, ySpeed, 0);
			anim.SetBool("jumping",true);
            jumpPressed = true;
		} 
		
		/*
		 * Waits until the jump duration is expended or until the jump button is released to
		 * set the y velocity to negative.
		 */ 
		if ((jumping >= jumpDuration || doneJump)) {
			
			//anim.SetBool("Running", false);
			//grounded = 0;
			jumping = 0;
			body.velocity = new Vector3 (body.velocity.x, -ySpeed, 0);
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", true);
            jumpPressed = false;
			
		}
		
		/*
		 * If grounded and the jump animation is still happening, that needs to stop
		 */ 
		if(grounded>0){
			
			anim.SetBool("Falling",false);
			anim.SetBool("Landing",true);
			
		}

        jumping += Time.deltaTime;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
