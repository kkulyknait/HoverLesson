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
            Debug.Log("Interact hit" + hit.collider.name);
            if (hit.collider.CompareTag("SecurityConsole"))
            {
                stateController.TransitionToState(new ConsoleState(stateController));
            }
        }

    }
}
