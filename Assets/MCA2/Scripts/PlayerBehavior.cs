using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.contacts[0];
            Debug.Log("Contact normal Y: " + contact.normal.y);

            if (contact.normal.y > 0.5f)
            {
                collision.gameObject.GetComponent<EnemyBehavior>().DestroyEnemy();
            }
            else
            {
                LevelManager levelManager = FindAnyObjectByType<LevelManager>();
                levelManager.LevelLost();
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Firebar") || collision.gameObject.CompareTag("Deathzone"))
        {
            LevelManager levelManager = FindAnyObjectByType<LevelManager>();
            levelManager.LevelLost();
            Destroy(gameObject);
        }
    }
}
