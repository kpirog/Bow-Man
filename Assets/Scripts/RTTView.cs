using TMPro;
using UnityEngine;

public class RTTView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rttText;

    private void Update()
    {
        rttText.text = $"RTT = {RTTest.RTTime}";
    }
}
