using UnityEngine;
using System.Collections;

public class MusicObject : MonoBehaviour
{
    AudioSource aS;

    public void StartFadeIn(AudioClip c)
    {
        aS = gameObject.AddComponent<AudioSource>();
        aS.clip = c;
        
        aS.loop = true;
        aS.Play();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float volume = 0f;

        while (volume < .65)
        {
            volume += Time.deltaTime * .1f;
            aS.volume = volume;

            yield return new WaitForEndOfFrame();
        }

    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float volume = .65f;

        while (volume > 0)
        {
            volume -= Time.deltaTime * .1f;
            aS.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        MusicManager.Instance.RemoveMusic(this);
        Destroy(this.gameObject);
    }
}
