﻿using UnityEngine;
using Assets.Scripts.Animation;
using System.Collections;
using System;

[RequireComponent (typeof(AvatarCollision))]
public class Avatar : MonoBehaviour, IControllable, IPauseable {

    private enum CollisionType {TOP,BOTTOM,LEFT,RIGHT};

    private PlayerAnimationController playerAnimationController;

	protected Rigidbody body;
	protected AudioSource sound;

	public AvatarCollision avaCol;
	public float ySpeed = 8;
	public float xSpeed = 6;
    public float accel = 0.5f;
    public float maxFall = -8.0f;
	public float wallJumpForce = 15;

    public GameObject amunition;
	
	//private float distToGround;
	protected bool grounded;
	protected float jumping;
    protected bool jumpPressed;
	protected float runInput;
	protected bool wallGlide;
	protected int facing;

    private bool donejumping;

    private bool paused = false;

	//prefab to spawn
	public GameObject bulletPrefab;
	
	//the bullet that has been spawned
	public GameObject spawnedBullet;

	
	// Use this for initialization
	void Start () {
        jumping = 0;
        grounded = false;
        body = GetComponent<Rigidbody>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        avaCol = GetComponent<AvatarCollision>();
		avaCol.spriteRenderer = GetComponent<SpriteRenderer> ();
        jumpPressed = false;
		wallGlide = false;
		facing = 1;
		sound = GetComponent<AudioSource> ();
		
	}

    public bool isGrounded()
    {
        return grounded;
    }
	
	/*
	 * Sets the jumping animation boolean to true.
	 */ 
	void setJumpingAnim()
    {
        playerAnimationController.SetJumpAnimationState(true);
	}

