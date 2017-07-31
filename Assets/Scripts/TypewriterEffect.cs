using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public static TypewriterEffect Instance;

    string fullText = "";
    string typeText;
    Text uiText;
    int textLength = 0;
    int startPosition = 0;

    public Player playerRef;

    string savedFullText;
    string savedUIText;

    public bool showingText;

    private void Awake()
    {
        Instance = this;
        uiText = GetComponent<Text>();
    }

    void Start()
    {
        playerRef.AddPower(uiText.text.Length);
        fullText = uiText.text;
        SaveState();
    }

    public void SaveState()
    {
        savedFullText = uiText.text;
        savedUIText = uiText.text;
    }

    public void ResetState()
    {
        fullText = savedFullText;
        uiText.text = savedUIText;
    }

    public void UpdateText(int textNum)
    {
        int diff = fullText.Length - textNum;
        

        fullText = fullText.Substring(diff, fullText.Length - diff);
        if (!showingText)
            uiText.text = fullText;
    }

    public void AddText(string s)
    {
        startPosition = uiText.text.Length;
        typeText = uiText.text;
        fullText = uiText.text + "\n" + s;
        textLength = startPosition;
        showingText = true;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        //Debug.Log(textLength + " : " + fullText.Length);
        while (textLength < fullText.Length)
        {
            typeText = fullText.Substring(0, textLength);
            //Debug.Log(typeText);
            if (textLength % 6 < 3)
                uiText.text = typeText + "|";
            else
                uiText.text = typeText + " ";
            textLength++;
            yield return new WaitForSeconds(.075f);
        }

        showingText = false;
        GameManager.Instance.satelliteActive = false;
        GameManager.Instance.SignalActivated = false;
        uiText.text = fullText;
    }

    public void NewText(string s)
    {

    }
}
