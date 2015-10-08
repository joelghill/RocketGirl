using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ResetScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void onGui(){
		Debug.Log ("Button Pressed");
	}
	public void Reset(){
		Debug.Log ("Button Pressed");
		Application.LoadLevel(Application.loadedLevel);
	}
}
