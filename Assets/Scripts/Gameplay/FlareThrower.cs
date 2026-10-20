using UnityEngine;

public class FlareThrower : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInputReader inputReader;

    [Header("Flare")]
    [SerializeField] private Rigidbody flarePrefab;
    [SerializeField] private Transform throwPoint;
    [Tooltip("Seconds before a thrown flare is removed.")]
    [SerializeField, Range(1f, 30f)] private float burnTime = 10f;

    [Header("Throw")]
    [SerializeField, Range(0f, 20f)] private float throwForce = 8f;
    [SerializeField, Range(0f, 10f)] private float upwardForce = 3f;

    [Header("Ammo")]

    [SerializeField] private IntVariable flareCount;
    [SerializeField] private GameEvent flaresChangedEvent;
    [SerializeField, Min(0)] private int maxFlares = 5;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flareCount.Value = maxFlares;
        flaresChangedEvent.Raise();
    }

    private void OnEnable()
    {
        inputReader.FireEvent += HandleFire;
    }

    private void OnDisable()
    {
        inputReader.FireEvent -= HandleFire;
    }

    private void HandleFire()
    {
        if (flareCount.Value <= 0)
        {
            Debug.Log("No flares left!");
            return;
        }

        Rigidbody flare = Instantiate(flarePrefab, throwPoint.position, throwPoint.rotation);
        Vector3 force = throwPoint.forward * throwForce + Vector3.up * upwardForce;
        flare.AddForce(force, ForceMode.Impulse);
        Destroy(flare.gameObject, burnTime);
        flareCount.Value--;
        flaresChangedEvent.Raise();
    }

}
