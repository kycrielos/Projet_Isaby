using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public delegate void PlayerDieEvent();
    public static event PlayerDieEvent PlayerDie; 

    public static void Damaged(float damage)
    {
        GameManager.Instance.playerHP -= (int)Mathf.Floor(damage);
        if (GameManager.Instance.playerHP <= 0)
        {
            GameManager.Instance.currentState = GameManager.PlayerState.Die;
            PlayerDieEventHandler();
            GameManager.Instance.playerHP = GameManager.Instance.maxPlayerHp;
        }
    }

    protected static void PlayerDieEventHandler()
    {
        PlayerDie?.Invoke();
    }
}
