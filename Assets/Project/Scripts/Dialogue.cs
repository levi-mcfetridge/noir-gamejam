using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Refs")]
    [SerializeField] private CanvasGroup panel;        // parent panel to show/hide
    [SerializeField] private TextMeshProUGUI text;     // the text box

    [Header("Typing")]
    [SerializeField] private float textSpeed = 0.03f;

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

        if (Input.GetMouseButtonDown(0))
        {
            // if fully shown, go next; else complete instantly
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                if (typingRoutine != null) StopCoroutine(typingRoutine);
                text.text = lines[index];
            }
        }
    }

    // Call this to start dialogue
    public void Show(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;

        lines = newLines;
        index = 0;
        isOpen = true;

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
        foreach (char c in lines[index])
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
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
