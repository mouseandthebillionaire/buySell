using UnityEngine;

// Drop on a color preset button. Wire the Button OnClick → AdminColorPresetButton.Apply.
public class AdminColorPresetButton : MonoBehaviour {
    public int   traderIndex;
    public Color color = Color.white;

    public void Apply() {
        if (AdminSettings.S != null)
            AdminSettings.S.PickColor(traderIndex, color);
    }
}
