using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public static AudioSource backgroundAudio;
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        backgroundAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (!target) return;
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }
}
