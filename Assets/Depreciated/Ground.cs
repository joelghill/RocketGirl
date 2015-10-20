using UnityEngine;
using System.Collections;
using Blocks;

public class Ground : MonoBehaviour, MapComponent{
		
	public GameObject Top;
	public GameObject Bottom;
	public GameObject Front;
	public GameObject Back;
	public GameObject Left;
	public GameObject Right;

	
	public Texture2D MainTexture;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetTopMaterial(Material m){
		Renderer r = this.Top.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
	public void SetBottomMaterial(Material m){
		Renderer r = this.Bottom.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
	public void SetBackMaterial(Material m){
		Renderer r = this.Back.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
	public void SetFrontMaterial(Material m){
		Renderer r = this.Front.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
	public void SetLeftMaterial(Material m){
		Renderer r = this.Left.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
	public void SetRightMaterial(Material m){
		Renderer r = this.Right.GetComponent<Renderer> ();
		if(r != null) r.sharedMaterial = m;
	}
}



