using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    List<GravityObject> gSaveObjects = new List<GravityObject>();
    Player playerRef;
    PlayerControls playerControlsRef;

    public Text StartText;

    public Image FadeImage;

    public bool Reseting = false;
    public bool Paused = false;
    public bool Ending = false;

    public LineRenderer l;
    List<Transform> satellites = new List<Transform>();
    List<Transform> satellitesSinceCheckpoint = new List<Transform>();
    List<Transform> allSatellites = new List<Transform>();

    public bool SignalActivated;

    public GameObject StartStar;
    public GameObject FinalStar;

    bool playedToFarAudio = false;
    public TooFarScript tooFarComp;

    public GameObject OptionsMenu;
    public GameObject OptionsPrompt;

    public bool satelliteActive = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        Color c = FadeImage.color;
        c.a = 1f;
        FadeImage.color = c;
        TypewriterEffect.Instance.AddText(StartText.text);
        playerRef.AddPower(StartText.text.Length);
        playerRef.SaveState();
        Paused = true;
        StartCoroutine(WaitToStartUnpause());
    }

    IEnumerator WaitToStartUnpause()
    {
        yield return new WaitForSeconds(.05f);

        //do stuff
        FadeIn(3f);
    }


    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        //for testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Checkpoint();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            //ResetToCheckpoint();
        }
        //else if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ResetGame();
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsMenu.activeSelf)
            {
                OptionsMenu.SetActive(false);
            }
            else
            {
                OptionsMenu.SetActive(true);
            }

            OptionsPrompt.SetActive(!OptionsMenu.activeSelf);
        }

        if (OptionsMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }

        int count = satellites.Count * 2 + 1;
       
        l.SetVertexCount(count);
        for (int i = 0; i < satellites.Count; i++)
        {
            l.SetPosition(i * 2, transform.position);
            l.SetPosition((i * 2) + 1, satellites[i].position);
        }
        l.SetPosition(count-1, transform.position);

        float dist = Vector3.Distance(playerRef.transform.position, Vector3.zero);
        if (dist > 500  && !playedToFarAudio && !GameManager.Instance.SignalActivated)
        {
            tooFarComp.PlayMessage(playerRef);
            ResetToCheckpoint();
            playedToFarAudio = true;
        }
        
    }

    public void Checkpoint()
    {
        satellitesSinceCheckpoint.Clear();

        for (int i = 0; i < gSaveObjects.Count; i++)
        {
            gSaveObjects[i].SaveState();
        }

        TrackingPlayer.Instance.SaveState();
        TypewriterEffect.Instance.SaveState();
        playerRef.SaveState();
        playerControlsRef.SaveState();
    }

    public void ResetToCheckpoint()
    {
        if (!Reseting)
            StartCoroutine(WaitToReset());
    }

    public void EndGame()
    {
        if (!Ending)
        {
            Ending = true;
            Paused = true;
            StartCoroutine(WaitToEnd());
        }
    }

    IEnumerator WaitToEnd()
    {
        //fade out
        FadeOut(2f);

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("End");
    }

    IEnumerator WaitToReset()
    {
        //fade out
        FadeOut(1f);
        Reseting = true;

        yield return new WaitForSecondsRealtime(3f);

        int poop = satellitesSinceCheckpoint.Count;
        int doop = satellites.Count;

        for (int i = 0; i < satellitesSinceCheckpoint.Count; i++)
        {
            satellites.Remove(satellitesSinceCheckpoint[i]);
            satellitesSinceCheckpoint[i].GetComponent<Message>().ResetActivated();
        }

        satellitesSinceCheckpoint.Clear();

        for (int i = 0; i < gSaveObjects.Count; i++)
        {
            gSaveObjects[i].ResetState();
        }

        TrackingPlayer.Instance.ResetState();
        TypewriterEffect.Instance.ResetState();
        playerRef.ResetState();
        playerControlsRef.ResetState();

        FadeIn(1f);
        Reseting = false;
    }

	public void AddGravityObject(GravityObject g)
    {
        gSaveObjects.Add(g);
    }

    public bool RemoveGravityObject(GravityObject g)
    {
        return gSaveObjects.Remove(g);
    }

    public void AddPlayer(Player p)
    {
        playerRef = p;
    }

    public void AddPlayerInput(PlayerControls pC)
    {
        playerControlsRef = pC;
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

    public void AddSattelite(Transform t)
    {
        satellites.Add(t);
        satellitesSinceCheckpoint.Add(t);

        if (satellites.Count == 10)
        {
            //turn on final star
            StartStar.SetActive(false);
            FinalStar.SetActive(true);
            //turn line renderer red
            
            l.SetColors(Color.yellow, Color.yellow);
            Checkpoint();
        }
    }

    public void AddInitS(Transform t)
    {
        allSatellites.Add(t);
    }

    public GameObject SpawnObject;

    public void ShowRemainingSattelites()
    {
        Debug.Log(allSatellites.Count);
        for (int i = 0; i < allSatellites.Count; i++)
        {
            if (!satellites.Contains(allSatellites[i]) && !satellitesSinceCheckpoint.Contains(allSatellites[i]))
            {
                //show on map
                Vector3 d = allSatellites[i].transform.position - playerRef.transform.position;
                GameObject temp = Instantiate(SpawnObject, playerRef.transform.position + d.normalized * 3f, Quaternion.identity) as GameObject;
                Destroy(temp, 3f);
            }
            
        }
    }
}
