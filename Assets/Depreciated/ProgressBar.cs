using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Image image = GetComponent<Image>();
        image.fillAmount = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        Image image = GetComponent<Image>();
        image.fillAmount = .7f;
        //image.fillAmount = Mathf.MoveTowards(image.fillAmount, 1f, Time.deltaTime * .5f);
	}
}
