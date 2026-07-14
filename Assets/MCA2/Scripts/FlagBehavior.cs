using UnityEngine;

public class FlagBehavior : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material marioFlag;
    [SerializeField] private Material enemyFlag;

    [Header("Materials")]
    [SerializeField] private GameObject flagObject;

    private Renderer flagRenderer;
    public static bool IsFlagPlanted{get; private set;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsFlagPlanted = false;

        flagRenderer = flagObject.GetComponent<Renderer>();
        if (flagRenderer)
        {
            flagRenderer.material = enemyFlag;
        }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && PickupBehavior.pickupCount == 0)
        {
            PlantFlag();
        }
    }

    void PlantFlag()
    {
        if (flagRenderer)
        {
            flagRenderer.material = marioFlag;
        }

        IsFlagPlanted = true;
    }
}
