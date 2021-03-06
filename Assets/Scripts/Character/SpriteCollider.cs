﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Looks at the tags of the parent game object.
/// Returns whether or not a collision with any given object has occured.
/// Used by "RayCollider"
/// </summary>

public class SpriteCollider : MonoBehaviour {

	private CollectCoin coin;

	public float margin;
	// Use this for initialization
	void Start () {
		coin = GetComponent<CollectCoin> ();
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
        bool collide;
		//based on tag, check if collision with other at point occured
		switch (tag) {

		case "Coin":
			coin.OnCoinCollision(other);
			collide = false;
			break;

		case "None":
			//never register collision
			collide = false;
            break;
		case "Solid": 
			//always collides
			collide =  true;
            break;

		case "SemiSolid":
			//return true;
			//get transform

			Transform otherT =other.transform;
			//get rigidbody
			Rigidbody rb = other.GetComponent<Rigidbody>();
			float otherHeight = other.GetComponent<SpriteRenderer> ().bounds.size.y;

                //if moving up, no collision
                if (rb.velocity.y > 0)
                {
					Debug.Log("Detected one way tile but no collision");
                    return false;
                    break;
                }
               	else if (other.transform.position.y - otherHeight/2 < transform.position.y)
               	{
					
					Debug.Log("Detected one way tile but no collision; y lower than tile");
                    return false;
               	}

                else{
                    if(other.GetComponent<Player>() != null){
						other.GetComponent<Player>().setRespawnPosition(new Vector3(rb.transform.position.x,
					                                                            this.transform.position.y+otherHeight+0.5f, 
					                                                            this.transform.position.z));
					}

                    return true;
				}
                break;


		case "Moveable":
			//like solid
			collide = true;
            break;
		default:
			//if untagged no collision
			collide  = false;
            break;
		}
        return collide;
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

		case "Coin":
			coin.OnCoinCollision(other);
			return false;
			
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

    public void correctVerticalPosition(GameObject other)
    {
        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
        SpriteRenderer otherRender = other.GetComponent<SpriteRenderer>();
        //SpriteRenderer thisRender = gameObject.GetComponent<SpriteRenderer>();

        if (otherRigid == null | otherRender == null)
        {
            Debug.Log("Returning from position correction....");
            return;
        }

        Vector3 otherPos = other.transform.position;
        Vector3 thisPos = transform.position;

        float otherheight = otherRender.bounds.size.y;
        float thisHeight = 1.0f;
        //If moving up....
        if(otherRigid.velocity.y > 0)
        {
            Debug.Log("Correcting Vertical position...");
            other.transform.position = new Vector3(otherPos.x, thisPos.y - (otherheight / 2 + thisHeight / 2), otherPos.z);
        }else if(otherRigid.velocity.y < 0)
        {
            Debug.Log("Correcting Vertical position...");
            other.transform.position = new Vector3(otherPos.x, thisPos.y + (otherheight / 2 + thisHeight / 2), otherPos.z);
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
