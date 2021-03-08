using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public delegate void PlayerDieEvent();
    public static event PlayerDieEvent PlayerDie;
    private PlayerUI playerUI;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        playerUI.UpdateInterface();
    }

    public void Damaged(float damage)
    {
        GameManager.Instance.playerHP -= (int)Mathf.Floor(damage);
        playerUI.UpdateInterface();
        if (GameManager.Instance.playerHP <= 0)
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
