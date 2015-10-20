using UnityEngine;
using System.Collections;

public class zPosition : MonoBehaviour {

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		this.rb = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Vector3 raypos = new Vector3 (this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
		if (Physics.Raycast (raypos, Camera.main.transform.forward, out hit)) {
			if(hit.collider.transform.position.z < this.transform.position.z - 0.5){
				this.transform.position = new Vector3(transform.position.x, transform.position.y, hit.collider.transform.position.z -1);
			}
		}
		if (this.isFloating()) {
			//send out raycast into screen from feet of player
			Vector3 below = new Vector3(transform.position.x, transform.position.y - 1, Camera.main.transform.position.z);
			if(Physics.Raycast (below, Camera.main.transform.forward, out hit)){
				//if deeper in, then closest in z, go there
				if(hit.collider.transform.position.z > this.transform.position.z){
					this.transform.position = new Vector3(transform.position.x, transform.position.y, hit.collider.transform.position.z);
				}else{
					//ptherwise creep forward until not floating
					while(this.isFloating()){
						this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z -1);
					}
				}
			}
		}
	}

	/// <summary>
	/// Is the Object floating in mid air?
	/// </summary>
	/// <returns><c>true</c>, if floating, <c>false</c> otherwise.</returns>
	bool isFloating(){
		RaycastHit hit;
		//if the player is falling or jumping, don't bother with all this noise.
		if (rb.velocity.y != 0)
			return false;
		//Send raycast down, one unit long. If no collision, player is floating in mid air
		return !(Physics.Raycast (this.transform.position, new Vector3 (0, -1, 0),out hit ,1.0f));
	}
}
