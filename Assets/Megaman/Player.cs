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

	void OnCollisionEnter(Collision col){

		//col.enabled = false;

		grounded = true;

		if (col.gameObject.tag == "Bottom") {
			transform.position = new Vector2(-3,-1);
		}

	}

	void OnCollisionStay(Collision col){

		grounded = true;

	}

	void OnCollisionExit(Collision col){

		grounded = false;
		anim.SetBool("jumping",true);

	}

	void move(){

		float axis = Input.GetAxis ("Horizontal");
		if (Input.GetKey ("a")) {
			axis = 1.0f;
			print (axis.ToString());
		} else if (Input.GetKey ("d")) {
			axis = -1.0f;
		}
		
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
			
		} else{
			body.velocity = new Vector3 (0, body.velocity.y,0);
			anim.SetBool("Running", false);
		}

	}

	void jump(){

		if (grounded && Input.GetButtonDown ("Jump")) {

			//print (grounded.ToString());
			anim.SetBool("Running", false);
			jumping = jumpDuration;
			body.velocity = new Vector3 (body.velocity.x, ySpeed, 0);
			anim.SetBool("jumping",true);
			anim.SetFloat("jumpTime",0);

		} else if (jumping == 0 || Input.GetButtonUp ("Jump")) {

			//anim.SetBool("Running", false);
			//grounded = 0;
			jumping = 0;
			body.velocity = new Vector3 (body.velocity.x, -ySpeed, 0);
			anim.SetFloat("jumpTime",1);

		}

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
