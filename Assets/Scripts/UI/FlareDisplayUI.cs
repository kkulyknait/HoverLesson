using UnityEngine;
using TMPro;

public class FlareDisplayUI : MonoBehaviour
{
    [SerializeField] private IntVariable flareCount;
    [SerializeField] private TMP_Text flareText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        flareText.text = "Flares:" + flareCount.Value;
    }

   
}
