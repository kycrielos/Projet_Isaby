using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    public static T Instance => LazyInstance.Value;

    private static T CreateSingleton()
    {
        var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}

//usage:
public class GameManager : Singleton<GameManager>
{
    public Animator anim;
    public int maxPlayerHp = 3;
    public int playerHP = 3;

    public int coinNumbers;
    public int teddyPartsNumbers;
    public bool playerIsInActivableObject;

    public bool pause;
    public bool pauseSecurity;

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling,
        Die,
        Sliding,
    }

    public delegate void RefreshUIEvent();
    public static event RefreshUIEvent RefreshUI;

    public GameObject player;
    public PlayerState currentState = PlayerState.Idle;

    public void RefreshUIActivation()
    {
        RefreshUIEventHandler();
    }
    protected virtual void RefreshUIEventHandler()
    {
        RefreshUI?.Invoke();
    }

    public void RefreshAnimation()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Walking:
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Running:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Jumping:
                anim.SetTrigger("isJumping");
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Falling:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", true);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Die:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", true);
                anim.SetBool("isSliding", false);
                break;
            case PlayerState.Sliding:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", true);
                break;
        }
    }
}
