using UnityEngine;
using System.Collections;

public class MenuInput : MonoBehaviour {

	private GameObject[] buttons;
	private bool canInput;

	// Use this for initialization
	void Start () {
		buttons = GameObject.FindGameObjectsWithTag("Button");
		canInput = true;
	}

	public void setSelected(int x){
		for (int i = 0; i < buttons.Length; i++) {
			if(i == x){
				buttons[i].GetComponent<MenuButton>().select();
			} else {
				buttons[i].GetComponent<MenuButton>().deselect();
			}
		}
		//selected = true;
	}
	
	public int getSelected(){
		for (int i = 0; i < buttons.Length; i++) {
			if(buttons[i].GetComponent<MenuButton>().getSelect()){
				return i;
			}
		}
		return -1;
	}

	void selectDown(){

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Vertical") < -0.1f || Input.GetAxis ("Vertical") > 0.1f && canInput) {

			//canInput = false;
			//print (canInput.ToString());
			
			if (getSelected () == -1  && canInput) {
				setSelected (0);
			} else if (getSelected () == 0 && canInput) {
				setSelected (1);
			} else if (getSelected () == 1 && canInput) {
				setSelected (0);
			}

			canInput = false;
			
		} else if (Input.GetAxis ("Vertical") >= -0.1f && Input.GetAxis ("Vertical") <= 0.1f) {
			canInput = true;
		}
	
	}
}
