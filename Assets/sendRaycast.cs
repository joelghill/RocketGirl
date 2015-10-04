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
			return;
		}
		this.collider.size = (new Vector3(.2f, .35f, .4f));
	}

	bool isFloating(){
		RaycastHit hit;
		return !(Physics.Raycast (this.transform.position, new Vector3 (0, -1, 0),out hit ,1.0f));
	}

	void OnCollisionEnter(Collision c){
		if (isFloating()) {
			Vector3 pos = this.transform.position;
			this.transform.position = new Vector3(pos.x, pos.y, c.gameObject.transform.position.z);
		}
	}
}
