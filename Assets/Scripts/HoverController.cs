using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverController : MonoBehaviour
{
    [Header("Modular Input")]
    [SerializeField] private HoverInputReader inputReader;

    [Header("Hover Physics Settings")]
    [SerializeField] private float hoverHeight = 2.0f;
    [SerializeField] private float hoverForce = 65.0f;
    [SerializeField] private float damping = 3.0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float turnSpeed = 10.0f;

    [Header("Stability Settings")]
    [SerializeField] private float maxAngularVelocity = 5f;


    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleHover();
        HandleMovement();
        ClampRotation();
    }

    private void HandleHover()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, hoverHeight * 2f))
        {
            float heightError = hoverHeight - hit.distance;
            float upwardForce = (heightError * hoverForce) - (_rb.linearVelocity.y * damping);

            _rb.AddForce(transform.up * upwardForce, ForceMode.Acceleration);
        }

        Debug.DrawRay(transform.position, -transform.up * hoverHeight, Color.red);
    }

    private void HandleMovement()
    {
        Vector2 input = inputReader.MoveInput;

        _rb.AddForce(transform.forward * input.y * moveSpeed, ForceMode.Acceleration);
        _rb.AddTorque(transform.up * input.x * turnSpeed, ForceMode.Acceleration);
    }
    private void ClampRotation()
    {
        Vector3 angularVelocity = _rb.angularVelocity;
        angularVelocity.x = Mathf.Clamp(angularVelocity.x, -maxAngularVelocity, maxAngularVelocity);
        angularVelocity.z = Mathf.Clamp(angularVelocity.z, -maxAngularVelocity, maxAngularVelocity);
        _rb.angularVelocity = angularVelocity;
    }

}