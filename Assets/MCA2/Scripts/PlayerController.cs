using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float stopSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Audio")]
    public AudioClip jumpSFX;
    
    private bool isGrounded;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.IsPlaying) return;
        Jump();
    }

    void FixedUpdate()
    {
        if (LevelManager.IsPlaying)
        {
            Move();
        }
        else
        {
            // rb.linearVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * stopSpeed);
            // rb.angularVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * stopSpeed);
        }
    }

    void Move()
    {
        
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Debug.Log("Horizontal: " + horizontal);
        // Debug.Log("Vertical: " + vertical);

        // Compute movement vector
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // Apply force
        // rb.AddForce(movement * speed);

        // Tried applying instantaneous force for better responsiveness using ForceMode parameter
        rb.AddForce(movement * speed, ForceMode.Acceleration);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlaySound();

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void PlaySound()
    {
        if (jumpSFX) 
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = jumpSFX;
            audioSource.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collided with: " + collision.transform.name);
        
        ContactPoint contact = collision.contacts[0];

        if (contact.normal.y > 0.5f)
        {
            isGrounded = true;
        }

        // Debug.Log("Collided with floor: " + contact);
        // Debug.Log("Contact position: " + contact.point);
        // Debug.Log("Contact normal: " + contact.normal);
        
    }

    // void OnCollsionStay(Collision collision)
    // {
        
    // }
}
