using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour {

    public Text messageText;
    public Image backgoundImage;

    public float changeTime = 0.3f;
    public float waitTime = 3;
    public bool fadeIn = false;

    public AudioSource music;

    public GameObject player;

    Color textC;
    Color imageC;

    Color imagecolorToFadeTo;
    Color textcolorfadeto;

    // Use this for initialization
    void Start()
    {

        textC = messageText.color;
        imageC = backgoundImage.color;
        if(music != null)
        {
     
        }
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(waitTime);
        fadeIn = true;
    }

    IEnumerator WaitForEnd()
    {
        show();
        yield return new WaitForSeconds(waitTime+1);
        Application.LoadLevel(Application.loadedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
        {
            enabled = true;
        }

        if (fadeIn)
        {
            fadeIn = false;
            hide();
        }  
        
        if(player != null)
        {
            if(player.transform.position.y < -5.0f)
            {
                StartCoroutine(WaitForEnd());
            }
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

    private void playMusic()
    {

    }

    private void playWhiteNoise()
    {

    }
}
