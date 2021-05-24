using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public delegate void PlayerDieEvent();
    public static event PlayerDieEvent PlayerDie;

    public void Damaged(float damage)
    {
        if (GameManager.Instance.currentState != GameManager.PlayerState.Damaged)
        {
            GameManager.Instance.playerHP -= (int)Mathf.Floor(damage);
        }

        if (GameManager.Instance.playerHP < 0)
        {
            GameManager.Instance.playerHP = 0;
        }
        GameManager.Instance.RefreshUIActivation();

        if (damage  >= 1)
        {
            StartCoroutine(damageDelay());
        }

    }

    IEnumerator damageDelay()
    {
        GameManager.Instance.currentState = GameManager.PlayerState.Damaged;
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.currentState = GameManager.PlayerState.Idle;
        if (GameManager.Instance.playerHP == 0)
        {
            GameManager.Instance.currentState = GameManager.PlayerState.Die;
            PlayerDieEventHandler();
        }

    }

    protected virtual void PlayerDieEventHandler()
    {
        PlayerDie?.Invoke();
    }
}
