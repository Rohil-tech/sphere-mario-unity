using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Transform target;
    public AudioClip enemySFX;
    public float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target && LevelManager.IsPlaying)
        {
            FollowTarget();
        }
    }

    void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Firebar"))
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        GetComponent<Collider>().enabled = false;
        AudioSource.PlayClipAtPoint(enemySFX, Camera.main.transform.position);
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetTrigger("enemyDestroyed");
        }
        Destroy(gameObject, 1f);
    }
}
