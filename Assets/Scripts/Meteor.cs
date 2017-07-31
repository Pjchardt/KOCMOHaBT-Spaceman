using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    public GameObject RotateTarget;
    public float OrbitSpeed;
    float orbitRadius;

    Rigidbody2D rb;

    private void Start()
    {
        orbitRadius = Vector3.Distance(transform.position, RotateTarget.transform.position);
    }

    private void Update()
    {

        transform.RotateAround(RotateTarget.transform.position, Vector3.forward, Time.deltaTime * OrbitSpeed);
    }
}
