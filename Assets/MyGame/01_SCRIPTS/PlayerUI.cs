using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject[] heartSprite;
    public GameObject[] teddyPartSprite;
    public GameObject[] teddyPartShadowSprite;
    public GameObject[] pauseMenuTextUI;
    public GameObject[] pauseMenuselectedTextUI;

    public GameObject pauseMenu;
    public GameObject inGameInterface;

    public GameObject book;
    public GameObject optionMenu;
    public GameObject baseOption;
    public GameObject buttonChangeMenu;

    public GameObject validationButton;

    public Button[] baseButton;

    public Slider soundSlider;
    public Text soundValue;

    public GameObject winnSprite;

    public Text pressESprite;

    public int actualPageIndex;

    public bool validation;

    private void Start()
    {
        GameManager.RefreshUI += UpdateInterface;
    }

    private void Update()
    {
        if (actualPageIndex == 1)
        {
            soundValue.text = Mathf.Round(soundSlider.value*100) + "%";
            AudioManager.Instance.soundVolume = soundSlider.value;
        }
    }

    public void ChangePage(bool forward)
    {
        if (forward)
        {
            switch (actualPageIndex)
            {
                case 0:
                    optionMenu.SetActive(true);
                    book.SetActive(false);
                    actualPageIndex = 1;
                    break;
                case 1:
                    buttonChangeMenu.SetActive(true);
                    baseOption.SetActive(false);
                    actualPageIndex = 2;
                    break;
            }
        }
        else
        {

            switch (actualPageIndex)
            {
                case 0:
                    Time.timeScale = 1f;
                    GameManager.Instance.pause = false;
                    GameManager.Instance.RefreshUIActivation();
                    break;
                case 1:
                    optionMenu.SetActive(false);
                    book.SetActive(true);
                    actualPageIndex = 0;
                    break;
                case 2:
                    buttonChangeMenu.SetActive(false);
                    baseOption.SetActive(true);
                    actualPageIndex = 1;
                    break;
            }
        }
    }

    public void QuitGame(bool leave)
    {
        if (leave)
        {
            if (!validation)
            {
                validation = true;
                validationButton.SetActive(true);
                foreach (Button button in baseButton)
                {
                    button.interactable = false;
                }
            }
            else
            {
                Application.Quit();
                Debug.Log("Quit");
            }
        }
        else
        {
            validation = false;
            foreach (Button button in baseButton)
            {
                button.interactable = true;
            }
            validationButton.SetActive(false);
        }
    }

    public void UpdateInterface()
    {
        if (!GameManager.Instance.pause)
        {
            pauseMenu.SetActive(false);
            inGameInterface.SetActive(true);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            switch (GameManager.Instance.playerHP)
            {
                case 3:
                    foreach (GameObject heart in heartSprite)
                    {
                        heart.SetActive(true);
                    }
                    break;

                case 2:
                    heartSprite[2].SetActive(false);
                    break;

                case 1:
                    heartSprite[2].SetActive(false);
                    heartSprite[1].SetActive(false);
                    break;

                case 0:
                    foreach (GameObject heart in heartSprite)
                    {
                        heart.SetActive(false);
                    }
                    break;
            }


            if (GameManager.Instance.playerIsInActivableObject)
            {
                pressESprite.text = "Press E";
            }
            else
            {
                pressESprite.text = "";
            }

            if (GameManager.Instance.teddyPartsNumbers == 5)
            {
                winnSprite.SetActive(true);
            }
        }
        else
        {
            pauseMenu.SetActive(true);
            inGameInterface.SetActive(false);

            switch (GameManager.Instance.teddyPartsNumbers)
            {
                case 1:
                    teddyPartSprite[0].SetActive(true);
                    teddyPartShadowSprite[0].SetActive(false);
                    break;
                case 2:
                    teddyPartSprite[0].SetActive(true);
                    teddyPartShadowSprite[0].SetActive(false);
                    teddyPartSprite[1].SetActive(true);
                    teddyPartShadowSprite[1].SetActive(false);
                    break;
                case 3:
                    teddyPartSprite[0].SetActive(true);
                    teddyPartShadowSprite[0].SetActive(false);
                    teddyPartSprite[1].SetActive(true);
                    teddyPartShadowSprite[1].SetActive(false);
                    teddyPartSprite[2].SetActive(true);
                    teddyPartShadowSprite[2].SetActive(false);
                    break;
                case 4:
                    teddyPartSprite[0].SetActive(true);
                    teddyPartShadowSprite[0].SetActive(false);
                    teddyPartSprite[1].SetActive(true);
                    teddyPartShadowSprite[1].SetActive(false);
                    teddyPartSprite[2].SetActive(true);
                    teddyPartShadowSprite[2].SetActive(false);
                    teddyPartSprite[3].SetActive(true);
                    teddyPartShadowSprite[3].SetActive(false);
                    break;
                case 5:
                    teddyPartSprite[0].SetActive(true);
                    teddyPartShadowSprite[0].SetActive(false);
                    teddyPartSprite[1].SetActive(true);
                    teddyPartShadowSprite[1].SetActive(false);
                    teddyPartSprite[2].SetActive(true);
                    teddyPartShadowSprite[2].SetActive(false);
                    teddyPartSprite[3].SetActive(true);
                    teddyPartShadowSprite[3].SetActive(false);
                    teddyPartSprite[4].SetActive(true);
                    teddyPartShadowSprite[4].SetActive(false);
                    break;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
    ~PlayerUI()
    {
        GameManager.RefreshUI -= UpdateInterface;
    }
}
