using UnityEngine;
using System.Collections;

/// <summary>
/// Looks at the tags of the parent game object.
/// Returns whether or not a collision with any given object has occured.
/// Used by "RayCollider"
/// </summary>

public class SpriteCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	bool getVertCollision(GameObject other){
		return true;
	}

	bool getHorzCollision(GameObject other){
		return true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
