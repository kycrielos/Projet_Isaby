using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject[] heartSprite;
    public GameObject[] heartEmptySprite;
    public void UpdateInterface()
    {
        switch (GameManager.Instance.playerHP)
        {
            case 3:
                foreach (GameObject heartEmpty in heartEmptySprite)
                {
                    heartEmpty.SetActive(false);
                }

                foreach (GameObject heart in heartSprite)
                {
                    heart.SetActive(true);
                }
                break;

            case 2:
                heartEmptySprite[2].SetActive(true);
                heartSprite[2].SetActive(false);
                break;

            case 1:
                heartEmptySprite[1].SetActive(true);
                heartSprite[1].SetActive(false);
                break;

            case 0:
                foreach (GameObject heart in heartSprite)
                {
                    heart.SetActive(false);
                }

                foreach (GameObject heartEmpty in heartEmptySprite)
                {
                    heartEmpty.SetActive(true);
                }
                break;
        }
    }
}
