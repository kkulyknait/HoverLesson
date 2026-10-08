using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class OnFootMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController _controller;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = inputReader.MoveInput;
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        if (_controller.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;  // Small negative value to keep the player grounded
        }
        _verticalVelocity += gravity * Time.deltaTime;
        Vector3 velocity = move * moveSpeed;
        velocity.y = _verticalVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }
}
