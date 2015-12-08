using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timer : MonoBehaviour {
    public Text counterText;
    public float seconds, minutes;

    // Use this for initialization
    void Start()
    {
        counterText = GetComponent<Text>() as Text;

    }

    // Update is called once per frame
    void Update()
    {
        minutes = (int)(Time.time / 60f);
        seconds = (int)(Time.time % 60f);
        counterText.text = minutes.ToString("02") + ":" + seconds.ToString("00");
    }

    //Time.timeSinceLevelLoad

    /**void onResetClick()
    {
       Button resetButton = GetComponent<Button>();
       minutes = (int)(Time.time / 60f);
       seconds = (int)(Time.time % 60f);
       counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }**/
   
}
