using UnityEngine;
using System.Collections;

public class sendRaycast : MonoBehaviour {
	public BoxCollider collider;
	public Transform transform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform == null)
			return;
		RaycastHit hit;
		float distanceToGround = 0;
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y - 1, transform.position.z);
		if (Physics.Raycast(pos, transform.forward, out hit)) {
			distanceToGround = hit.distance;
			this.collider.size = (new Vector3(1, 1, (distanceToGround +1)*2));
			Debug.Log(hit.ToString());
			Debug.Log("Object is " + distanceToGround.ToString() + " away");
		}else{
			this.collider.size = (new Vector3(1, 1, 1));
		}

	}
}
