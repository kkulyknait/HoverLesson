using UnityEngine;

public class FlareCrate : MonoBehaviour, IInteractable
{
    [SerializeField] private IntVariable flareCount;
    [SerializeField] private GameEvent flaresChangedEvent;
    [SerializeField, Min(1)] private int refillAmount = 5;
    private bool isEmpty;

    public void Interact(StateController controller)
    {
        if (isEmpty)
        {
            Debug.Log("The crate is empty");
            return;
        }
        flareCount.Value += refillAmount;
        flaresChangedEvent.Raise();
        isEmpty = true;
    }
}
