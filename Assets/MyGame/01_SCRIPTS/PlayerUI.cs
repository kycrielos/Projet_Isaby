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
    public GameObject optionMenu;

    public Slider soundSlider;
    public Text soundValue;

    public GameObject winnSprite;

    public Text pressESprite;

    private bool optionMenuIsActive;

    private void Start()
    {
        GameManager.RefreshUI += UpdateInterface;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameManager.Instance.pause = false;
        GameManager.Instance.RefreshUIActivation();
    }

    public void OpenOptions()
    {
        optionMenu.SetActive(true);
        optionMenuIsActive = true;
    }

    private void Update()
    {
        if (optionMenuIsActive)
        {
            soundValue.text = Mathf.Round(soundSlider.value*100) + "%";
            AudioManager.Instance.soundVolume = soundSlider.value;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeButton(string input)
    {
        
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
