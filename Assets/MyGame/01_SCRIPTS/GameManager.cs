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
    public Animator snakeAnim;
    public int maxPlayerHp = 3;
    public int playerHP = 3;

    public int teddyPartsNumbers;
    public bool playerIsInActivableObject;

    public bool isGrounded;

    public bool pause;
    public bool pauseSecurity;

    public float playerSpeedScale;

    public float timeSinceGrounded;

    public GameObject followPlayer;

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling,
        Die,
        Sliding,
        Wait,
        Damaged,
        None,
    }

    public delegate void RefreshUIEvent();
    public static event RefreshUIEvent RefreshUI;

    public GameObject player;
    public PlayerState currentState = PlayerState.Idle;

    public enum SnakeState
    {
        Idle,
        PreAspiration,
        PreShoot,
        Aspiration,
        Shoot,
    }
    public SnakeState currentSnakeState = SnakeState.Idle;

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
            ActualisePlayerState();
            ActualiseSnakeState();
            ActualiseAnimation();
        }
    }

    public void ActualisePlayerState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                anim.SetBool("isWalking", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", true);
                AudioManager.Instance.StopSound("Player");
                break;
            case PlayerState.Walking:
                anim.SetBool("isWalking", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.StopSound("Player");
                break;
            case PlayerState.Running:
                anim.SetBool("isWalking", true);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.PlaySound(AudioManager.SoundName.Sprint, "Player", true);
                break;
            case PlayerState.Jumping:
                anim.SetBool("isJumping", true);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.StopSound("Player");
                break;
            case PlayerState.Falling:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", true);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.StopSound("Player");
                break;
            case PlayerState.Die:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", true);
                anim.SetBool("isSliding", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.StopSound("Player");
                break;
            case PlayerState.Sliding:
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isDying", false);
                anim.SetBool("isSliding", true);
                anim.SetBool("isJumping", false);
                anim.SetBool("idle", false);
                AudioManager.Instance.StopSound("Player");
                break;
            default:
                AudioManager.Instance.StopSound("Player");
                break;
        }
    }

    public void ActualiseSnakeState()
    {
        switch (currentSnakeState)
        {
            case SnakeState.Idle:
                AudioManager.Instance.StopSound("Snake");
                snakeAnim.SetBool("Idle", true);
                snakeAnim.SetBool("Attack", false);
                break;
            case SnakeState.PreAspiration:
                AudioManager.Instance.PlaySound(AudioManager.SoundName.Serpent_PreAspiration, "Snake", false);
                break;
            case SnakeState.PreShoot:
                AudioManager.Instance.PlaySound(AudioManager.SoundName.Serpent_PreCrachat, "Snake", false);
                snakeAnim.SetBool("Idle", false);
                snakeAnim.SetBool("Attack", true);
                break;
            case SnakeState.Aspiration:
                AudioManager.Instance.PlaySound(AudioManager.SoundName.Serpent_Aspiration, "Snake", false);
                break;
            case SnakeState.Shoot:
                AudioManager.Instance.PlaySound(AudioManager.SoundName.Serpent_Crachat, "Snake", false);
                break;
            default:
                AudioManager.Instance.StopSound("Snake");
                break;
        }
    }

    public void ActualiseAnimation()
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
    }

}

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource ambientSFX;
    private AudioSource playerEffectsSFX;
    private AudioSource snakeEffectsSFX;
    private AudioSource otherSFX;

    private float timer;

    public float soundVolume;

    private void Awake()
    {
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        ambientSFX = this.gameObject.AddComponent<AudioSource>();
        playerEffectsSFX = this.gameObject.AddComponent<AudioSource>();
        snakeEffectsSFX = this.gameObject.AddComponent<AudioSource>();
        otherSFX = this.gameObject.AddComponent<AudioSource>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void PlaySound(SoundName soundName, string soundType, bool loop)
    {
        switch (soundType)
        {
            case "Player":
                if (actualPlayerSound != soundName)
                {
                    actualPlayerSound = soundName;
                    playerEffectsSFX.clip = (AudioClip)Resources.Load(GetSoundLoc(soundName));
                    playerEffectsSFX.loop = loop;
                    playerEffectsSFX.volume = soundVolume;
                    playerEffectsSFX.Play();
                }
                break;
            case "Snake":
                if (actualSnakeSound != soundName)
                {
                    actualSnakeSound = soundName;
                    snakeEffectsSFX.clip = (AudioClip)Resources.Load(GetSoundLoc(soundName));
                    snakeEffectsSFX.loop = loop;
                    snakeEffectsSFX.volume = soundVolume;
                    snakeEffectsSFX.Play();
                }
                break;
            case "Ambient":
                break;
            case "Other":
                if (actualOtherSound != soundName)
                {
                    actualOtherSound = soundName;
                    otherSFX.clip = (AudioClip)Resources.Load(GetSoundLoc(soundName));
                    otherSFX.loop = loop;
                    otherSFX.volume = soundVolume;
                    otherSFX.Play();
                }
                break;
        }
    }

    public void StopSound(string soundType)
    {
        switch (soundType)
        {
            case "Player":
                playerEffectsSFX.Stop();
                actualPlayerSound = SoundName.None;
                break;
            case "Snake":
                snakeEffectsSFX.Stop();
                actualSnakeSound = SoundName.None;
                break;
            case "Ambient":
                break;
            case "Other":
                otherSFX.Stop();
                actualOtherSound = SoundName.None;
                break;
        }
    }

    public void PauseSound()
    {
        if (GameManager.Instance.pause)
        {
            playerEffectsSFX.Pause();
            snakeEffectsSFX.Pause();
        }
        else
        {
            playerEffectsSFX.Play();
            snakeEffectsSFX.Play();
        }
    }

    public enum SoundName
    {
        None, Ambiant, Levier, Serpent_Aspiration,
        Serpent_Crachat, Serpent_PreAspiration, Serpent_PreCrachat,
        Sprint, Ouverture_Porte, Cristal_Rotation,
    }

    public SoundName actualPlayerSound;
    public SoundName actualSnakeSound;
    public SoundName actualOtherSound;
    public string GetSoundLoc(SoundName sl)
    {
        return "SoundFX/SFX_" + sl.ToString();
    }

}

