using UnityEngine;
using System.Collections;
using Blocks;

public class Trile : MonoBehaviour {
		
	public GameObject Top;
	public GameObject Bottom;
	public GameObject Front;
	public GameObject Back;
	public GameObject Left;
	public GameObject Right;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetTop(Sprite s){
		Sprite sp = this.Top.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
	public void SetBottom(Sprite s){
		Sprite sp = this.Bottom.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
	public void SetBack(Sprite s){
		Sprite sp = this.Back.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
	public void SetFront(Sprite s){
		Sprite sp = this.Front.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
	public void SetLeft(Sprite s){
		Sprite sp = this.Left.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
	public void SetRight(Sprite s){
		Sprite sp = this.Right.GetComponent<Sprite> ();
		if (sp != null)
			sp = s;
	}
}



