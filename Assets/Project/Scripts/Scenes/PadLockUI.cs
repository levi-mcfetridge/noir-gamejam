using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class PadlockUI : MonoBehaviour
{
    public static PadlockUI Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private CanvasGroup panel;          // PadlockPanel’s CanvasGroup
    [SerializeField] private TMP_InputField[] digits;    // size = 3 (Digit0..Digit2)
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button backButton;

    [Header("Code")]
    [SerializeField] private string correctCode = "000"; // set your code here

    [Header("Behaviour")]
    [SerializeField] private bool pauseGame = true;      // freeze game while open
    [SerializeField] private RectTransform cardToShake;  // assign Card for fail shake
    [SerializeField] private float shakeStrength = 12f;
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private bool openOnStart = false;   // helper for testing

    private Action onUnlockCallback;
    private CursorLockMode prevLockMode;
    private bool prevCursorVisible;
    private bool isOpen;
    private Vector3 cardStartPos;

    private void Awake()
    {
        Instance = this;
        if (!panel) panel = GetComponent<CanvasGroup>();

        // Wire buttons
        if (unlockButton) unlockButton.onClick.AddListener(TryUnlock);
        if (backButton) backButton.onClick.AddListener(Close);

        // Per-digit behaviour
        for (int i = 0; i < digits.Length; i++)
        {
            int idx = i;
            if (!digits[i]) continue;
            digits[i].characterLimit = 1;
            digits[i].contentType = TMP_InputField.ContentType.IntegerNumber;
            digits[i].onValueChanged.AddListener(_ => OnDigitChanged(idx));
        }

        if (cardToShake) cardStartPos = cardToShake.anchoredPosition;

        // Start hidden by default
        HideImmediate();
    }


    private void Start()
    {
        if (openOnStart) Open();
    }


    public void Open(Action onUnlock = null)
    {
        onUnlockCallback = onUnlock;

        // Example: pre-fill boxes when opened
        string startingDigits = "000"; // or whatever you want to display initially
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i])
            {
                digits[i].text = (i < startingDigits.Length) ? startingDigits[i].ToString() : "";
            }
        }

        gameObject.SetActive(true);
        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;
        isOpen = true;

        if (pauseGame) Time.timeScale = 0f;
        StartCoroutine(FocusFirstNextFrame());

        // Unlock cursor, etc. (as before)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        isOpen = false;
        if (pauseGame) Time.timeScale = 1f;

        // restore cursor state
        Cursor.lockState = prevLockMode;
        Cursor.visible = prevCursorVisible;

        panel.alpha = 0f;
        panel.blocksRaycasts = false;
        panel.interactable = false;
        gameObject.SetActive(false);

        // (optional) re-enable player movement/camera
        // Player.instance.canMove = true;
    }

    private IEnumerator FocusFirstNextFrame()
    {
        yield return null; // next frame so TMP can select
        if (digits.Length > 0 && digits[0]) digits[0].ActivateInputField();
    }

    /// <summary>
    /// Called by Unlock button.
    /// </summary>
    public void TryUnlock()
    {
        string entered =
            (digits.Length > 0 && digits[0] ? digits[0].text : "") +
            (digits.Length > 1 && digits[1] ? digits[1].text : "") +
            (digits.Length > 2 && digits[2] ? digits[2].text : "");

        if (entered == correctCode)
        {
            // Success: notify caller (e.g., Lock.OnUnlocked) then close
            onUnlockCallback?.Invoke();
            Close();
        }
        else
        {
            // Feedback: shake and reset
            if (cardToShake) StartCoroutine(ShakeCard());
            foreach (var f in digits) if (f) f.text = string.Empty;
            if (digits.Length > 0 && digits[0]) digits[0].ActivateInputField();
        }
    }

    private void OnDigitChanged(int idx)
    {
        if (!digits[idx]) return;

        // auto-advance when a digit is typed
        if (digits[idx].text.Length == 1)
        {
            int next = idx + 1;
            if (next < digits.Length && digits[next])
                digits[next].ActivateInputField();
        }
    }

    private IEnumerator ShakeCard()
    {
        Vector3 start = cardStartPos;
        float t = 0f;
        while (t < shakeDuration)
        {
            float dt = Time.unscaledDeltaTime; // animate while paused
            t += dt;

            float offset = Mathf.Sin(t * 60f) * shakeStrength * (1f - t / shakeDuration);
            cardToShake.anchoredPosition = start + new Vector3(offset, 0f, 0f);

            yield return null;
        }
        cardToShake.anchoredPosition = start;
    }

    private void HideImmediate()
    {
        isOpen = false;
        if (panel)
        {
            panel.alpha = 0f;
            panel.blocksRaycasts = false;
            panel.interactable = false;
        }
        gameObject.SetActive(false);
    }
}
