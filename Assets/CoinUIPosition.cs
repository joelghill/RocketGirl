using UnityEngine;
using System.Collections;

public class CoinUIPosition : MonoBehaviour {

	public GameObject cam;
	private float camWidth;
	private float camHeight;

	// Use this for initialization
	void Start () {
		if (cam != null)
			cam = GameObject.Find ("Main Camera");
		camHeight = Camera.main.orthographicSize;
		camWidth = camHeight * Screen.width / Screen.height;
	}

	float getX(){
		return (cam.transform.position.x - camWidth+1);
	}

	float getY(){
		return (cam.transform.position.y - camHeight+1);
	}
	
	// Update is called once per frame
	void Update () {
		if (camHeight != Camera.main.orthographicSize) {
			camHeight = Camera.main.orthographicSize;
		}
		if (camWidth != camHeight * Screen.width / Screen.height) {
			camWidth = camHeight * Screen.width / Screen.height;
		}
		transform.position = new Vector3 (getX (), getY (), cam.transform.position.z + 1);
	}
}
