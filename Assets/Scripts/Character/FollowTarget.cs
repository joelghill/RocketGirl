using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public GameObject target;
    public float minDepth = 9.0f;
	public float margin = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {

			float newX;
			float newY;

			Vector3 targetPos = target.transform.position;
			Vector3 cameraPos = transform.position;

			float dX = targetPos.x - cameraPos.x;
			float dY = targetPos.y - cameraPos.y;

			newY = cameraPos.y;
			if(dY <= -1*margin){
				newY = targetPos.y + margin;
			}

			if(dY >= 1*margin){
				newY = targetPos.y - margin;
			}

            if(newY <= minDepth)
            {
                newY = minDepth;
            }

            this.transform.position = new Vector3(this.target.transform.position.x, newY, this.target.transform.position.z - 200.0f);

        } else {
			Debug.Log("CAMERA IS NULL");
		}
	}
}
