using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private bool isPause;
	private GameObject pauseGUI;

	// Use this for initialization
	void Start () {
		isPause = false;
		//pauseGUI = GameObject.Find ("Pause");
		//pauseGUI.transform.localScale = new Vector3 (transform.lossyScale.x, 0, transform.lossyScale.z);
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetButtonDown("Cancel"))
		{
            Debug.Log("Pause!");
			isPause = !isPause;
			if(isPause){
				//pauseGUI.transform.localScale = new Vector3 (transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
				Time.timeScale = 0;
			} else {
				//pauseGUI.transform.localScale = new Vector3 (transform.lossyScale.x, 0, transform.lossyScale.z);
				Time.timeScale = 1;
			}
		}

	}

}
