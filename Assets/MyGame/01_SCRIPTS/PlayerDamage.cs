using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int maxplayerHP;
    public int playerHP;

    public delegate void PlayerDieEvent();

    public static event PlayerDieEvent PlayerDie;
    private void Start()
    {
        playerHP = maxplayerHP;
    }
    public void Damaged(float damage)
    {
        playerHP -= (int)Mathf.Floor(damage);
        if (playerHP <= 0)
        {
            PlayerDieEventHandler();
            playerHP = maxplayerHP;
        }
    }

    protected virtual void PlayerDieEventHandler()
    {
        PlayerDie?.Invoke();
    }
}
