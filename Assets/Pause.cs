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

}
