using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    public GameObject Star;
    public float OrbitSpeed;
    float orbitRadius;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        orbitRadius = Vector3.Distance(transform.position, Star.transform.position);
    }

    private void Update()
    {
        
        transform.RotateAround(Star.transform.position, Vector3.forward, Time.deltaTime * OrbitSpeed);
    }
}
