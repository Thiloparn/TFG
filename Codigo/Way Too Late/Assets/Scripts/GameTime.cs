using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public static GameTime sharedInstance;
    public Text text;
    public float time;

    void Awake()
    {
        sharedInstance = this;
    }

    
    void Start()
    {
        time = 0;
        text.text = time.ToString();
    }

    
    void Update() {
  
        time = Time.timeSinceLevelLoad;
        text.text = Mathf.FloorToInt((time / 60) % 60).ToString("00") + " : " + Mathf.FloorToInt(time % 60).ToString("00") + " : " + Mathf.FloorToInt((time * 60) % 60).ToString("00");
    }
}