	/*
	 * Object movement script. Speed is base of the set xSpeed, axis is the direction/scale 
	 * (negative axis means reverse movement, 1.0f will give full xSpeed)
	 */
    public void move (float axis){
        if (paused) return;
        GameObject right = avaCol.collideRight();
        GameObject left = avaCol.collideLeft();

        if (right)
        {
            Debug.Log("Collision on Right side");
            AdjustPosition(right, CollisionType.RIGHT);
        }

        if (left)
        {
            //Debug.Log("Collision on Left side");
            AdjustPosition(left, CollisionType.LEFT);
        }

        /*
		 * Checks whether the axis is positive or negative, sets the speed of the character. 
		 * Checks if the y rotation is correct, sets correct rotation with quaternion euler 
		 */
        if (axis > 0.2f && !avaCol.collideRight()) {

			if(body.velocity.x < axis*xSpeed){
				body.velocity = new Vector3 (body.velocity.x + accel*Time.deltaTime, body.velocity.y,0);
			}else{
				body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			}

            this.playerAnimationController.SetRunAnimationState(true);

            this.playerAnimationController.SetRunAnimationSpeed(axis);
			
            this.playerAnimationController.SetDirection(AnimationDirection.RIGHT);

			if(transform.lossyScale.x < 0){
				adjustRotation();
			}
			//facing = 1;

		}else if (axis < -0.2f && !avaCol.collideLeft()) {
			if(body.velocity.x > axis*xSpeed){
				body.velocity = new Vector3 (body.velocity.x - accel*Time.deltaTime, body.velocity.y,0);
			}else{
				body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			}

            this.playerAnimationController.SetRunAnimationSpeed(axis);

            this.playerAnimationController.SetRunAnimationState(true);

            this.playerAnimationController.SetDirection(AnimationDirection.LEFT);

			if(transform.lossyScale.x > 0){
				adjustRotation();
			}
			//facing = -1;

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

            this.playerAnimationController.SetRunAnimationState(false);
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

        body.velocity = new Vector3(body.velocity.x, body.velocity.y - accel*Time.deltaTime, 0);
		//if(grounded) body.velocity = new Vector3(body.velocity.x, 0, 0);

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

        if (avaCol.collideBottom())
        {
            if (body.velocity.y <= 0)
            {
                body.velocity = new Vector3(body.velocity.x, 0, 0);
                grounded = true;
                AdjustPosition(avaCol.collideBottom(), CollisionType.BOTTOM);
            }
        }

        if(avaCol.collideTop() && body.velocity.y > 0)
        {
            body.velocity = new Vector3(body.velocity.x, 0, 0);
        }

    }

    /*
     * Parameters are whether the jump action is executed (like the jump button, or when the AI wants to jump)
     * and when the jump is completed ( upward collision, jump button released, jump duration expended)
     */
	public void jump(){
        if (paused) return;
		
		/*
		 * Checks if the character is grounded. If yes, and the jump button is pressed, set animations accordingly
		 * and set the y velocity upwards.
		 */ 
		if (grounded == true) {
			
            donejumping = false;
			body.velocity = new Vector3 (body.velocity.x, ySpeed, 0);

            this.playerAnimationController.SetJumpAnimationState(true);
            grounded = false;
			sound.Play();
        }

		/*
		 * If the player is gliding down a wall the jump will not be as high and add a bit of horizontal speed
		 */ 
		else if (wallGlide) {
			donejumping = false;
			wallGlide = false;
			body.velocity = new Vector3 ((wallJumpForce)*(-runInput), ySpeed, 0);

            this.playerAnimationController.SetJumpAnimationState(true);
		}
        else
        {
            Debug.Log("Jump pressed, however player is not grounded");
        }
    }

	// A setter function to turn off the shooting animation
	public void shootFalse(){
        this.playerAnimationController.SetShootingAnimationState(false);
	}

    public void shoot(float direction)
    {
		//anim.SetBool ("shooting", true);
		Vector3 fireSpot = new Vector3 (transform.position.x + (0.5f)*facing, transform.position.y, transform.position.z);
		spawnedBullet = GameObject.Instantiate(bulletPrefab, fireSpot, transform.rotation) as GameObject;
		//add force to bullet
		bullet bill = spawnedBullet.GetComponent<bullet> ();
		bill.setBody(bill.GetComponent<Rigidbody> ());
		bill.setVelocity ((10 * facing) + ((body.velocity.x)/2), 0);
		//anim.SetBool ("shooting", false);
    }

    void setAnimations()
    {

        if(body.velocity.y > 6)
        {
            this.playerAnimationController.SetJumpAnimationState(true);
            this.playerAnimationController.SetFallAnimationState(false);
        }

        if(body.velocity.y <= 6)
        {
            this.playerAnimationController.SetJumpAnimationState(false);
            this.playerAnimationController.SetFallAnimationState(true);

        }

        if (grounded == true)
        {
            this.playerAnimationController.SetJumpAnimationState(false);
            this.playerAnimationController.SetFallAnimationState(false);
            this.playerAnimationController.SetLandingAnimationState(true);
        }

    }

	public void adjustRotation(){
		//Vector3 targetAngles = transform.eulerAngles + 180f * Vector3.up;
		//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 100000f);
		transform.localScale = new Vector3 (transform.lossyScale.x * -1, transform.lossyScale.y, transform.lossyScale.z);
	}

	// Update is called once per frame
	void Update () {
        if (paused) return;
        //adjustFallSpeed();
        setAnimations();
	}

	void FixedUpdate() {
		if (paused) return;
		adjustFallSpeed();
	}


    private void AdjustPosition(GameObject other, CollisionType type)
    {
        Trile t = other.GetComponent<Trile>();
        if (t == null) return;
        switch (type){
            case CollisionType.BOTTOM:

                if (avaCol.Bottom() < t.topPosition())
                {

                    Vector3 pos = transform.position;
					float newY = transform.position.y - (avaCol.Bottom() - t.topPosition());
                    transform.position = new Vector3(pos.x, newY, pos.z);
                }
                break;
            case CollisionType.TOP:
                break;
            case CollisionType.LEFT:
                if (avaCol.Left() < t.rightPosition())
                {
                    Vector3 pos = transform.position;
                    float newX = transform.position.x - (avaCol.Left() - t.rightPosition());
                    transform.position = new Vector3(newX, pos.y, pos.z);
                }
                break;
            case CollisionType.RIGHT:
                if (avaCol.Right() > t.leftPosition())
                {
                    Vector3 pos = transform.position;
                    float newX = transform.position.x - (avaCol.Right() - t.leftPosition());
                    transform.position = new Vector3(newX, pos.y, pos.z);
                }
                break;
            default:
                break;
        }
    }

	public bool getPause(){
		return paused;
	}

    public void onPause()
    {
        paused = true;
    }

    public void onResume()
    {
        paused = false;
    }
}
