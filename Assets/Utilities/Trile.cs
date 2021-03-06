﻿using UnityEngine;
using System.Collections;
using Blocks;

public class Trile : MonoBehaviour {

    public Shader shader;

	public GameObject Top;
	public GameObject Bottom;
	public GameObject Front;
	public GameObject Back;
	public GameObject Left;
	public GameObject Right;

    	
	// Use this for initialization
	void Start () {
		SpriteRenderer sp = this.Top.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		sp = this.Bottom.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		sp = this.Back.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		sp = this.Front.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		sp = this.Left.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

		sp = this.Right.GetComponent<SpriteRenderer> ();
		sp.receiveShadows = true;
		sp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

	}
	public void setAllSides(Sprite s){
		this.SetTop (s);
		this.SetRight (s);
		this.SetLeft (s);
		this.SetFront (s);
		this.SetBack (s);
		this.SetBottom (s);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetTop(Sprite s){
		SpriteRenderer sp = this.Top.GetComponent<SpriteRenderer> ();
		Debug.Log("SetTop Called");
		if (sp != null)
			Debug.Log("SetTop Sprite Component not null");
			sp.sprite = s;
	}
	public void SetBottom(Sprite s){
		SpriteRenderer sp = this.Bottom.GetComponent<SpriteRenderer> ();
		if (sp != null)
			sp.sprite = s;
	}
	public void SetBack(Sprite s){
		SpriteRenderer sp = this.Back.GetComponent<SpriteRenderer> ();
		if (sp != null)
			sp.sprite = s;
	}
	public void SetFront(Sprite s){
		SpriteRenderer sp = this.Front.GetComponent<SpriteRenderer> ();
		if (sp != null)
			sp.sprite = s;
	}
	public void SetLeft(Sprite s){
		SpriteRenderer sp = this.Left.GetComponent<SpriteRenderer> ();
		if (sp != null)
			sp.sprite = s;
	}
	public void SetRight(Sprite s){
		SpriteRenderer sp = this.Right.GetComponent<SpriteRenderer> ();
		if (sp != null)
			sp.sprite = s;
	}

    public void ApplyShader()
    {

        Debug.Log("Apply Shader called...");

    }

    public float Height()
    {
        if (Front == null) return 0.0f;

        return Front.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public float Width()
    {
        if (Front == null) return 0.0f;

        return Front.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public float topPosition()
    {
        Vector3 pos = transform.position;
        if (Height() == 0) return pos.y;
        return pos.y + (Height()/2);
    }

    public float bottomPosition()
    {
        Vector3 pos = transform.position;
        if (Height() == 0) return pos.y;
        return pos.y - (Height() / 2);
    }

    public float leftPosition()
    {
        Vector3 pos = transform.position;
        if (Width() == 0) return pos.x;
        return pos.x - (Width()/ 2);
    }

    public float rightPosition()
    {
        Vector3 pos = transform.position;
        if (Width() == 0) return pos.x;
        return pos.x + (Width() / 2);
    }

}



