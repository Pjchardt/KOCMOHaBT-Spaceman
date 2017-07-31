using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour {

    public Text StartText;

    float timeElapsed = 0f;
    bool loading = false;

    public Image FadeImage;

    private void Start()
    {
        StartCoroutine(DoStuff());
    }

    IEnumerator DoStuff()
    {
        FadeIn(2f);
        yield return new WaitForSeconds(10f);
        FadeOut(2f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Start");
    }

    public void FadeIn(float t)
    {
        StartCoroutine(DoFadeIn(t));
    }

    IEnumerator DoFadeIn(float t)
    {
        float time = 1;
        float speed = 1f / t;
        Color c;

        while (time > 0)
        {
            c = FadeImage.color;
            c.a = time;
            FadeImage.color = c;
            time -= Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        c = FadeImage.color;
        c.a = 0f;
        FadeImage.color = c;
    }

    public void FadeOut(float t)
    {
        StartCoroutine(DoFadeOut(t));
    }

    IEnumerator DoFadeOut(float t)
    {
        float time = 0;
        float speed = 1f / t;
        Color c;

        while (time < 1)
        {
            c = FadeImage.color;
            c.a = time;
            FadeImage.color = c;
            time += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        c = FadeImage.color;
        c.a = 1f;
        FadeImage.color = c;
    }
}
