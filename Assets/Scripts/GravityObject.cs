using UnityEngine;
using System.Collections;

public class GravityObject : MonoBehaviour
{
    public bool influences;
    public bool isInfluenced;

    public float InitRadius = 1f;
    public float noRigidBodyMass = 1f;
    Rigidbody2D rb;

    Vector3 savedPos;
    Quaternion savedRot;
    Vector3 savedVel;
    float savedAngVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.AddGravityObject(this);
        SaveState();
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveGravityObject(this);
    }

    public void ApplyGravity(Vector2 g)
    {
        rb.velocity += g * Time.fixedDeltaTime;
        //Debug.Log("adding grazvity");
    }

    private void OnTriggerStay2D (Collider2D collision)
    {
        if (!influences)
            return;

        GravityObject obj = collision.gameObject.GetComponent<GravityObject>();

        if (obj)
        {
            if (obj.isInfluenced)
            {
                Vector2 v = transform.position - obj.gameObject.transform.position;
                float force = InitRadius / Mathf.Pow(v.magnitude, 2f);

                float mass;
                if (rb)
                    mass = rb.mass;
                else
                    mass = noRigidBodyMass;
                obj.ApplyGravity(v.normalized * force * mass);
            }
        }
    }

    public void SaveState()
    {
        savedPos = transform.localPosition;
        savedRot = transform.localRotation;
        if (rb)
        {
            savedVel = rb.velocity;
            savedAngVel = rb.angularVelocity;
        }
    }

    public void ResetState()
    {
        transform.localPosition = savedPos;
        transform.localRotation = savedRot;
        if (rb)
        {
            rb.velocity = savedVel;
            rb.angularVelocity = savedAngVel;
        }
    }
}
