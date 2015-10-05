using UnityEngine;
using System.Collections;

public class sendRaycast : MonoBehaviour {
	
	public BoxCollider collider;
	public Transform transform;
	public Rigidbody rigid;

	// Use this for initialization
	void Start () {
		if (transform == null) {
			transform = this.gameObject.transform;
		}
		if (this.collider == null)
			this.collider = this.gameObject.GetComponent<BoxCollider> ();
		if (rigid == null)
			this.rigid = this.gameObject.GetComponent<Rigidbody> ();

	}

	void OnCollisionEnter(Collision c){
		//If the user is floating, move to collider that user is collided with.
		if (isFloating()) {
			Vector3 pos = this.transform.position;
			this.transform.position = new Vector3(pos.x, pos.y, c.gameObject.transform.position.z);
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (transform == null || this.rigid.velocity.magnitude == 0) {
			Debug.Log ("No movement or transform.");

			return;
		}
		if(isFloating())
			Debug.Log ("FLOATING");
		RaycastHit hit;
		float distanceToGround = 0;
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
		if (Physics.Raycast (pos, transform.forward, out hit)) {
			distanceToGround = hit.distance;
			this.collider.size = (new Vector3 (.2f, .35f, (distanceToGround + .4f) * 2));
			return;
		}
			//Debug.Log(hit.ToString());
			//Debug.Log("Object is " + distanceToGround.ToString() + " away");
		if (Physics.Raycast (pos, -1*transform.forward, out hit)) {
			distanceToGround = hit.distance;
			this.collider.size = (new Vector3 (.2f, .35f, (distanceToGround + .35f) * 2));
		} else {
			AdjustCollider();
		}
	}

	void AdjustCollider(){
		float yDist = 0;
		float xDist = 0;

		bool yHit;
		bool xHit;
		//nothing to do if no player movement
		if (rigid.velocity.magnitude == 0.0f)
			return;
		RaycastHit hit;
		Vector3 xPos = getXRaycastPosition ();
		Vector3 yPos = getYRaycastPosition ();

		yDist = sendRaycastFromPosition (yPos);
		xDist = sendRaycastFromPosition (xPos);

		float newDist = Mathf.Max (xDist, yDist);

		if(newDist >= 0)
			Debug.Log ("Setting COllider Size.");
			this.collider.size = (new Vector3 (1, 1, (newDist + 1) * 2));

	}
	/// <summary>
	/// Sends the raycast from position.
	/// </summary>
	/// <returns> True if ray hit, false otherwise.</returns>
	/// <param name="position">Position to send raycast from.</param>
	/// <param name="distance">Distance to hit. Updated if there is a hit with a ray</param>
	float sendRaycastFromPosition(Vector3 position){
		RaycastHit hit;
		//send raycast towards screen from camera.
		if (Physics.Raycast (position, Camera.main.transform.forward, out hit)) {
			Debug.Log ("RAYCAST HIT");
			Debug.Log ("Distance Was:  " + hit.distance.ToString());
			float d = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
			return  Mathf.Abs(d - hit.distance);
		}
		this.collider.size = (new Vector3(.2f, .35f, .4f));

		//if no collisions, return 0
		return -1;
	}

	/// <summary>
	/// Is the Object floating in mid air?
	/// </summary>
	/// <returns><c>true</c>, if floating, <c>false</c> otherwise.</returns>
	bool isFloating(){
		RaycastHit hit;
		//Send raycast down, one unit long. If no collision, player is floating in mid air
		return !(Physics.Raycast (this.transform.position, new Vector3 (0, -1, 0),out hit ,1.0f));
	}

	/// <summary>
	/// Gets the X raycast start position for adjusting z collider.
	/// </summary>
	/// <returns>The X raycast position.</returns>
	Vector3 getXRaycastPosition(){
		float xVel = rigid.velocity.x;
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z);
		//if moving left, put position one unit to left
		if (xVel < 0) {
			pos.x = pos.x -1;
		//if moving right, put pos to one unit right
		} else if (xVel > 0) {
			pos.x = pos.x +1;
		}
		//if not moving, keep in center of player.
		return pos;
	}

	/// <summary>
	/// Gets the X raycast start position for adjusting z collider.
	/// </summary>
	/// <returns>The X raycast position.</returns>
	Vector3 getYRaycastPosition(){
		float yVel = rigid.velocity.y;
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, Camera.main.transform.position.z);
		//if falling, put position one unit below
		if (yVel < 0) {
			pos.y = pos.y -1;
			//if moving up, put pos one unit up
		} else if (yVel > 0) {
			pos.y = pos.y +1;
		}
		//if not moving, keep in center of player.
		return pos;
	}
}
