using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TooFarScript : MonoBehaviour
{
    public AudioSource audioS;
    public Text text;

    bool activated = false;

    public void PlayMessage(Player pRef)
    {
        GameManager.Instance.SignalActivated = true;
        TypewriterEffect.Instance.AddText(text.text);
        pRef.AddPower(text.text.Length);
       
        audioS.Play();
    }

}
