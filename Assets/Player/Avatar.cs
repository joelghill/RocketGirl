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

    void stayGrounded(){
        if(grounded == 0) {
            grounded = grounded + 1;
        }
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

	public bool collideLeft(){
		
		Vector3 pos = this.transform.position;
		
		Vector3 rayLeft = new Vector3(pos.x - 0.5f, pos.y, pos.z-1000);
		RaycastHit hit;
		
		bool left = Physics.Raycast(rayLeft, Camera.main.transform.forward, out hit);
		
		
		if (left) {
			
			transform.position = new Vector3 (pos.x, pos.y, hit.collider.gameObject.transform.position.z);
			return true;
		}
		else {
			
			return false;
		}
		
	}

	public bool collideRight(){

		Vector3 pos = this.transform.position;
		
		Vector3 rayRight = new Vector3(pos.x + 0.5f, pos.y, pos.z-1000);
		Vector3 rayRightUp = new Vector3(pos.x + 0.5f, pos.y+0.7f, pos.z-1000);
		Vector3 rayRightDown = new Vector3(pos.x + 0.5f, pos.y-0.7f, pos.z-1000);

		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;
		
		bool right = Physics.Raycast(rayRight, Camera.main.transform.forward, out hit1);
		bool rightUp = Physics.Raycast(rayRightUp, Camera.main.transform.forward, out hit2);
		bool rightDown = Physics.Raycast(rayRightDown, Camera.main.transform.forward, out hit3);
		
		
		if (right) {
			
			transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return true;
		} else if (rightUp) {
			
			transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return true;
			
		} else if (rightDown) {
			
			transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return true;
			
		} else {
			
			return false;
			
		}

	}

	/*
	 * Sends raycast bellow the character. If it hits an object, the character is grounded
	 */
    public bool isGrounded()
    {
        Vector3 pos = this.transform.position;

        Vector3 rayBottom = new Vector3(pos.x, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomLeft = new Vector3(pos.x-0.5f, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
		Vector3 rayBottomRight = new Vector3(pos.x+0.5f, pos.y - (transform.lossyScale.y)/5, pos.z-1000);
        
		RaycastHit hit1;
		RaycastHit hit2;
		RaycastHit hit3;

        bool downCenter = Physics.Raycast(rayBottom, Camera.main.transform.forward, out hit1);
		bool downLeft = Physics.Raycast(rayBottomLeft, Camera.main.transform.forward, out hit2);
		bool downRight = Physics.Raycast(rayBottomRight, Camera.main.transform.forward, out hit3);


        if (downCenter) {

			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			transform.position = new Vector3 (pos.x, pos.y, hit1.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);

		} else if (downLeft) {

			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			transform.position = new Vector3 (pos.x, pos.y, hit2.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);

		} else if (downRight) {
			
			SpriteCollider sc = hit1.collider.gameObject.GetComponent<SpriteCollider>();
			transform.position = new Vector3 (pos.x, pos.y, hit3.collider.gameObject.transform.position.z);
			return sc.getVertCollision(this.gameObject, transform.position.y);
			
		} else {

			return false;

		}

        //return !(Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out hit, rayDistance)); 
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
		if (axis > 0.1f && !collideRight()) {
			
			body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
            anim.SetBool("Running", true);

			anim.SetFloat ("runSpeed", axis);

		}else if (axis < -0.1f && !collideLeft()) {
			body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			
			anim.SetFloat("runSpeed", axis);
            anim.SetBool("Running", true);

            //if (transform.rotation.y == 0) {
                //transform.rotation = Quaternion.Euler(0, 180, 0);
                //Quaternion.Euler(0, 180, 0);
            //}
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
		if (isGrounded() && jump) {
			
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
		if(isGrounded()){
			
			anim.SetBool("Falling",false);
			anim.SetBool("Landing",true);
            if (!jumpPressed) {
                body.velocity = new Vector3(body.velocity.x, 0, 0);
            }
        }

        jumping += Time.deltaTime;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
