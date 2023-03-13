using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFPS : MonoBehaviour
{

    public float timer, refresh, avgFramerate;
    public string display = "{0}";
    public TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timelapse = Time.deltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        _text.text = string.Format(display, avgFramerate.ToString());
    }
}
