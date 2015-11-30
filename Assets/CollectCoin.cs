using UnityEngine;
using System.Collections;

public class CollectCoin : MonoBehaviour {

	//This should be false if the coin starts rotated 90 from the camera
	public bool enabled = true;

	private bool collected;
	private Collider col;
	private CoinsRemaining coin;
	private AudioSource ding;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider> ();
		col.enabled = enabled;
		coin = GameObject.Find ("Coins Remaining").GetComponent<CoinsRemaining> ();
		ding = GetComponent<AudioSource> ();
		collected = false;
	}

	void OnTriggerEnter() {
		ding.Play ();
		col.enabled = false;
		collected = true;
		transform.localScale = new Vector3 (0, 0, 0);
		Destroy (gameObject, 1.0f);
		coin.addCoin ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2") && !collected) {
			col.enabled = !col.enabled;
		}
	
	}
}
