using UnityEngine;

public class InteractPromptUI : MonoBehaviour
{
    [SerializeField] private BoolVariable canInteract;
    [SerializeField] private GameObject prompt;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        prompt.SetActive(canInteract.Value);
    }
}
