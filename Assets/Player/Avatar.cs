using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AvatarCollision))]
public class Avatar : MonoBehaviour, IControllable {

	//protected Rigidbody body;
	protected Animator anim;

	public AvatarCollision avaCol;
	public float deltaY = 0;
	public float deltaX = 0;
    public float xSpeed = 8;
    public float accelY = 0.5f;
    public float accelX = 0.5f;
    public float maxFall = -8.0f;
    public float jumpSpeed = 8.0f;

    public GameObject amunition;
	
	//private float distToGround;
	protected bool grounded;
	protected float jumping;
    protected bool jumpPressed;
	protected float runInput;
	protected bool wallGlide;
	protected int facing;

    private bool donejumping;

	//prefab to spawn
	public GameObject bulletPrefab;
	
	//the bullet that has been spawned
	public GameObject spawnedBullet;

	
	// Use this for initialization
	void Start () {
        jumping = 0;
        grounded = false;
        //body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        avaCol = GetComponent<AvatarCollision>();
        jumpPressed = false;
		wallGlide = false;
		facing = 1;
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
		if (axis > 0.1f) {

			if(deltaX < axis*xSpeed){
				deltaX = deltaX + accelX;
			}else{
                deltaX = axis * xSpeed;
			}
            anim.SetBool("Running", true);

			anim.SetFloat ("runSpeed", axis);
			facing = 1;

		}else if (axis < -0.1f){
			if(deltaX > axis*xSpeed){
				deltaX = deltaX - accelX;
			}else{
                deltaX = axis * xSpeed;
			}
            anim.SetFloat("runSpeed", axis);
            anim.SetBool("Running", true);
            facing = -1;
        }
		/*
		 * If there is no input, sets the x velocity to 0, keeps y velocity the same
		 * Animation Running is set to false, no change in rotation
		 */ 
		else{
            deltaX = 0;
            anim.SetFloat("runSpeed", axis);
            anim.SetBool("Running", false);
        }

		runInput = axis;
	}

    public void doneJump()
    {
        if(deltaY > 0)
        {
            deltaY = 0;
        }
    }

    /// <summary>
    /// Helper function to adjust fall speed.
    /// </summary>
    private void adjustFallSpeed()
    {
        if (!grounded)
        {
            deltaY = deltaY - accelY;
            if ((avaCol.collideRight() && runInput > 0) || (avaCol.collideLeft() && runInput < 0))
            {

                if (deltaY < maxFall / 2)
                {
                    deltaY = maxFall / 2;
                    wallGlide = true;
                }

            }
            else if (deltaY < maxFall)
            {
                deltaY = maxFall;
                wallGlide = false;
            }
        }
        else {
            deltaY = 0;
        } 
    }

    public void adjustPosition()
    {
        float newX = transform.position.x + deltaX * Time.deltaTime;
        float newY = transform.position.y + deltaY * Time.deltaTime;
        float z = transform.position.z;
        transform.position = new Vector3(newX, newY, z);
    }

    private void resolveCollisions()
    {
        GameObject topCollide = avaCol.collideTop();
        GameObject bottomCollide = avaCol.collideBottom();
        GameObject leftCollide = avaCol.collideLeft();
        GameObject rightCollide = avaCol.collideRight();

        Vector3 pos = transform.position;

        float thisHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float thisWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        if (topCollide != null)
        {
            if(deltaY > 0)
            {
                deltaY = 0;
            }
        }
        if (bottomCollide != null)
        {
            if(deltaY < 0)
            {
                deltaY = 0;
                grounded = true;
                transform.position = new Vector3(pos.x, thisHeight / 2 + 0.5f + bottomCollide.transform.position.y - 0.1f, pos.z);
            }
        }
        else
        {
            grounded = false;
        }
        if (leftCollide != null)
        {
            deltaX = 0;
        }
        if (rightCollide != null)
        {
            deltaX = 0;
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
		if (avaCol.collideBottom()) {
            donejumping = false;
            grounded = false;
            deltaY = jumpSpeed;
			anim.SetBool("jumping",true);
		} 

		/*
		 * If the player is gliding down a wall the jump will not be as high and add a bit of horizontal speed
		 */ 
		else if (wallGlide) {
			donejumping = false;
            deltaX = xSpeed * (-runInput);
			anim.SetBool("jumping",true);
		}
	}

    public void shoot(float direction)
    {
        //TODO
		Vector3 fireSpot = new Vector3 (transform.position.x + (0.5f)*facing, transform.position.y, transform.position.z);
		spawnedBullet = GameObject.Instantiate(bulletPrefab, fireSpot, transform.rotation) as GameObject;
		//add force to bullet
		bullet bill = spawnedBullet.GetComponent<bullet> ();
		bill.setBody(bill.GetComponent<Rigidbody> ());
		bill.setVelocity (10*facing, 0);
    }

    void setAnimations()
    {

        if(deltaY > 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("Falling", false);
        }

        if(deltaY < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", true);
        }

        if (avaCol.collideBottom())
        {
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", false);
            anim.SetBool("Landing", true);
        }

    }

	// Update is called once per frame
	void Update () {
        adjustFallSpeed();
        adjustPosition();
        resolveCollisions();
        setAnimations();
	}

    void FixedUpdate()
    {
        //adjustFallSpeed();
        //adjustPosition();
        //adjustYPosition();
        //adjustXPosition();
    }
}
