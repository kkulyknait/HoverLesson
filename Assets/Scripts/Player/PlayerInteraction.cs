using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private StateController stateController;
    [SerializeField] private Transform rayOrigin;

    [Header("Sensor")]
    [SerializeField, Range(0.5f, 10f)] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayers;

    [Header("Prompt")]
    [SerializeField] private BoolVariable canInteract;
    [SerializeField] private GameEvent interactTargetChanged;

    private bool _isQuitting;


    private void OnEnable()
    {
        inputReader.InteractEvent += HandleInteract;
    }

    private void Start()
    {
        canInteract.Value = false;
        interactTargetChanged.Raise();
    }

    private void Update()
    {
        SetCanInteract(FindInteractable(out _));
    }

    private void OnDisable()
    {
        inputReader.InteractEvent -= HandleInteract;
        if (!_isQuitting) SetCanInteract(false);
    }

    private void HandleInteract()
    {
        if(FindInteractable(out IInteractable interactable))
        {
            interactable.Interact(stateController);
        }
        
    }

    private bool FindInteractable(out IInteractable interactable)
    {
        interactable = null;
        
        return Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit,
            interactRange, interactableLayers, QueryTriggerInteraction.Ignore)
            && hit.collider.TryGetComponent(out interactable);
    }

    private void SetCanInteract(bool value)
    {
        if (canInteract.Value == value) return;
        canInteract.Value = value;
        interactTargetChanged.Raise();

    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}
