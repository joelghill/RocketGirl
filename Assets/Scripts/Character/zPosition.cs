using UnityEngine;
using System.Collections;

public class zPosition : MonoBehaviour {

    Rigidbody rb;
    AvatarCollision ac;
	Avatar avatar;
    public bool allowFloat = false;

    private float height;
    private float width;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
        ac = gameObject.GetComponent<AvatarCollision>();
		avatar = gameObject.GetComponent<Avatar>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (rb.velocity.magnitude == 0) return;
        if (rb == null) return;
        //get game object bounds
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
		Vector3 pos = Camera.main.transform.position;//transform.position

        //points to send rays from 
        Vector3 top = new Vector3(pos.x, pos.y + (height / 2), Camera.main.transform.position.z);
        Vector3 bottom = new Vector3(pos.x, pos.y - (height / 2),Camera.main.transform.position.z);
        Vector3 left = new Vector3(pos.x - 0.5f, pos.y,Camera.main.transform.position.z);
        Vector3 right = new Vector3(pos.x + 0.5f, pos.y,Camera.main.transform.position.z);

        //movement cases...
        //if moving right
        if(rb.velocity.x > 0)
        {
            adjustDepth(right, new Vector3(right.x - (float)0.6, right.y, right.z));
        }
        //else if moving left
        else if(rb.velocity.x < 0)
        {
            adjustDepth(left, new Vector3(left.x + (float)0.6, left.y, left.z));
        }

        //if moving up
        if (rb.velocity.y > 0)
        {
            adjustDepth(top, new Vector3(top.x, top.y - (float)0.4, top.z));
        }
        //if moving down
        else if (rb.velocity.y < 0)
        {
            adjustDepth(bottom, new Vector3(bottom.x, bottom.y + (float)0.4, bottom.z));

        }

		RaycastHit hit;
		if (this.isFloating()) {
			if (allowFloat) return;
			//send out raycast into screen from feet of player
			Vector3 below = new Vector3(transform.position.x, transform.position.y - 1, Camera.main.transform.position.z);
			if(Physics.Raycast (below, Camera.main.transform.forward, out hit)){
				this.transform.position = new Vector3(transform.position.x, transform.position.y, hit.collider.transform.position.z);
			}
		}

	}

    /// <summary>
    /// Function that Adjusts the Z position of the parent object based on whether it is behind colliable objects or not.
    /// </summary>
    /// <param name="outside"> Point furthest from center of object in direction of travel.</param>
    /// <param name="inside">Point closest to center of object in direction of travel</param>
    void adjustDepth(Vector3 outside, Vector3 inside)
    {
        Vector3 rayDir = Camera.main.transform.forward;
        RaycastHit hit1;
        RaycastHit hit2;
        // If colliding at the outside point
        if (Physics.Raycast(outside, rayDir, out hit1))
        {
			if(hit1.collider.gameObject.tag == "Player") return;
            // check for collision closer to center of game object
            bool otherCollide = Physics.Raycast(inside, rayDir, out hit2);
            // if there is a collision and the player is not front, then return;
            if (otherCollide && hit2.collider.transform.position.z < transform.position.z) return;

            //else continue and check if outside point is behind object
            if (hit1.collider.transform.position.z < transform.position.z)
            {
                //then game object is about to move behind an object, so adjust z depth
                transform.position = new Vector3(transform.position.x, transform.position.y, hit1.collider.transform.position.z -1 );
            }
        }
    }

    /// <summary>
    /// Is the Object floating in mid air?
    /// </summary>
    /// <returns><c>true</c>, if floating, <c>false</c> otherwise.</returns>
    bool isFloating() {
        RaycastHit hit;
        bool floating = true;
        Vector3 pos = transform.position;

        Vector3 left = new Vector3(pos.x - (width / 2), pos.y, pos.z);
        Vector3 right = new Vector3(pos.x + (width / 2), pos.y, pos.z);

        Vector3[] points = {pos, left, right };

        //if we have the avatar collider
        if(ac != null)
        {
            //... and if the collider is grounded
            if (!ac.collideBottom())
            {
                //then sprite is not considered to be floating
                return false;
            }
        }
        else
        {
            //if for some reason the avatar collision is not there, us velocity. Not as reliable though.
            if (rb.velocity.y != 0)
                return false;
        }

        for(int i = 0; i < points.Length; i++)
        {
            if(Physics.Raycast(points[i], new Vector3(0, -1, 0), out hit, 1.0f))
            {
				floating = false;
			} 	
        }
		//Send raycast down, one unit long. If no collision, player is floating in mid air
		return floating;
	}

    bool rayCheck(Vector3 point)
    {
        return false;

    }
}
