﻿using UnityEngine;
using System.Collections;

public class AvatarCollision : MonoBehaviour {

	protected Vector3 VertColPos;
	protected Vector3 HorColPos;
	protected float height;

	// Use this for initialization
	void Start () {
		height = gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
	}


	public bool collideTop()
	{
		Vector3 pos = this.transform.position;
		
		Vector3 rayTop = new Vector3(pos.x, pos.y + (transform.lossyScale.y)/5, Camera.main.transform.position.z);
		Vector3 rayTopLeft = new Vector3(pos.x-0.4f, pos.y + (transform.lossyScale.y)/5, Camera.main.transform.position.z);
		Vector3 rayTopRight = new Vector3(pos.x+0.4f, pos.y + (transform.lossyScale.y)/5, Camera.main.transform.position.z);

        Vector3[] points = { rayTop, rayTopLeft, rayTopRight };

        Vector3 rayTop2 = new Vector3(pos.x, pos.y + (transform.lossyScale.y) / 5, pos.z);
        Vector3 rayTopLeft2 = new Vector3(pos.x - 0.4f, pos.y + (transform.lossyScale.y) / 5, pos.z);
        Vector3 rayTopRight2 = new Vector3(pos.x + 0.4f, pos.y + (transform.lossyScale.y) / 5, pos.z);

        Vector3[] points2 = { rayTop2, rayTopLeft2, rayTopRight2 };

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.up);
    }

	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public bool collideLeft(){
		
		Vector3 pos = this.transform.position;
		
		Vector3 rayLeft = new Vector3(pos.x - 0.5f, pos.y, Camera.main.transform.position.z);
		Vector3 rayLeftUp = new Vector3(pos.x - 0.5f, pos.y+0.5f, Camera.main.transform.position.z);
		Vector3 rayLeftDown = new Vector3(pos.x - 0.5f, pos.y-0.5f, Camera.main.transform.position.z);

        Vector3[] points = { rayLeft, rayLeftUp, rayLeftDown };

        Vector3 rayLeft2 = new Vector3(pos.x - 0.5f, pos.y, pos.z);
        Vector3 rayLeftUp2 = new Vector3(pos.x - 0.5f, pos.y + 0.5f, pos.z);
        Vector3 rayLeftDown2 = new Vector3(pos.x - 0.5f, pos.y - 0.5f,pos.z);

        Vector3[] points2 = { rayLeft2, rayLeftUp2, rayLeftDown2 };

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.right*(-1));
    }
	
	/*
     * Creates three arrays to the right of the player. Returns true if there is an object, therefore a 
     * collision is detected.
     */
	public bool collideRight(){
		
		Vector3 pos = this.transform.position;
		
		Vector3 rayRight = new Vector3(pos.x + 0.5f, pos.y, Camera.main.transform.position.z);
		Vector3 rayRightUp = new Vector3(pos.x + 0.5f, pos.y+0.5f, Camera.main.transform.position.z);
		Vector3 rayRightDown = new Vector3(pos.x + 0.5f, pos.y-0.5f, Camera.main.transform.position.z);

        Vector3[] points = { rayRight, rayRightUp, rayRightDown};

        //second set of points
        Vector3 rayRight2 = new Vector3(pos.x + 0.5f, pos.y, pos.z);
        Vector3 rayRightUp2 = new Vector3(pos.x + 0.5f, pos.y + 0.5f,pos.z);
        Vector3 rayRightDown2 = new Vector3(pos.x + 0.5f, pos.y - 0.5f, pos.z);

        Vector3[] points2 = { rayRight2, rayRightUp2, rayRightDown2 };

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.right);

    }
	
	/*
	 * Sends raycast bellow the character. If it hits an object, the character is grounded
	 */
     /// <summary>
     /// Determines whether or not the player os grounded. Checks for collision with sprite, then uses Sprite collider to resolved collision.
     /// </summary>
     /// <returns>True or false</returns>
	public bool isGrounded()
	{
        //avatar position
		Vector3 pos = this.transform.position;
		
        //first set of points
		Vector3 rayBottom = new Vector3(pos.x, pos.y - (transform.lossyScale.y)/5, Camera.main.transform.position.z);
		Vector3 rayBottomLeft = new Vector3(pos.x-0.4f, pos.y - (transform.lossyScale.y)/5, Camera.main.transform.position.z);
		Vector3 rayBottomRight = new Vector3(pos.x+0.4f, pos.y - (transform.lossyScale.y)/5, Camera.main.transform.position.z);

        Vector3[] points = { rayBottom, rayBottomLeft, rayBottomRight };

        //second set of points
        Vector3 rayBottom2 = new Vector3(pos.x, pos.y, pos.z);
        Vector3 rayBottomLeft2 = new Vector3(pos.x - 0.4f, pos.y, pos.z);
        Vector3 rayBottomRight2 = new Vector3(pos.x + 0.4f, pos.y, pos.z);

        Vector3[] points2 = { rayBottom2, rayBottomLeft2, rayBottomRight2 };

        //check collision
        return twoLevelCollisionCheck(points, points2, transform.up * (-1));
	}

    /// <summary>
    /// Function to check for collision using a series of check points.
    /// </summary>
    /// <param name="pointList">List of points to send rays to.</param>
    /// <param name="direction">Direction to send the ray in.</param>
    /// <param name="distance">Optional. Usually distance from center of object to border in direction of collision check.</param>
    /// <returns>The Sprite collider of the other object.</returns>
    public SpriteCollider checkCollisionList(Vector3[] pointList, Vector3 direction, float distance = 0.0f)
    {
        RaycastHit hit;
        for (int i = 0; i < pointList.Length; i++)
        {
            bool collide;
            if(distance != 0.0)
            {
                collide = Physics.Raycast(pointList[i], direction, out hit, distance);
            }
            else
            {
                collide = Physics.Raycast(pointList[i], direction, out hit);
            }
            if (collide)
            {
                return hit.collider.gameObject.GetComponent<SpriteCollider>();
            }
        }
        return null;
    }

    /// <summary>
    /// A collision check with 2 level verification.
    /// The First check uses rays from camera to player.
    /// If first check returns false, use alternative rays from player.
    /// This is to ensure the player still collides with objects when hidden from view.
    /// </summary>
    /// <param name="firstPoints">First set of points to send rays from. Usually at camera</param>
    /// <param name="secondPoints">Second set. Usually at player</param>
    /// <param name="secondaryDirection"> Direction to send secondary rays from. Usually left, right, up or down.</param>
    /// <returns></returns>
    public bool twoLevelCollisionCheck(Vector3[] firstPoints, Vector3[] secondPoints, Vector3 secondaryDirection)
    {
        //The allowed distance of the ray for secondary collision check.
        float distance;
        //if it's not up or down it's left or right
        if(secondaryDirection == transform.up || secondaryDirection == -1 * transform.up)
        {
            //vertical collision from center to bottom or top of sprite
            distance = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        }
        else
        {
            //horizontal collision is distance from center to left or right side.
            distance = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }

        //check intial point list for valid collsion
        SpriteCollider sc = checkCollisionList(firstPoints, Camera.main.transform.forward);
        bool collide = false;

        //
        if (sc != null)
        {
            //resolve if there was a valid collision with other sprite.
            collide = sc.getVertCollision(this.gameObject, transform.position.y);
        }
        //if there was no valid collision, check secondary
        //this is to avoid player falling through platforms when behind non-colliding objects.
        if (sc == null || !collide)
        {
            sc = checkCollisionList(secondPoints, secondaryDirection, distance);
            if (sc != null)
            {
                collide = sc.getVertCollision(this.gameObject, transform.position.y);
            }
            if (sc == null || !collide)
            {
                return false;
            }
        }
        //shouldn't actually get to this point....
        return collide;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
