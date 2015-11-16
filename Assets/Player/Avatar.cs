using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(AvatarCollision))]
public class Avatar : MonoBehaviour, IControllable, IPauseable {

    private enum CollisionType {TOP,BOTTOM,LEFT,RIGHT};

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
        grounded = 0;
        body = GetComponent<Rigidbody>();
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
        if (paused) return;
        GameObject right = avaCol.collideRight();
        GameObject left = avaCol.collideLeft();

        if (right)
        {
            AdjustPosition(right, CollisionType.RIGHT);
        }

        if (left)
        {
            AdjustPosition(left, CollisionType.LEFT);
        }

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
			//facing = 1;

		}else if (axis < -0.1f && !avaCol.collideLeft()) {
			if(body.velocity.x > axis*xSpeed){
				body.velocity = new Vector3 (body.velocity.x - accel, body.velocity.y,0);
			}else{
				body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);
			}
			anim.SetFloat("runSpeed", axis);
            anim.SetBool("Running", true);
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

        if (avaCol.collideBottom())
        {
            if (body.velocity.y <= 0)
            {
                body.velocity = new Vector3(body.velocity.x, 0, 0);
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
		if (avaCol.collideBottom()) {
			
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
        else
        {
            Debug.Log("Jump pressed, however player is not grounded");
        }
    }

	// A setter function to turn off the shooting animation
	public void shootFalse(){
		anim.SetBool ("shooting", false);
	}

    public void shoot(float direction)
    {
		anim.SetBool ("shooting", true);
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

        if (avaCol.collideBottom())
        {
            anim.SetBool("jumping", false);
            anim.SetBool("Falling", false);
            anim.SetBool("Landing", true);
        }

    }

	public void adjustRotation(){
		//Vector3 targetAngles = transform.eulerAngles + 180f * Vector3.up;
		//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 100000f);
		transform.localScale = new Vector3 (transform.lossyScale.x * -1, transform.lossyScale.y, transform.lossyScale.z);
		facing = facing * -1;
	}

	// Update is called once per frame
	void Update () {
        if (paused) return;
        adjustFallSpeed();
        setAnimations();
		if ((Input.GetAxis ("Horizontal") > 0.1 || Input.GetKey ("d")) && facing == -1) {
			adjustRotation();
		} else if ((Input.GetAxis ("Horizontal") < -0.1 || Input.GetKey ("a")) && facing == 1) {
			adjustRotation();
		}
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

    public void onPause()
    {
        paused = true;
    }

    public void onResume()
    {
        paused = false;
    }
}
