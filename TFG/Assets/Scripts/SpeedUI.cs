using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Player player;

    private int clicks = 0;
    private float timerClicks, timerIdle;

    private void Awake()
    {
        slider1.value = 0;
        slider2.value = 0;
        slider3.value = 0;
        timerClicks = 0;
        timerIdle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isUsingShortcut && !player.animator.GetBool("IsJumping") && !player.animator.GetBool("IsSliding") && !player.animator.GetBool("IsHitted"))
        {
            timerClicks = 0;
            clicks += 1;
            if(clicks >= 3)
            {
                clicks = 0;
                if(slider1.value < 5)
                {
                    slider1.value += 1;
                }
                else if (slider2.value < 5)
                {
                    player.actualSpeed = player.speeds[1];
                    slider2.value += 1;
                } 
                else
                {
                    player.actualSpeed = player.speeds[2];
                    slider3.value += slider3.value < 5 ? 1 : 0;
                }
            }
        }

        timerClicks += Time.deltaTime;
        clicks = timerClicks >= 1 ? 0 : clicks;

        if(Mathf.Abs(player.animator.GetFloat("Speed")) < 0.01)
        {
            timerIdle += Time.deltaTime;
            if(timerIdle >= 1)
            {
                timerIdle = 0;
                if (slider3.value > 0)
                {
                    slider3.value -= 1;
                }
                else if (slider2.value > 0)
                {

                    player.actualSpeed = player.speeds[1];
                    slider2.value -= 1;
                }
                else
                {
                    player.actualSpeed = player.speeds[0];
                    slider1.value -= 1;
                }
            }
        } 
        else
        {
            timerIdle = 0;
        }
    }
}
