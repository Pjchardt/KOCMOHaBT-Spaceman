using UnityEngine;
using System.Collections;

public class FinalStar : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();

        if (p)
        {
            GameManager.Instance.EndGame();
        }
    }
}
