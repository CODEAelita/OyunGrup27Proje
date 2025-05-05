using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterManager : MonoBehaviour
{
    public enum Speaker
    {
        Mert,
        Narrator,
        Sedat,
        Yaratik
    }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextMeshProUGUI speakerUI;
    [SerializeField] private GameObject[] PauseObjects;

    [Header("Typing Settings")]
    [TextArea(3, 10)]
    public string[] lines;

    public float typingSpeed = 0.05f;
    public AudioSource typingSound;

    [Header("Speech Settings")]
    public bool isSpeech = false;
    public Speaker selectedSpeaker = Speaker.Narrator;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool lineFinished = false;

    void Start()
    {
        if (speakerUI != null) 
        {
            speakerUI.text = selectedSpeaker.ToString();
        }
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (lineFinished && Input.GetMouseButtonDown(0))
        {
            lineFinished = false;
            currentLineIndex++;

            if (currentLineIndex < lines.Length)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                textUI.text = "";

                if (speakerUI != null)
                {
                    speakerUI.text = "";
                }
                

                foreach (var obj in PauseObjects)
                {
                    obj.SetActive(true);
                }

                Destroy(gameObject);
            }
        }
    }


    IEnumerator TypeLine()
    {
        isTyping = true;
        textUI.text = "";

        if (isSpeech && typingSound != null)
        {
            typingSound.Play();
        }

        foreach (char c in lines[currentLineIndex])
        {
            textUI.text += c;

            if (!isSpeech && typingSound != null)
            {
                typingSound.Play();
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        lineFinished = true;
    }
}
