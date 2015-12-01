using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsRemaining : MonoBehaviour {

	private Text text;
	public Text winText;
	private int coins;
	private int totalCoins;
	private AudioSource win;
	private AudioSource[] levelMusic;
	private bool soundPlayed;
	private Avatar playerAva;
	private bool gameWon;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		coins = 0;
		totalCoins = GameObject.FindGameObjectsWithTag ("Coin").Length;
		win = GetComponent<AudioSource> ();
		soundPlayed = false;
		playerAva = GameObject.FindGameObjectWithTag ("Player").GetComponent<Avatar> ();
		levelMusic = GameObject.Find ("level").GetComponents<AudioSource> ();
		gameWon = false;
	}

	public void addCoin(){
		coins = coins + 1;
	}

	void OnGUI(){
		if (gameWon && winText != null) {
			GUI.Box (new Rect ((Screen.width) / 2 - (Screen.width) / 8,
		                   (Screen.height) / 2 - (Screen.height) / 8, (Screen.width) / 4,
		                   (Screen.height) / 4), winText.text);
		}
	}

	// Update is called once per frame
	void Update () {

		if(gameWon && !win.isPlaying && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit"))){
			Application.LoadLevel(0);
		}

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
				Debug.Log(Application.levelCount);
				if(Application.levelCount - 1 > currentLevel){
					Application.LoadLevel(currentLevel+1);
				} else {
					gameWon = true;
				}
			}


		} else {
			text.text = "No Coins";
		}
	}
}
