using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	private Renderer rend;
	private bool selected;
	private GameObject[] buttons;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		selected = false;
		buttons = GameObject.FindGameObjectsWithTag("Button");
	}

	void OnMouseEnter() {
		setSelected ();
	}

	void OnMouseExit() {
		selected = false;
	}

	void setSelected(){
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].GetComponent<StartGameButton>().selected = false;
		}
		selected = true;
	}

	int getSelected(){
		for (int i = 0; i < buttons.Length; i++) {
			if(buttons[i].GetComponent<StartGameButton>().selected){
				return i;
			}
		}
		return -1;
	}
	
	// Update is called once per frame
	void Update () {

		if (selected) {
			rend.material.color = Color.yellow;
		} else {
			rend.material.color = Color.white;
		}

		if (Input.GetAxis ("Vertical") < -0.1f || Input.GetAxis ("Vertical") > 0.1f) {

			if(getSelected() == -1){
				buttons[0].GetComponent<StartGameButton>().setSelected();
			} else if(getSelected() == 0){
				buttons[1].GetComponent<StartGameButton>().setSelected();
			}else if(getSelected() == 1){
				buttons[0].GetComponent<StartGameButton>().setSelected();
			}

		}

	}
}
