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

    public bool isGrounded;

    public bool pause;
    public bool pauseSecurity;

    public float playerSpeedScale;

    private AudioSource ambientSFX;
    private AudioSource snakeEffectsSFX;
    private AudioSource playerEffectsSFX;

    private void Awake()
    {
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        ambientSFX = this.gameObject.AddComponent<AudioSource>();
        snakeEffectsSFX = this.gameObject.AddComponent<AudioSource>();
        playerEffectsSFX = this.gameObject.AddComponent<AudioSource>();
    }

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

    private void Update()
    {
        if (anim != null)
        {
            RefreshAnimation();
            RefreshSound();
        }
    }

    public void RefreshAnimation()
    {
        anim.SetFloat("Speed", playerSpeedScale);

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }

        switch (currentState)
        {
            case PlayerState.Idle:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                break;
            case PlayerState.Walking:
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                break;
            case PlayerState.Running:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                break;
            case PlayerState.Jumping:
                anim.SetBool("isJumping", true);
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
                anim.SetBool("isJumping", false);
                break;
            case PlayerState.Die:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", true);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                break;
            case PlayerState.Sliding:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", true);
                anim.SetBool("isJumping", false);
                break;
        }
    }

    public void RefreshSound()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                actualPlayerSound = SoundName.None;
                playerEffectsSFX.Stop();
                break;
            case PlayerState.Walking:
                playerEffectsSFX.Stop();
                break;
            case PlayerState.Running:
                if (actualPlayerSound != SoundName.SFX_Sprint)
                {
                    actualPlayerSound = SoundName.SFX_Sprint;
                    playerEffectsSFX.clip = (AudioClip)Resources.Load(GetSoundLoc(SoundName.SFX_Sprint));
                    playerEffectsSFX.loop = true;
                    playerEffectsSFX.volume = 1;
                    playerEffectsSFX.Play();
                }
                break;
            case PlayerState.Jumping:
                playerEffectsSFX.Stop();
                break;
            case PlayerState.Falling:
                playerEffectsSFX.Stop();
                break;
            case PlayerState.Die:
                playerEffectsSFX.Stop();
                break;
            case PlayerState.Sliding:
                playerEffectsSFX.Stop();
                break;
            default:
                playerEffectsSFX.Stop();
                actualPlayerSound = SoundName.None;
                break;
        }
    }
    public enum SoundName
    {
        None, SFX_Ambiant, SFX_Levier, SFX_Serpent_Aspiration,
        SFX_Serpent_Crachat, SFX_Serpent_PreAspiration, SFX_Serpent_PreCrachat,
        SFX_Sprint,
    }

    public SoundName actualPlayerSound;

    public string GetSoundLoc(SoundName sl)
    {
        return "SoundFX/" + sl.ToString();
    }
}
