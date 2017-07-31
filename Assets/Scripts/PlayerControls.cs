using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    Player playerRef;

    public GameObject ShipForward;
    public GameObject DirectionKnob;
    public float DirectionChangeSpeed;
    float directionAngle = 0;

    bool isTyping;

    public Text InputText;
    public string currentStoryText;

    public GameObject EnginesDown;
    public ParticleSystem EnginesEffect;
    AudioSource rocketAudio;

    public GameObject ShieldsDown;
    public GameObject Shields;

    public GameObject RadioDown; 
    public ParticleSystem RadioEffect;
    public Radio RadioComp;
    public Collider2D RadioCollider;
    public float RadioDelay = 3f;
    float lastRadioSignal = 0f;

    public Image PowerMeter;

    public GameObject PressRPrompt;
    public bool showingRPromptAgain;
    public GameObject ControlsPrompt;
    bool aPressed, dPressed, wPressed, hidingControls = false;

    public AudioSource noRadio; //man I have hacked this code up so bad in the past 24 hours... this project was overly ambitious for a compo and the code is a mess as a result

    private void Awake()
    {
        playerRef = GetComponent<Player>();
        RadioCollider.enabled = false;
        rocketAudio = GetComponent<AudioSource>();
        rocketAudio.volume = 0f;

        RadioDown.SetActive(false);
        RadioCollider.enabled = false;
        RadioEffect.Stop();
    }

    private void Start()
    {
        GameManager.Instance.AddPlayerInput(this);
        SaveState();
        StartCoroutine(WaitToShowR());
    }

    IEnumerator WaitToShowR()
    {
        yield return new WaitForSeconds(6f);

        PressRPrompt.SetActive(true);
    }

    IEnumerator WaitToShowCOntrols()
    {
        yield return new WaitForSeconds(5f);
        ControlsPrompt.SetActive(true);
    }

    IEnumerator WaitToHideCOntrols()
    {
        yield return new WaitForSeconds(3f);
        ControlsPrompt.SetActive(false);
    }

    public void SaveState()
    {
        //if need to save stuff
    }

    public void ResetState()
    {
        //reload
    }

    void Update ()
    {
        if (isTyping)
            return;

        float powerUsed = 0f;

        if (GameManager.Instance.Reseting || GameManager.Instance.Paused)
        {
            if (PressRPrompt.activeSelf)
            {
                bool r = Input.GetKeyDown(KeyCode.R);
                if (r)
                {
                    PressRPrompt.SetActive(false);
                    showingRPromptAgain = true;
                    StartCoroutine(WaitToShowCOntrols());

                    RadioComp.PlayAudio();
                    RadioDown.SetActive(true);
                    RadioCollider.enabled = true;
                    RadioEffect.Play();
                    powerUsed += Time.deltaTime * 10f;
                    lastRadioSignal = Time.timeSinceLevelLoad;

                    playerRef.powerAmount -= powerUsed;
                    PowerMeter.fillAmount = playerRef.powerAmount / playerRef.maxPowerAmount;
                    GameManager.Instance.Paused = false;

                    GameManager.Instance.ShowRemainingSattelites();
                }
            }

            return;
        }

        

        if (Input.GetKey(KeyCode.A))
        {
            if (ControlsPrompt.activeSelf)
                aPressed = true;
            directionAngle += Time.deltaTime * DirectionChangeSpeed;
            //powerUsed += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (ControlsPrompt.activeSelf)
                dPressed = true;
            directionAngle -= Time.deltaTime * DirectionChangeSpeed;
            //powerUsed += Time.deltaTime;
        }

        ShipForward.transform.localRotation = Quaternion.Euler(0f, 0f, directionAngle);
        DirectionKnob.transform.localRotation = Quaternion.Euler(0f, 0f, directionAngle);

        

        //bool ShieldsActive = Input.GetKey(KeyCode.H);
        //ShieldsDown.SetActive(ShieldsActive);
        //Shields.SetActive(ShieldsActive);
        //if (ShieldsActive)
        //    powerUsed += Time.deltaTime * 5f;

        bool RadioActive = Input.GetKeyDown(KeyCode.R) && lastRadioSignal + RadioDelay < Time.timeSinceLevelLoad;
        if (RadioActive)
        {
            if (GameManager.Instance.satelliteActive)
            {
                noRadio.PlayOneShot(noRadio.clip);
            }
            else
            {
                RadioComp.PlayAudio();
                RadioDown.SetActive(true);
                RadioCollider.enabled = true;
                RadioEffect.Play();
                powerUsed += Time.deltaTime * 20f;
                lastRadioSignal = Time.timeSinceLevelLoad;

                GameManager.Instance.ShowRemainingSattelites();
            }
        }
        else if (lastRadioSignal + RadioDelay < Time.timeSinceLevelLoad)
        {
            RadioDown.SetActive(false);
            RadioCollider.enabled = false;
        }

        playerRef.powerAmount -= powerUsed;
        PowerMeter.fillAmount = playerRef.powerAmount / playerRef.maxPowerAmount;

        if (aPressed && wPressed && dPressed && !hidingControls)
        {
            hidingControls = true;
            StartCoroutine(WaitToHideCOntrols());
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.Reseting || GameManager.Instance.Paused)
        {
            return;
        }

            float powerUsed = 0f;

        bool enginesDown = Input.GetKey(KeyCode.W);
        EnginesDown.SetActive(enginesDown);
        EnginesEffect.enableEmission = enginesDown;
        if (enginesDown)
        {
            if (ControlsPrompt.activeSelf)
                wPressed = true;
            Vector2 moveVec = new Vector2(Mathf.Sin(-directionAngle * Mathf.Deg2Rad), Mathf.Cos(directionAngle * Mathf.Deg2Rad));
            playerRef.MoveShip(moveVec * 4f);
            powerUsed += Time.fixedDeltaTime * 10f;
            rocketAudio.volume = 1f;
        }
        else
        {
            rocketAudio.volume = 0f;
        }

        playerRef.powerAmount -= powerUsed;
        PowerMeter.fillAmount = playerRef.powerAmount / playerRef.maxPowerAmount;
    }

        public void Typing()
    {
        isTyping = true;
    }

    public void DoneEdit()
    {
        isTyping = false;

        //look at text and check if same as story string
        //if so, calc fuel meter based of clip length
        //start playing audio
    }
}
