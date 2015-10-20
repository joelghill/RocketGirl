using UnityEngine;
using System.Collections;

public class setCollision : MonoBehaviour {

	public BoxCollider mainCollider;
	public BoxCollider oneWayTrigger;

	// Use this for initialization
	void Start () {
		//Get colliders and set them if they are null.
		if (mainCollider == null ||
			oneWayTrigger == null) {
			BoxCollider[] colliders = transform.gameObject.GetComponents<BoxCollider>();

			foreach(BoxCollider collider in colliders){
				if(collider.isTrigger)
					this.oneWayTrigger = collider;
				else
					this.mainCollider = collider;
			}
		}
		if (gameObject.tag == "None") {
			//this.mainCollider.enabled = false;
		}

		//if (this.transform.gameObject.tag == "None")

	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			if (this.gameObject.tag == "None") {
				Physics.IgnoreCollision (col.collider, this.mainCollider);
			} else {
				col.gameObject.SendMessage ("addGrounded");
			}
		}
	}

	void OnCollisionExit(Collision col){
		Debug.Log ("Collision Exit");
		col.gameObject.SendMessage ("minusGrounded");
		
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("Trigger Enter");
		if (col.gameObject.tag == "Player") {
			if(this.gameObject.tag == "SemiSolid" || this.gameObject.tag == "None"){
				Physics.IgnoreCollision(col.GetComponent<Collider>(), this.mainCollider);
				if(col.gameObject.transform.position.z >= this.transform.position.z - 0.5){
					Vector3 curPos = col.gameObject.transform.position;
					col.gameObject.transform.position = new Vector3(curPos.x, curPos.y, (float)(this.transform.position.z - 0.5));
				}
			}
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player" && this.gameObject.tag == "SemiSolid") {
			Physics.IgnoreCollision(col.GetComponent<Collider>(), this.mainCollider, false);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
