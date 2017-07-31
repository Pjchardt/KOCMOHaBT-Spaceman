using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour
{
    public AudioClip musicClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p)
        {
            TrackingPlayer.Instance.star = this.gameObject;
            MusicManager.Instance.NewMusic(musicClip);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p)
        {
            GameManager.Instance.ResetToCheckpoint();
        }
    }
}
