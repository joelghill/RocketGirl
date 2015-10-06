using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			this.transform.position= new Vector3(this.target.transform.position.x, this.target.transform.position.y, this.transform.position.z);
		} else {
			Debug.Log("CAMERA IS NULL");
		}
	}
}
