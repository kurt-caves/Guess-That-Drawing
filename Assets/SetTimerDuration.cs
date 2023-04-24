using UnityEngine;
using UnityEngine.UI;

public class SetTimerDuration : MonoBehaviour
{
    public Timer timer;
    public InputField inputField;

    public void OnButtonClick()
    {
        float duration = float.Parse(inputField.text);
        timer.SetTimerDuration(duration);
    }
}
