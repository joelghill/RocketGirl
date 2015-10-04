using UnityEngine;
using System.Collections;

public class player_movement_2 : MonoBehaviour {

	public Rigidbody r_body;

	public float jump_force;
	public float max_speed_x;
	public float x_accel = 0.5f;
	public bool grounded = true;

	private Vector3 startPos;

	private float working_x_accel;
	//public float max_speed_fall;

	void Start () {
		this.startPos = this.gameObject.transform.position;
		r_body = GetComponent<Rigidbody>();
	}
	
	void Update () {
	
		// get spacebar input and jump
		if(Input.GetButtonDown ("Jump"))
		{
			// jump - add force up
			r_body.velocity = new Vector2(r_body.velocity.x, 0.5f);
			r_body.AddForce(new Vector2(0,jump_force));
			// play jump animation
		}
	}

	void Kill(){
		this.gameObject.transform.position = this.startPos;
		this.r_body.velocity = new Vector2 (0, 0);
	}

	void ApplyFriction(float friction){
		if (friction == 0)
			this.working_x_accel = 0f;
		if (friction < 1) {
			this.working_x_accel = 0.5f;
		} else {
			working_x_accel  = x_accel;
		}

	}

	void AdjustPositionFromPlatform(Vector3 delta){
		if(!Input.GetButtonDown ("Jump")){
			Debug.Log ("Adjusting position with platform");
			//if the player is moving left or right, do not follow platform X
			//if(!(Input.GetAxis("Horizontal") == 0)) 
			delta.x = 0;
			this.gameObject.transform.Translate(delta);
		}
	}

	void SetGrounded(bool g){
		this.grounded = g;
	}

	void FixedUpdate()
	{
		// get left/right , a/d 
		float h  = Input.GetAxis ("Horizontal");
		// add forces for movement

		if (h == 0) {
			Debug.Log ("No horizontal movement");
			return;
		}
		/*
		if (Mathf.Abs (r_body.velocity.x) >= max_speed_x) {
			r_body.velocity = new Vector2 (h * max_speed_x, r_body.velocity.y);
		} else {
			r_body.AddForce(new Vector2(h * move_force,0));
		}*/
		if (Mathf.Abs (r_body.velocity.x) >= max_speed_x) {
			r_body.velocity = new Vector3 (max_speed_x * h, r_body.velocity.y, 0);
		} else {
			r_body.velocity = new Vector3 (r_body.velocity.x + (h * this.x_accel), r_body.velocity.y, 0);
		}
		//anim.SetFloat ("Speed", Mathf.Abs (h));
	}

	void SetSpriteDirection(){
		float h = Input.GetAxis ("Horizontal");
		Quaternion r = this.gameObject.transform.rotation;

		if (h > 0 && r.y > 0)
			this.gameObject.transform.rotation = new Quaternion (r.x, 0, r.z, r.w);

		if (h < 0 && r.y < 170)
			this.gameObject.transform.rotation =  new Quaternion (r.x, 180, r.z, r.w);
	}
}