public class MyInputManager : Singleton<MyInputManager>
{
    private float rawX;
    private float rawY;

    public Dictionary<string, KeyCode> keyMapping; static string[] keyMaps = new string[7]
     {
        "Forward",
        "Backward",
        "Left",
        "Right",
        "Jump",
        "Interaction",
        "Sprint"
     };

    public KeyCode[] defaults = new KeyCode[7]
    {
        KeyCode.Z,
        KeyCode.S,
        KeyCode.Q,
        KeyCode.D,
        KeyCode.Space,
        KeyCode.E,
        KeyCode.LeftShift
    };

    public MyInputManager()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
        {
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        }
        keyMapping[keyMap] = key;
    }

    public bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }

    public bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }

    public float GetAxisRaw(string axis)
    {
        if (axis == "Horizontal")
        {
            if (GetKey("Right"))
            {
                if (rawX < 0)
                {
                    rawX = 0;
                }
                else if (rawX < 1)
                {
                    rawX += Time.deltaTime * 25;
                }
                else
                {
                    rawX = 1;
                }
            }
            else if (GetKey("Left"))
            {
                if (rawX > 0)
                {
                    rawX = 0;
                }
                else if (rawX > -1)
                {
                    rawX -= Time.deltaTime * 25;
                }
                else
                {
                    rawX = -1;
                }
            }
            else
            {
                rawX = 0;
            }
            return rawX;
        }
        else if (axis == "Vertical")
        {
            if (GetKey("Forward"))
            {
                if (rawY < 0)
                {
                    rawY = 0;
                }
                else if (rawY < 1)
                {
                    rawY += Time.deltaTime * 25;
                }
                else
                {
                    rawY = 1;
                }
            }
            else if (GetKey("Backward"))
            {
                if (rawY > 0)
                {
                    rawY = 0;
                }
                else if (rawY > -1)
                {
                    rawY -= Time.deltaTime * 25;
                }
                else
                {
                    rawY = -1;
                }
            }
            else
            {
                rawY = 0;
            }
            return rawY;
        }
        else
        {
            return 0;
        }
    }

    public float GetAxis(string axis)
    {
        if (axis == "Horizontal")
        {
            if (GetKey("Right"))
            {
                return 1;
            }
            else if (GetKey("Left"))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else if (axis == "Vertical")
        {
            if (GetKey("Forward"))
            {
                return 1;
            }
            else if (GetKey("Backward"))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}
