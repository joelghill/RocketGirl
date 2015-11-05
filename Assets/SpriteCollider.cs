using UnityEngine;
using System.Collections;

/// <summary>
/// Looks at the tags of the parent game object.
/// Returns whether or not a collision with any given object has occured.
/// Used by "RayCollider"
/// </summary>

public class SpriteCollider : MonoBehaviour {

	public float margin;
	// Use this for initialization
	void Start () {
	
	}

	/// <summary>
	/// Gets the Vertical collision state.
	/// </summary>
	/// <returns><c>true</c>, if vert collision was gotten, <c>false</c> otherwise.</returns>
	/// <param name="other"> The other game object to check collsion against.</param>
	/// <param name="y">y position of the raycast that collided with this object</param>
	public bool getVertCollision(GameObject other, float y){
		//get tag
		string tag = this.gameObject.tag;
		//based on tag, check if collision with other at point occured
		switch (tag) {

		case "None":
			//never register collision
			return false;
		case "Solid": 
			//always collides
			//Debug.Log ("Vert collision detected");
			return true;
		case "SemiSolid":
			//get transform
			Transform otherT =other.transform;
			//get rigidbody
			Rigidbody rb = other.GetComponent<Rigidbody>();
			float otherHeight = other.GetComponent<SpriteRenderer> ().bounds.size.y;

			//if moving up, no collision
			if(rb.velocity.y > 0 || (y -.5f < transform.position.y + GetComponent<BoxCollider>().bounds.size.y/2)){

				return false;
			}
			//if moving not up, and near top, collide
			//else if(y > (getTopY()))
			//	return true;
			else
				//else no collision
				return true;

		case "Moveable":
			//like solid
			return true;
		default:
			//if untagged no collision
			return false;
		}
	}

	/// <summary>
	/// Gets the horz collision.
	/// </summary>
	/// <returns><c>true</c>, if horz collision, <c>false</c> otherwise.</returns>
	/// <param name="other">The other game objct to check collsion against.</param>
	public bool getHorzCollision(GameObject other){
		//get tag
		string tag = this.gameObject.tag;
		//based on tag, check if collision with other at point occured
		switch (tag) {
			
		case "None":
			//never register collision
			return false;
		case "Solid": 
			//always collides
			return true;

        case "SemiSolid":
			//semisolid no not collid in horz directions
			return false;
			
		case "Moveable":
			//like solid
			return true;
		default:
			//if untagged no collision
			return false;
		}
	}

	private float getTopY(){
		//middle position plus half it's size minus the margin
		return this.transform.position.y + (this.transform.lossyScale.y / 2) - this.margin;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
