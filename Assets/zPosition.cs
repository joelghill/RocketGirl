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
        if (rb.velocity.magnitude == 0) return;

        //get game object bounds
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 pos = transform.position;

        //points to send rays from 
        Vector3 top = new Vector3(pos.x, pos.y + (height / 2), Camera.main.transform.position.z);
        Vector3 bottom = new Vector3(pos.x, pos.y - (height / 2), Camera.main.transform.position.z);
        Vector3 left = new Vector3(pos.x - 0.4f, pos.y, Camera.main.transform.position.z);
        Vector3 right = new Vector3(pos.x + 0.4f, pos.y, Camera.main.transform.position.z);

        Vector3 rayDir = Camera.main.transform.forward;

        RaycastHit hit1;
        RaycastHit hit2;

        //movement cases...
        //if moving right
        if(rb.velocity.x > 0)
        {
            //... and colliding on the right but not the left
            if(Physics.Raycast(right, rayDir, out hit1) &&
                !Physics.Raycast(new Vector3(right.x - (float)0.4, right.y, right.z), rayDir, out hit2))
            {
                //.... and is behind the object
                if(hit1.collider.transform.position.z < transform.position.z)
                {
                    //then move to a point in fron of that z
                    transform.position = new Vector3(pos.x, pos.y, hit1.collider.transform.position.z - 1);
                }
            }
        }
        //else if moving left
        else if(rb.velocity.x < 0)
        {
            //... and colliding on the right but not the left
            if (Physics.Raycast(left, rayDir, out hit1) &&
                !Physics.Raycast(new Vector3(left.x + (float)0.4, left.y, left.z), rayDir, out hit2))
            {
                //.... and is behind the object
                if (hit1.collider.transform.position.z < transform.position.z)
                {
                    //then move to a point in fron of that z
                    transform.position = new Vector3(pos.x, pos.y, hit1.collider.transform.position.z - 1);
                }
            }
        }

        if (rb.velocity.y > 0)
        {
            //... and colliding on the right but not the left
            if (Physics.Raycast(top, rayDir, out hit1) &&
                !Physics.Raycast(new Vector3(top.x, top.y - (float)0.4, top.z), rayDir, out hit2))
            {
                //.... and is behind the object
                if (hit1.collider.transform.position.z < transform.position.z)
                {
                    //then move to a point in fron of that z
                    transform.position = new Vector3(pos.x, pos.y, hit1.collider.transform.position.z - 1);
                }
            }
        }
        //if moving up
        else if (rb.velocity.y < 0)
        {
            //... and colliding on the right but not the left
            if (Physics.Raycast(bottom, rayDir, out hit1) &&
                !Physics.Raycast(new Vector3(bottom.x, bottom.y + (float)0.4, bottom.z), rayDir, out hit2))
            {
                //.... and is behind the object
                if (hit1.collider.transform.position.z < transform.position.z)
                {
                    //then move to a point in fron of that z
                    transform.position = new Vector3(pos.x, pos.y, hit1.collider.transform.position.z - 1);
                }
            }
        }

        RaycastHit hit;
		if (this.isFloating()) {
			//send out raycast into screen from feet of player
			Vector3 below = new Vector3(transform.position.x, transform.position.y - 1, Camera.main.transform.position.z);
			if(Physics.Raycast (below, Camera.main.transform.forward, out hit)){
					this.transform.position = new Vector3(transform.position.x, transform.position.y, hit.collider.transform.position.z);
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

    bool rayCheck(Vector3 point)
    {
        return false;

    }
}
