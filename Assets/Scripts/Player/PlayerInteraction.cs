using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private StateController stateController;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private Transform rayOrigin;

    private void OnEnable()
    {
        inputReader.InteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        inputReader.InteractEvent -= HandleInteract;
    }

    private void HandleInteract()
    {
        if(Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                Debug.DrawRay(rayOrigin.position, rayOrigin.forward * interactRange, Color.red, 1f);
                Debug.Log($"Ray hit {hit.collider.name}");
                interactable.Interact(stateController);
            }
        }

    }
}
