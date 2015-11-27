using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	private Renderer rend;
	private bool selected;
	private GameObject[] buttons;
	private bool horizontal;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		selected = false;
		buttons = GameObject.FindGameObjectsWithTag("Button");
		horizontal = false;
	}

	void OnMouseEnter() {
		//setSelected ();
		selected = true;
	}

	void OnMouseExit() {
		selected = false;
	}

	public void setSelected(int x){
		for (int i = 0; i < buttons.Length; i++) {
			if(i == x){
				buttons[i].GetComponent<MenuButton>().selected = true;
			} else {
				buttons[i].GetComponent<MenuButton>().selected = false;
			}
		}
		//selected = true;
	}

	int getSelected(){
		for (int i = 0; i < buttons.Length; i++) {
			if(buttons[i].GetComponent<MenuButton>().selected){
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

		//if (Input.GetAxis ("Vertical") < -0.1f || Input.GetAxis ("Vertical") > 0.1f && !horizontal) {
		if(Input.GetKeyDown("s")){

			print(selected.ToString());
			horizontal = true;

			if(getSelected() == -1){
				setSelected(0);
			} else if(getSelected() == 0){
				setSelected(1);
				//buttons[0].GetComponent<MenuButton>().selected = false;
			}else if(getSelected() == 1){
				setSelected(0);
				//buttons[1].GetComponent<MenuButton>().selected = false;
			}

		}

	}
}
