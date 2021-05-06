using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    public static SpeedUI sharedInstance;
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Player player;

    private int clicks = 0;
    private float timerClicks = 0, timerBrake = 0;

    private void Awake()
    {
        sharedInstance = this;
        slider1.value = 0;
        slider2.value = 0;
        slider3.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isUsingShortcut && !player.animator.GetBool("IsJumping") && !player.animator.GetBool("IsSliding") && !player.animator.GetBool("IsHitted") && !ItemsUI.sharedInstance.isActive)
        {
            timerClicks = 0;
            clicks += 1;
            if(clicks >= 3 && slider3.value < 5)
            {
                clicks = 0;
                if(slider1.value < 5)
                {
                    player.slidingTime = 5;
                    slider1.value += 1;
                }
                else if (slider2.value < 5)
                {
                    player.slidingTime = 10;
                    player.actualSpeed = player.speeds[1];
                    slider2.value += 1;
                } 
                else
                {
                    player.slidingTime = 15;
                    player.actualSpeed = player.speeds[2];
                    slider3.value +=  1;
                }
            }
        }

        timerClicks += Time.deltaTime;
        clicks = timerClicks >= 1 ? 0 : clicks;

        if (Mathf.Abs(player.animator.GetFloat("Speed")) < 0.01 || player.animator.GetBool("IsSliding"))
        {
            timerBrake += Time.deltaTime;
            if(timerBrake >= 1 && slider1.value > 0)
            {
                timerBrake = 0;
                if (slider3.value > 0)
                {
                    player.slidingTime = 15;
                    slider3.value -= 1;
                }
                else if (slider2.value > 0)
                {
                    player.slidingTime = 10;
                    player.actualSpeed = player.speeds[1];
                    slider2.value -= 1;
                }
                else
                {
                    player.slidingTime = 5;
                    player.actualSpeed = player.speeds[0];
                    slider1.value -= 1;
                }

            }
        } 
        else
        {

            timerBrake = 0;
        }
    }

    public void obstacleHitted()
    {
        if (slider3.value > 0)
        {
            if(slider3.value > 1)
            {
                slider3.value -= 2;
            }
            else
            {
                player.actualSpeed = player.speeds[1];
                slider3.value -= 1;
                slider2.value -= 1;
            }
            player.slidingTime = 15;
        }
        else if (slider2.value > 0)
        {
            if (slider2.value > 1)
            {
                player.actualSpeed = player.speeds[1];
                slider2.value -= 2;
            }
            else
            {
                player.actualSpeed = player.speeds[0];
                slider2.value -= 1;
                slider1.value -= 1;
            }
            player.slidingTime = 10;
        }
        else
        {
            slider1.value = slider1.value > 1 ? slider1.value - 2 : 0;
            player.slidingTime = 5;
        }
    }
}
