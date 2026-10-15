using UnityEngine;

public class FlareThrower : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private Rigidbody flarePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private IntVariable flareCount;
    [SerializeField] private GameEvent flaresChangedEvent;
    [SerializeField] private int maxFlares = 5;
    [SerializeField] private float throwForce = 8f;
    [SerializeField] private float upwardForce = 3f;
    [SerializeField] private float burnTime = 10f;

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
