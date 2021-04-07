using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject[] heartSprite;
    public GameObject[] teddyPartSprite;
    public GameObject[] teddyPartShadowSprite;

    public GameObject winnSprite;

    private void Start()
    {
        GameManager.RefreshUI += UpdateInterface;
    }
    public void UpdateInterface()
    {
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

        switch (GameManager.Instance.teddyPartsNumbers)
        {
            case 1:
                teddyPartSprite[0].SetActive(true);
                teddyPartShadowSprite[0].SetActive(false);
                break;
            case 2:
                teddyPartSprite[1].SetActive(true);
                teddyPartShadowSprite[1].SetActive(false);
                break;
            case 3:
                teddyPartSprite[2].SetActive(true);
                teddyPartShadowSprite[2].SetActive(false);
                break;
            case 4:
                teddyPartSprite[3].SetActive(true);
                teddyPartShadowSprite[3].SetActive(false);
                break;
            case 5:
                teddyPartSprite[4].SetActive(true);
                teddyPartShadowSprite[4].SetActive(false);
                winnSprite.SetActive(true);
                break;
        }
    }

    ~PlayerUI()
    {
        GameManager.RefreshUI -= UpdateInterface;
    }
}
