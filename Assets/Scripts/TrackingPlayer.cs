using UnityEngine;
using System.Collections;

public class TrackingPlayer : MonoBehaviour
{
    public static TrackingPlayer Instance;

    public GameObject player;
    public GameObject star;
    public Vector2 Extents;
    public float Speed;

    Vector3 savedPosition;
    float savedZoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SaveState();
    }

    public void SaveState()
    {
        savedPosition = transform.position;
        savedZoom = Camera.main.orthographicSize;
    }

    public void ResetState()
    {
        transform.position = savedPosition;
        Camera.main.orthographicSize = savedZoom;
    }

    public void WarpCamera(Vector3 pos)
    {
        Camera.main.transform.position = pos;
    }

    private void Update()
    {
        Vector2 cameraCenter = Camera.main.transform.position;

        Vector2 targetCameraPos = player.transform.position;
        Vector3 v = targetCameraPos - cameraCenter;

        Vector2 updatedCameraPosition = cameraCenter;
        float delta = Time.deltaTime * Speed;

        if (v.magnitude > 5)
            delta *= Mathf.Pow(v.magnitude / 5f, 1.5f);

        //if (Mathf.Abs(v.x) > Extents.x)
        {
            updatedCameraPosition.x = (1f - delta) * cameraCenter.x + (delta) * targetCameraPos.x;
        }
        //if (Mathf.Abs(v.y) > Extents.y)
        {
            updatedCameraPosition.y = (1f - delta) * cameraCenter.y + (delta) * targetCameraPos.y;
        }

        Vector2 starDist = transform.position - star.transform.position;
        float zoomFactor = 18f + Mathf.Clamp(((starDist.magnitude - 30f) * .5f), 0f, 6f);
        float lastOrthoSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = zoomFactor;
        Camera.main.transform.position = new Vector3(updatedCameraPosition.x, updatedCameraPosition.y, Camera.main.transform.position.z);
    }

}
