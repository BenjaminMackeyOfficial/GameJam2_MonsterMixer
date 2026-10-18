using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TextBlinker : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b,math.abs(math.sin(Time.unscaledTime)));
    }
}
