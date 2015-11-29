using UnityEngine;
using System.Collections;

public class CollectCoin : MonoBehaviour {

	//This should be false if the coin starts rotated 90 from the camera
	public bool enabled = true;

	private Collider col;
	private CoinsRemaining coin;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider> ();
		col.enabled = enabled;
		coin = GameObject.Find ("Coins Remaining").GetComponent<CoinsRemaining> ();
	}

	void OnTriggerEnter() {
		Destroy (gameObject, 0.0f);
		coin.addCoin ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2")) {
			col.enabled = !col.enabled;
		}
	
	}
}
