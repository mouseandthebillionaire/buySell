using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// F1 admin overlay for Menu — edits GlobalVariables trader colors, rounds, and round length.
// Wire panelRoot + fields in the Inspector (or use Buy Sell → Build Admin Settings Panel).
public class AdminSettings : MonoBehaviour {

    public static AdminSettings S;
    public static bool         isOpen;

    private const string PrefNumRounds   = "AdminSettings_numRounds";
    private const string PrefRoundLength = "AdminSettings_roundLength";
    private const string PrefTraderColor = "AdminSettings_traderColor";

    [Header("UI (assign in Inspector)")]
    public GameObject  panelRoot;
    public InputField  numRoundsField;
    public InputField  roundLengthField;
    public Toggle      clearFundsToggle;
    public Image[]     colorSwatches = new Image[3];
    public Button      applyButton;
    public Button      closeButton;

    private Color[] draftColors = new Color[3];
    private bool    cursorWasVisible;

    public static readonly Color[] Presets = {
        Hex("E90000"), // Red
        Hex("F8F61D"), // Yellow
        Hex("145AFA"), // Blue
        Hex("ed7423"), // Orange
        Hex("64804f"), // Green
        Hex("111111"), // Near-black
        Hex("FFFFFF"), // White
    };

    void Awake() {
        if (S == null) S = this;
        else {
            Destroy(this);
            return;
        }
    }

    void Start() {
        // Images created without a Source Image won't render — fix any still missing
        EnsureImageSprites();
        SetOpen(false);
    }

    private void EnsureImageSprites() {
        if (panelRoot == null) return;

        Sprite fallback = null;
        foreach (Image img in panelRoot.GetComponentsInChildren<Image>(true)) {
            if (img.sprite != null) continue;
            if (fallback == null) {
                fallback = Sprite.Create(
                    Texture2D.whiteTexture,
                    new Rect(0, 0, 4, 4),
                    new Vector2(0.5f, 0.5f),
                    4f);
            }
            img.sprite = fallback;
        }
    }

    void Update() {
        if (SceneManager.GetActiveScene().name != "Menu") return;

        if (Input.GetKeyDown(KeyCode.F1))
            SetOpen(!isOpen);

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            SetOpen(false);
    }

    public static void ApplySavedPrefsToGlobals() {
        if (GlobalVariables.S == null) return;

        if (PlayerPrefs.HasKey(PrefNumRounds))
            GlobalVariables.S.numRounds = PlayerPrefs.GetInt(PrefNumRounds);
        if (PlayerPrefs.HasKey(PrefRoundLength))
            GlobalVariables.S.roundLength = PlayerPrefs.GetInt(PrefRoundLength);

        if (GlobalVariables.S.traderColors == null || GlobalVariables.S.traderColors.Length < 3)
            GlobalVariables.S.traderColors = new Color[3];

        for (int i = 0; i < 3; i++) {
            string key = PrefTraderColor + i;
            if (PlayerPrefs.HasKey(key) && ColorUtility.TryParseHtmlString(PlayerPrefs.GetString(key), out Color c))
                GlobalVariables.S.traderColors[i] = c;
        }
    }

    public void SetOpen(bool open) {
        isOpen = open;
        if (panelRoot != null) panelRoot.SetActive(open);

        if (open) {
            cursorWasVisible = Cursor.visible;
            Cursor.visible = true;
            PullFromGlobals();
        } else {
            Cursor.visible = cursorWasVisible;
        }
    }

    public void ClosePanel() {
        SetOpen(false);
    }

    public void PickColor(int traderIndex, Color color) {
        if (traderIndex < 0 || traderIndex >= draftColors.Length) return;
        draftColors[traderIndex] = color;
        if (colorSwatches != null && traderIndex < colorSwatches.Length && colorSwatches[traderIndex] != null)
            colorSwatches[traderIndex].color = color;
    }

    private void PullFromGlobals() {
        var gv = GlobalVariables.S;
        if (gv == null) return;

        if (numRoundsField != null)
            numRoundsField.text = gv.numRounds.ToString();
        if (roundLengthField != null)
            roundLengthField.text = gv.roundLength.ToString();
        if (clearFundsToggle != null)
            clearFundsToggle.isOn = false;

        for (int i = 0; i < 3; i++) {
            draftColors[i] = gv.traderColors != null && i < gv.traderColors.Length
                ? gv.traderColors[i]
                : Color.white;
            if (colorSwatches != null && i < colorSwatches.Length && colorSwatches[i] != null)
                colorSwatches[i].color = draftColors[i];
        }
    }

    public void Apply() {
        var gv = GlobalVariables.S;
        if (gv == null) return;

        if (numRoundsField != null && int.TryParse(numRoundsField.text, out int rounds) && rounds > 0)
            gv.numRounds = rounds;
        if (roundLengthField != null && int.TryParse(roundLengthField.text, out int length) && length > 0)
            gv.roundLength = length;

        if (gv.traderColors == null || gv.traderColors.Length < 3)
            gv.traderColors = new Color[3];

        for (int i = 0; i < 3; i++) {
            gv.traderColors[i] = draftColors[i];
            PlayerPrefs.SetString(PrefTraderColor + i, "#" + ColorUtility.ToHtmlStringRGBA(draftColors[i]));
        }

        PlayerPrefs.SetInt(PrefNumRounds, gv.numRounds);
        PlayerPrefs.SetInt(PrefRoundLength, gv.roundLength);
        PlayerPrefs.Save();

        if (clearFundsToggle != null && clearFundsToggle.isOn) {
            if (FundManager.S != null)
                FundManager.S.CompleteResetOfAllWorth();
            else
                ClearTraderWorthLocally();
            clearFundsToggle.isOn = false;
            RefreshMenuFundDisplays();
        }

        RefreshMenuTraderColors();
        SetOpen(false);
    }

    private void ClearTraderWorthLocally() {
        if (GlobalVariables.S.traderWorth == null) return;
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            if (i < GlobalVariables.S.traderWorth.Length)
                GlobalVariables.S.traderWorth[i] = 0;
            PlayerPrefs.SetFloat("trader_" + i, 0f);
        }
        PlayerPrefs.Save();
    }

    private void RefreshMenuFundDisplays() {
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            // Login screen labels used by the existing Ctrl+1 reset
            GameObject fundsLabeled = GameObject.Find(i + "/CurrentFunds_" + i);
            if (fundsLabeled != null) {
                Text t = fundsLabeled.GetComponent<Text>();
                if (t != null) t.text = "Current Funds: \n$0";
            }

            GameObject funds = GameObject.Find(i + "/CurrentFunds");
            if (funds != null) {
                Text t = funds.GetComponent<Text>();
                if (t != null) t.text = "TOTAL FUND VALUE: $0";
            }
        }
    }

    private void RefreshMenuTraderColors() {
        TraderLogin[] logins = FindObjectsOfType<TraderLogin>();
        foreach (TraderLogin login in logins) {
            if (!int.TryParse(login.name, out int traderNum)) continue;
            if (traderNum < 0 || traderNum >= GlobalVariables.S.traderColors.Length) continue;

            login.logged_onColor = GlobalVariables.S.traderColors[traderNum];
            Image panel = login.GetComponentInChildren<Image>();
            if (panel != null && panel.color.a > 0.6f)
                panel.color = login.logged_onColor;
        }
    }

    private static Color Hex(string hex) {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}
