using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;
    List<GravityObject> gravityObjects = new List<GravityObject>();

    public GameObject playerObject;

    private void Awake()
    {
        Instance = this;
    }

    public void AddGravityObject(GravityObject obj)
    {
        gravityObjects.Add(obj);
    }
	
    public bool RemoveGravityObject(GravityObject obj)
    {
        return gravityObjects.Remove(obj);
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
