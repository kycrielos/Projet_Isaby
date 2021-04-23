using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public delegate void PlayerDieEvent();
    public static event PlayerDieEvent PlayerDie;


    public void Damaged(float damage)
    {
        GameManager.Instance.playerHP -= (int)Mathf.Floor(damage);
        if (GameManager.Instance.playerHP < 0)
        {
            GameManager.Instance.playerHP = 0;
        }
        GameManager.Instance.RefreshUIActivation();

        if (GameManager.Instance.playerHP == 0)
        {
            GameManager.Instance.currentState = GameManager.PlayerState.Die;
            GameManager.Instance.RefreshAnimation();
            PlayerDieEventHandler();
        }
    }

    protected virtual void PlayerDieEventHandler()
    {
        PlayerDie?.Invoke();
    }
}
