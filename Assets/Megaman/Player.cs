using UnityEngine;
using System.Collections;

public class Player : Avatar {

	// Use this for initialization
	void Start () {

		jumping = 0;
		grounded = 0;
		body = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		avaCol = GetComponent<AvatarCollision> ();


	}
	
	void playerMove(){

		/*
		 * Retrieves axis from analog stick, then checks for keyboard input
		 */ 
		float axis = Input.GetAxis ("Horizontal");
		if (Input.GetKey ("d")) {
			axis = 1.0f;
		} else if (Input.GetKey ("a")) {
			axis = -1.0f;
		}

		move (axis);

	}

	/*
	 * Tells when jump starts based on when the jump button is pressed, and when the 
	 * jump ends based on when the jump button is released.
	 */ 
	void playerJump(){

		jump (Input.GetButtonDown ("Jump"), Input.GetButtonUp ("Jump"));

	}

    // Update is called once per frame
    void Update () {

        if(!this.avaCol.isGrounded() && !jumpPressed) {
            body.velocity = new Vector3(body.velocity.x, -ySpeed, body.velocity.z);
            anim.SetBool("Falling", true);
        }//else if (isGrounded() && !jumpPressed) {
           // body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
       // }

		playerMove ();
		playerJump ();

    }
}
