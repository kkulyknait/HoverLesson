using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;

    [Header("Humanoid")]
    [SerializeField] private OnFootMovement onFootMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private GameObject humanoidCamera;

    [Header("Console")]
    [SerializeField] private GameObject securityCamera;
    [SerializeField] private float panSensitivity = 1f;

    public PlayerInputReader InputReader => inputReader;
    public OnFootMovement OnFootMovement => onFootMovement;
    public PlayerLook PlayerLook => playerLook;
    public GameObject HumanoidCamera => humanoidCamera;
    public GameObject SecurityCamera => securityCamera;
    public float PanSensitivity => panSensitivity;
    private BaseState _currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TransitionToState(new HumanoidState(this));
    }

    // Update is called once per frame
    void Update()
    {
        
        _currentState?.Update();
    }

    public void TransitionToState(BaseState newState)
    {
        
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
