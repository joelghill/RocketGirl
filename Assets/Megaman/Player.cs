using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Rigidbody body;
	Animator anim;

	public float ySpeed = 5;
	public float xSpeed = 5;
	public int jumpDuration = 15;

	//private float distToGround;
	private bool grounded;
	private int jumping;

	// Use this for initialization
	void Start () {
		//distToGround = transform.lossyScale.y;
		jumping = 0;
		grounded = false;
		body = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
	}

	/*
	 * Tells the player they are grounded upon collision with another object
	 */ 
	void OnCollisionEnter(Collision col){

		//col.enabled = false;

		grounded = true;

		if (col.gameObject.tag == "Bottom") {
			transform.position = new Vector2(-3,-1);
		}

	}

	/*
	 * As long as the player stays collided, they are grounded
	 */ 
	void OnCollisionStay(Collision col){

		grounded = true;

	}

	/*
	 * When exiting another objects collider, grounded is set to false.
	 */ 
	void OnCollisionExit(Collision col){

		grounded = false;
		anim.SetBool("jumping",true);

	}

	void move(){

		/*
		 * Retrieves axis from analog stick, then checks for keyboard input
		 */ 
		float axis = Input.GetAxis ("Horizontal");
		if (Input.GetKey ("a")) {
			axis = 1.0f;
			print (axis.ToString());
		} else if (Input.GetKey ("d")) {
			axis = -1.0f;
		}

		/*
		 * checks whether the axis is positive or negative, sets the speed of the character. 
		 * Checks if the y rotation is correct, sets correct rotation with quaternion euler 
		 */
		if (axis > 0.1f) {

			body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);

			if(grounded){
				anim.SetBool ("Running", true);
			}else{
				anim.SetBool ("Running", false);
			}

			anim.SetFloat ("runSpeed", axis);
			//transform.rotation.y = 180;
			if(transform.rotation.y == 1){
				transform.rotation = Quaternion.Euler(0, 0, 0);
			}
			
		}else if (axis < -0.1f) {
			body.velocity = new Vector3 (axis * xSpeed, body.velocity.y,0);

			anim.SetFloat("runSpeed", axis);

			if(grounded){
				anim.SetBool ("Running", true);
			}else{
				anim.SetBool ("Running", false);
			}
			
			if(transform.rotation.y == 0){
				transform.rotation = Quaternion.Euler(0, 180, 0);
				//Quaternion.Euler(0, 180, 0);
			}
			
		} 
		/*
		 * If there is no input, sets the x velocity to 0, keeps y velocity the same
		 * Animation Running is set to false, no change in rotation
		 */ 
		else{
			body.velocity = new Vector3 (0, body.velocity.y,0);
			anim.SetBool("Running", false);
		}

	}

	void jump(){

		/*
		 * Checks if the player is grounded. If yes, and the jump button is pressed, set animations accordingly
		 * and set the y velocity upwards.
		 */ 
		if (grounded && Input.GetButtonDown ("Jump")) {

			//print (grounded.ToString());
			anim.SetBool("Running", false);
			jumping = jumpDuration;
			body.velocity = new Vector3 (body.velocity.x, ySpeed, 0);
			anim.SetBool("jumping",true);
			anim.SetFloat("jumpTime",0);

		} 

		/*
		 * Waits until the jump duration is expended or until the jump button is released to
		 * set the y velocity to negative.
		 */ 
		else if (jumping == 0 || Input.GetButtonUp ("Jump")) {

			//anim.SetBool("Running", false);
			//grounded = 0;
			jumping = 0;
			body.velocity = new Vector3 (body.velocity.x, -ySpeed, 0);
			anim.SetFloat("jumpTime",1);

		}

		/*
		 * If grounded and the jump animation is still happening, that needs to stop
		 */ 
		if(grounded && anim.GetFloat("jumpTime") == 1){

			anim.SetBool("jumping",false);
			anim.SetFloat("jumpTime",2);

		}
		
		if (jumping > 0) {
			jumping--;
		}

	}

	// Update is called once per frame
	void Update () {

		move ();
		jump ();

	}
}
