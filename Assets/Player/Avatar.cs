﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AvatarCollision))]
public class Avatar : MonoBehaviour, IControllable {

	protected Rigidbody body;
	protected Animator anim;

	public AvatarCollision avaCol;
	public float ySpeed = 8;
	public float xSpeed = 6;
    public float accel = 0.5f;
    public float maxFall = -8.0f;

    public GameObject amunition;
	
	//private float distToGround;
	protected int grounded;
	protected float jumping;
    protected bool jumpPressed;
	protected float runInput;
	protected bool wallGlide;

    private bool donejumping;
	
	// Use this for initialization
	void Start () {
        jumping = 0;
        grounded = 0;
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        avaCol = GetComponent<AvatarCollision>();
        jumpPressed = false;
		wallGlide = false;
		//avaCol = GetComponent<AvatarCollision> ();
		
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
		if (axis > 0.1f && !avaCol.collideRight()) {

			if(body.velocity.x < axis*xSpeed){
				body.velocity = new Vector3 (body.velocity.x + accel, body.velocity.y,0);
			}else{
				body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			}
            anim.SetBool("Running", true);

			anim.SetFloat ("runSpeed", axis);

		}else if (axis < -0.1f && !avaCol.collideLeft()) {
			if(body.velocity.x > axis*xSpeed){
				body.velocity = new Vector3 (body.velocity.x - accel, body.velocity.y,0);
			}else{
				body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			}
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

		runInput = axis;
	}

    public void doneJump()
    {
        if(body.velocity.y > 0)
        {
            body.velocity = new Vector3(body.velocity.x, 0, 0);
        }
    }

    /// <summary>
    /// Helper function to adjust fall speed.
    /// </summary>
    void adjustFallSpeed()
    {
        body.velocity = new Vector3(body.velocity.x, body.velocity.y - accel, 0);

		if ((avaCol.collideRight () && runInput > 0) || (avaCol.collideLeft () && runInput < 0)) {

			if(body.velocity.y < maxFall/2)
			{
				body.velocity = new Vector3(body.velocity.x, maxFall/2, 0);
				wallGlide = true;
			}
			
		}else if(body.velocity.y < maxFall)
        {
            body.velocity = new Vector3(body.velocity.x, maxFall, 0);
			wallGlide = false;
        }

        if (avaCol.isGrounded())
        {
            if(body.velocity.y < 0)
            {
                body.velocity = new Vector3(body.velocity.x, 0, 0);
            }
        }

    }

    /*
     * Parameters are whether the jump action is executed (like the jump button, or when the AI wants to jump)
     * and when the jump is completed ( upward collision, jump button released, jump duration expended)
     */
	public void jump(){
		
		/*
		 * Checks if the character is grounded. If yes, and the jump button is pressed, set animations accordingly
		 * and set the y velocity upwards.
		 */ 
		if (avaCol.isGrounded()) {
			
            donejumping = false;
			body.velocity = new Vector3 (body.velocity.x, ySpeed, 0);
			anim.SetBool("jumping",true);
		} 

		/*
		 * If the player is gliding down a wall the jump will not be as high and add a bit of horizontal speed
		 */ 
		else if (wallGlide) {
			donejumping = false;
			body.velocity = new Vector3 ((xSpeed)*(-runInput), ySpeed, 0);
			anim.SetBool("jumping",true);
		}
	}

    public void shoot(float direction)
    {

        //TODO

    }

    void setAnimations()
    {

        if(body.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("Falling", false);
        }

        if(body.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", true);
        }

        if (avaCol.isGrounded())
        {
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", false);
            anim.SetBool("Landing", true);
        }

    }

	// Update is called once per frame
	void Update () {
        adjustFallSpeed();
        setAnimations();
	}
}
