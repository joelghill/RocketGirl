using UnityEngine;
using System.Collections;

public class CollisionType : MonoBehaviour {

	public enum ColliderType {
		None,
		Solid,
		SemiSolid,
		Moveable
	};

	public ColliderType Type;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
