using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private bool isPause;
	private GameObject pauseGUI;
	private float width;
	private float height;

	// Use this for initialization
	void Start () {
		isPause = false;
		pauseGUI = GameObject.Find ("Pause");
		width = pauseGUI.transform.localScale.x;
		height = pauseGUI.transform.localScale.y;
		pauseGUI.transform.localScale = new Vector3 (transform.lossyScale.x, 0, transform.lossyScale.z);
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit"))
		{
			isPause = !isPause;
			if(isPause){
				pauseGUI.transform.localScale = new Vector3 (width, height, transform.lossyScale.z);
				Time.timeScale = 0;
			} else {
				pauseGUI.transform.localScale = new Vector3 (0, 0, transform.lossyScale.z);
				Time.timeScale = 1;
			}
		}

	}

}
