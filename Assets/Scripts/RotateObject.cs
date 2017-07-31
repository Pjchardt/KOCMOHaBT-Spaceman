using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    public float RotateSpeed;
    
	void Update ()
    {
        transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
	}

}
