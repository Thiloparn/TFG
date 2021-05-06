using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsUI : MonoBehaviour
{
    public static ItemsUI sharedInstance;
    public Image image;
    public Button drink, screwdriver, notes;
    public Text drinkText, screwdriverText, notesText;
    public Text drinkTime, screwdriverTime, notesTime;
    public SpeedUI speedUI;
    public LaptopUI laptopUI;

    public bool isActive;
    private float dt, st, nt;

    void Awake()
    {
        sharedInstance = this;
    }


    void Start()
    {
        image.enabled = false;

        drink.enabled = false;
        drink.image.enabled = false;
        drinkText.enabled = false;
        drinkTime.enabled = false;
        dt = 25;

        screwdriver.enabled = false;
        screwdriver.image.enabled = false;
        screwdriverText.enabled = false;
        screwdriverTime.enabled = false;
        st = 10;

        notes.enabled = false;
        notes.image.enabled = false;
        notesText.enabled = false;
        notesTime.enabled = false;
        nt = 50;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            image.enabled = !isActive;

            drink.enabled = !isActive;
            drink.image.enabled = !isActive;
            drinkText.enabled = !isActive;
            drinkTime.enabled = !drink.interactable && drink.enabled;

            screwdriver.enabled = !isActive;
            screwdriver.image.enabled = !isActive;
            screwdriverText.enabled = !isActive;
            screwdriverTime.enabled = !screwdriver.interactable && screwdriver.enabled;

            notes.enabled = !isActive;
            notes.image.enabled = !isActive;
            notesText.enabled = !isActive;
            notesTime.enabled = !notes.interactable && notes.enabled;

            isActive = !isActive;
        }

        drinkTimer();
        screwdriverTimer();
        notesTimer();
    }

    public void useDrink()
    {
        if (speedUI.slider1.value < 5)
        {
            speedUI.slider2.value = speedUI.slider1.value;
            speedUI.slider1.value = 5;
        }
        else if (speedUI.slider2.value < 5)
        {
            Player.sharedInstance.actualSpeed = Player.sharedInstance.speeds[1];
            speedUI.slider3.value = speedUI.slider2.value;
            speedUI.slider2.value = 5;
        }
        else
        {
            Player.sharedInstance.actualSpeed = Player.sharedInstance.speeds[2];
            speedUI.slider3.value = 5;
        }

        drink.interactable = false;
        drinkTime.enabled = true;
    }

    void drinkTimer()
    {
        if (!drink.interactable)
        {
            if (dt > 0)
            {
                dt -= Time.deltaTime % 60;
            }
            else
            {
                drink.interactable = true;
                drinkTime.enabled = false;
                dt = 10;
            }

            drinkTime.text = Mathf.FloorToInt(dt).ToString();
        }
    }

    public void useScrewdriver()
    {
        laptopUI.slider.value += 3;

        screwdriver.interactable = false;
        screwdriverTime.enabled = true;
    }

    void screwdriverTimer()
    {
        if (!screwdriver.interactable)
        {
            if (st > 0)
            {
                st -= Time.deltaTime % 60;
            }
            else
            {
                screwdriver.interactable = true;
                screwdriverTime.enabled = false;
                st = 10;
            }

            screwdriverTime.text = Mathf.FloorToInt(st).ToString();
        }
    }

    public void useNotes()
    {

        notes.interactable = false;
        notesTime.enabled = true;
    }

    void notesTimer()
    {
        if (!notes.interactable)
        {
            if (nt > 0)
            {
                nt -= Time.deltaTime % 60;
            }
            else
            {
                notes.interactable = true;
                notesTime.enabled = false;
                nt = 10;
            }

            notesTime.text = Mathf.FloorToInt(nt).ToString();
        }
    }
}
