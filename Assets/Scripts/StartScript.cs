using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public Text StartText;
    bool flashing = false;

    float timeElapsed = 0f;
    bool loading = false;

    public Image FadeImage;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (loading)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            loading = true;
            StartText.text = "KOCMOHaBT Spaceman";
            FadeOut(2f);
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 1)
            {
                timeElapsed -= 1;
                if (flashing)
                {
                    StartText.text = "KOCMOHaBT Spaceman\nSpace to start";
                }
                else
                {
                    StartText.text = "KOCMOHaBT Spaceman";
                }
                flashing = !flashing;
            }
        }
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
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        c = FadeImage.color;
        c.a = 1f;
        FadeImage.color = c;

        SceneManager.LoadScene("Scene_1");
    }
}
