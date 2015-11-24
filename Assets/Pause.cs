using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private bool isPause;

	// Use this for initialization
	void Start () {
		isPause = false;
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetKeyDown(KeyCode.Escape))
		{
			isPause = !isPause;
			if(isPause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

	}

	public Texture aTexture;

	void OnGUI() {
		if (isPause) {
			if (!aTexture) {
				Debug.LogError ("Assign a Texture in the inspector.");
				return;
			}
			GUI.DrawTexture (new Rect (10, 10, 60, 60), aTexture, ScaleMode.ScaleToFit, true, 10.0F);
		}
	}

}
