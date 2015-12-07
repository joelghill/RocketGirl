using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public GameObject target;
    public float minDepth = 9.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
            float dY;
            if(target.transform.position.y <= minDepth)
            {
                dY = minDepth;
            }
            else
            {
                dY = target.transform.position.y;
            }
            this.transform.position = new Vector3(this.target.transform.position.x, dY, this.target.transform.position.z - 200.0f);
        } else {
			Debug.Log("CAMERA IS NULL");
		}
	}
}
