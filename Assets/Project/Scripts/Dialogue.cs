using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Refs")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Typing")]
    [SerializeField] private float textSpeed = 0.03f;       // normal speed
    [SerializeField] private float fastMultiplier = 0.2f;   // while holding click
    [SerializeField] private bool pauseGame = true;

    private string[] lines;
    private int index;
    private bool isOpen;
    private Coroutine typingRoutine;

    void Awake()
    {
        Instance = this;
        HideImmediate();
    }

    void Update()
    {
        if (!isOpen) return;

        // Click logic: skip or go next
        if (Input.GetMouseButtonDown(0))
        {
            if (typingRoutine == null)  // line is fully shown
                NextLine();
            else                        // complete instantly
            {
                StopCoroutine(typingRoutine);
                text.text = lines[index];
                typingRoutine = null;
            }
        }
    }

    public void Show(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;

        lines = newLines;
        index = 0;
        isOpen = true;

        if (pauseGame) Time.timeScale = 0f;

        panel.alpha = 1f;
        panel.blocksRaycasts = true;
        panel.interactable = true;
        gameObject.SetActive(true);

        text.text = string.Empty;
        typingRoutine = StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            typingRoutine = StartCoroutine(TypeLine());
        }
        else
        {
            Hide();
        }
    }

    private IEnumerator TypeLine()
    {
        string line = lines[index];

        for (int i = 0; i < line.Length; i++)
        {
            text.text = line.Substring(0, i + 1);

            // while holding mouse, type faster
            float step = Input.GetMouseButton(0) ? textSpeed * fastMultiplier : textSpeed;

            // IMPORTANT: use unscaled time so it works while the game is paused
            yield return new WaitForSecondsRealtime(step);
        }

        typingRoutine = null;
    }

    public void Hide()
    {
        isOpen = false;
        panel.alpha = 0f;
        panel.blocksRaycasts = false;
        panel.interactable = false;
        gameObject.SetActive(false);

        if (pauseGame) Time.timeScale = 1f;
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
