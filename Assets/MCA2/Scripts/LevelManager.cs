using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class LevelManager : MonoBehaviour
{
    // Below, IsPlaying is a property which is why it is instantiated using Pascal case whereas if it was a simple variable then it would have been named using camel case (Standard followed by C#)
    public static bool IsPlaying {get; private set;}
    // public static bool isPlaying;
    
    [Header("Level Settings")]
    [SerializeField] private bool isFinalLevel = false;
    public float levelTime = 15f;
    public string nextLevel;
    public string levelName;
    private float countDown;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text messageText;
    public TMP_Text levelText;
    public GameObject nextButton;
    public GameObject restartButton;

    [Header("Audio")]
    public AudioClip winSFX;
    public AudioClip loseSFX;
    
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        countDown = levelTime;
        SetScoreText(0);
        IsPlaying = true;
        DisplayLevelText();
    }

    void DisplayLevelText()
    {
        levelText.text = levelName;
        levelText.enabled = true;
        Invoke("HideLevelText", 0.5f);
    }

    void HideLevelText()
    {
        if (levelText)
        {
            levelText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            LevelTimer();
            SetTimerText();
            // SetScoreText();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                DestroyAllPickups();
            }

            // Escape key destroys all enemies
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DestroyAllEnemies();
            }

            if (PickupBehavior.pickupCount == 0 && !isFinalLevel)
            {
                // Win
                LevelBeat();
            }
            else if (PickupBehavior.pickupCount == 0 && isFinalLevel && FlagBehavior.IsFlagPlanted) {
                // Win
                LevelBeat();
            }
            else if (countDown <= 0)
            {
                // Lose
                LevelLost();
            }
        }
    }

    void DestroyAllPickups()
    {
        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            pickup.GetComponent<PickupBehavior>().DestroyPickup();
        }
    }

    void DestroyAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyBehavior>().DestroyEnemy();
        }
    }

    void LevelTimer()
    {
        countDown -= Time.deltaTime;
        
        if (countDown <= 0) countDown = 0;
        
        Debug.Log("Count down: " + countDown.ToString("0.00"));
    }

    void SetTimerText() {
        if (!timerText) return;
        timerText.text = countDown.ToString("0.00");
    }

    public void SetScoreText(int currentScore)
    {
        if (!scoreText) return;
        // scoreText.text = "Score: " + PickupBehavior.totalScore.ToString();
        scoreText.text = "Score: " + currentScore.ToString();
    }

    void LevelBeat()
    {
        IsPlaying = false;

        // Play sound effect
        PlaySoundClip(winSFX);

        if (isFinalLevel)
        {
            DisplayGameMessage("GAME COMPLETED!");
            if (restartButton) restartButton.SetActive(true);
        }
        else
        {
            DisplayGameMessage("YOU WIN!");
            if (nextLevel.Length > 0) nextButton.SetActive(true);
        }

        PickupBehavior.ResetPickups();

        // Invoke("ReloadSameScene", 5);
    }

    public void LevelLost()
    {
        IsPlaying = false;

        // Play sound effect
        PlaySoundClip(loseSFX);

        // Display message
        DisplayGameMessage("GAME OVER!");

        PickupBehavior.ResetPickups();

        Invoke("ReloadSameScene", 4);

        // ReloadSameScene();
        
        // if (sceneName.Length > 0)
        // {
        //     LoadSceneByName(sceneName);
        // }
    }

    void PlaySoundClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        CameraController.backgroundAudio.Stop();
    }

    void DisplayGameMessage(string message)
    {
        if (messageText)
        {
            messageText.text = message;
            messageText.enabled = true;
        }
    }

    void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    void ReloadSameScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadNextLevel()
    {
        if (nextLevel.Length > 0)
        {
            LoadSceneByName(nextLevel);
        }
        else {
            Debug.LogWarning("No nextLevel is specified in the inspector");
        }
    }
}
