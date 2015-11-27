using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MessageBoard : MonoBehaviour {

    public Text messageText;
    public Image backgoundImage;

    public float changeTime = 0.3f;

    Color textC;
    Color imageC;

    Color imagecolorToFadeTo;
    Color textcolorfadeto;

    // Use this for initialization
    void Start () {

        textC = messageText.color;
        imageC = backgoundImage.color;

        imagecolorToFadeTo = new Color(imageC.r, imageC.b, imageC.g, 0.0f);
        textcolorfadeto = new Color(textC.r, textC.b, textC.g, 0.0f);

        messageText.CrossFadeColor(textcolorfadeto, 0.0f, true, true);
        backgoundImage.CrossFadeColor(imagecolorToFadeTo, 0.0f, true, true);
    }

    // Update is called once per frame
    void Update () {
        if (!enabled)
        {
            enabled = true;
        }
	}

    public void hide()
    {
        messageText.CrossFadeColor(textcolorfadeto, changeTime, true, true);
        backgoundImage.CrossFadeColor(imagecolorToFadeTo, changeTime, true, true);
        //messageText.enabled = false;
        //backgoundImage.enabled = false;
    }

    public void show()
    {
        messageText.CrossFadeColor(textC, changeTime, true, true);
        backgoundImage.CrossFadeColor(imageC, changeTime, true, true);
    }

}
