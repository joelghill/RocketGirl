using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsRemaining : MonoBehaviour {

	private Text text;
	private int coins;
	private int totalCoins;


	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		coins = 0;
		totalCoins = GameObject.FindGameObjectsWithTag ("Coin").Length;
	}

	public void addCoin(){
		coins = coins + 1;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = coins.ToString () + " / " + totalCoins.ToString ();
	}
}
