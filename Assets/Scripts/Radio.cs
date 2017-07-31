using UnityEngine;
using System.Collections;

public class Radio : MonoBehaviour
{
    AudioSource aS;
    
    public Player pRef;

    private void Awake()
    {
        aS = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        aS.Play();
    }

   void OnTriggerEnter2D (Collider2D collision)
    {
        Message m = collision.gameObject.GetComponent<Message>();

        if (m)
        {
            float dist = Vector3.Distance(transform.position, collision.gameObject.transform.position);
            m.ReceivingSignal(dist / 8f, pRef);
        }
    }
}
