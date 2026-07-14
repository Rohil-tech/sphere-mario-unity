using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 50f;
    public int scoreValue = 5;

    [Header("Audio")]
    public AudioClip pickupSFX;
    
    [Header("Oscillation")]
    private float amplitude = 0.15f;
    private float frequency = 2f;

    public static int pickupCount = 0;
    public static int totalScore = 0;
    
    private LevelManager levelManager;
    private Vector3 initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
        pickupCount += 1;
        Debug.Log("Pickup count from " + transform.name + " " + pickupCount);

        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Oscillatory up and down motion of the pickup coin
        Oscillate();
        Rotate();
    }

    void Oscillate()
    {
        transform.position = new Vector3(initialPosition.x, initialPosition.y + Mathf.Sin(Time.time) * frequency * amplitude, initialPosition.z);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            DestroyPickup();
        }
    }

    public void DestroyPickup()
    {
        GetComponent<Collider>().enabled = false;
        totalScore += scoreValue;
        Debug.Log("Current total score: " + totalScore);

        if (levelManager) 
        {
            levelManager.SetScoreText(totalScore);
        }

        PlayAudioEffect();

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("pickupDestroyed");

        pickupCount -= 1;
        
        Destroy(gameObject, 2);
        // Destroy(gameObject);
    }

    // void OnDestroy()
    // {
    //     pickupCount -= 1;
    //     Debug.Log("Remaining pickups: " + pickupCount);

    //     if (pickupCount <= 0)
    //     {
    //         Debug.Log("You win!");
    //         Debug.Log("Final total score: " + totalScore);
    //     }
    // }

    void PlayAudioEffect()
    {
        AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
    }

    public static void ResetPickups()
    {
        totalScore = 0;
        pickupCount = 0;
    }
}
