using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public string command;

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
		for (int i = 0; i < buttons.Length; i++) {
				buttons[i].GetComponent<MenuButton>().selected = false;
		}
		selected = true;
	}

	void OnMouseExit() {
		selected = false;
	}

	void OnMouseUp(){
		if(selected)
			execute ();
	}

	public void select(){
		selected = true;
	}

	public void deselect(){
		selected = false;
	}

	public bool getSelect(){
		return selected;
	}

	void execute(){

		switch (command){
			
		case "Start":
			Application.LoadLevel(1);
			break;
			
		case "Quit":
			Application.Quit();
			break;
			
		default:
			Debug.Log("Set a vaild button command in the inspector (Start, Quit)");
			break;
			
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (selected) {
			rend.material.color = Color.yellow;
		} else {
			rend.material.color = Color.white;
		}

		if (Input.GetButtonDown("Jump") && selected){
			execute();
		}

	}
}
