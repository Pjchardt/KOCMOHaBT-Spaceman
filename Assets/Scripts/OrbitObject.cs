using UnityEngine;
using System.Collections;

public class OrbitObject : MonoBehaviour
{
    public GameObject Target;
    public float OrbitSpeed;
    float orbitRadius;


    private void Start()
    {
        orbitRadius = Vector3.Distance(transform.position, Target.transform.position);
    }

    private void Update()
    {

        transform.RotateAround(Target.transform.position, Vector3.forward, Time.deltaTime * OrbitSpeed);
    }
}
