using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public AudioSource audioS;
    public AudioSource RespondMessageAudio;
    public ParticleSystem pSystem;
    public Text text;

    Color onColor = new Color(255/255, 255/255, 150/255);
    bool activated = false;

    Player pRef;

    private void Start()
    {
        GameManager.Instance.AddInitS(this.transform);
    }

    public void SignalDetected()
    {
        RespondMessageAudio.Play();
        pSystem.Play();
        StartCoroutine(WaitToShow());
    }

    IEnumerator WaitToShow()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<SpriteRenderer>().color = onColor;
        TypewriterEffect.Instance.AddText(text.text);
        pRef.AddPower(text.text.Length);
        GameManager.Instance.AddSattelite(this.transform);
        
        audioS.Play();
    }

    public void ReceivingSignal(float delay, Player p)
    {
        if (!activated && !GameManager.Instance.SignalActivated)
        {
            activated = true;
            GameManager.Instance.SignalActivated = true;
            pRef = p;
            GameManager.Instance.Checkpoint();
            GameManager.Instance.satelliteActive = true;

            StartCoroutine(WaitToReply(delay));
        }
    }

    IEnumerator WaitToReply(float delay)
    {
        yield return new WaitForSeconds(delay);

        SignalDetected();
    }

    public void ResetActivated()
    {
        activated = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        audioS.Stop();
    }
}
