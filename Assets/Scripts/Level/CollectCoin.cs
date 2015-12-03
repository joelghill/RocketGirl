using UnityEngine;
using System.Collections;

public class CollectCoin : MonoBehaviour {

	//This should be false if the coin starts rotated 90 from the camera
	public bool enabled = true;

	private bool collected;
	private Collider col;
	private CoinsRemaining coin;
	private AudioSource ding;
	private Avatar playerAva;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider> ();
		col.enabled = enabled;
		coin = GameObject.Find ("Coins Remaining").GetComponent<CoinsRemaining> ();
		ding = GetComponent<AudioSource> ();
		collected = false;
		playerAva = GameObject.FindGameObjectWithTag ("Player").GetComponent<Avatar> ();
	}

	public void OnCoinCollision(GameObject c) {

        if (c.gameObject.tag != "Player") return;

		ding.Play ();
		//Since remaining coins are counted by tag, tag must be changed from coin
		gameObject.tag = "None";
		col.enabled = false;
		collected = true;
		transform.localScale = new Vector3 (0, 0, 0);
		//Adds a second delay to destroy the object to allow the ding to play
		//This is why shrinking the transform and changing the tag are nescessary
		Destroy (gameObject, 1.0f);
		coin.addCoin ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
