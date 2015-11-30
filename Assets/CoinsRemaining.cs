using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsRemaining : MonoBehaviour {

	private Text text;
	private int coins;
	private int totalCoins;
	private AudioSource win;
	private AudioSource[] levelMusic;
	private bool soundPlayed;
	private Avatar playerAva;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		coins = 0;
		totalCoins = GameObject.FindGameObjectsWithTag ("Coin").Length;
		win = GetComponent<AudioSource> ();
		soundPlayed = false;
		playerAva = GameObject.FindGameObjectWithTag ("Player").GetComponent<Avatar> ();
		levelMusic = GameObject.Find ("level").GetComponents<AudioSource> ();
	}

	public void addCoin(){
		coins = coins + 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (totalCoins > 0) {

			text.text = coins.ToString () + " / " + totalCoins.ToString ();

			if (coins == totalCoins && !soundPlayed) {
				win.Play ();
				soundPlayed = true;
				playerAva.onPause ();
				playerAva.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);

				for (int i = 0; i < levelMusic.Length; i++) {
					if (levelMusic [i].isPlaying) {
						levelMusic [i].Stop ();
					}
				}

			} else if (coins == totalCoins && soundPlayed && !win.isPlaying) {
				int currentLevel = Application.loadedLevel;
				Application.LoadLevel(currentLevel+1);
			}
		} else {
			text.text = "No Coins In Level";
		}
	}
}
