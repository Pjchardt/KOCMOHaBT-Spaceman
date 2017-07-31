using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public enum PlayerState { None, Input, Movement, OutOfPower, Typing};
    PlayerState currentState;

    public Rigidbody2D[] SpringRigidBodies;
    Rigidbody2D rb;

    public float powerAmount;
    public float maxPowerAmount;
    float savedPower;



    private void Awake()
    {
        currentState = PlayerState.None;
        rb = GetComponent<Rigidbody2D>();
       
    }
   
    void Start ()
    {
        GameManager.Instance.AddPlayer(this);
        //SaveState();
    }

    public void AddPower(float amount)
    {
        powerAmount += amount;
        maxPowerAmount = powerAmount;
    }

    public void TakeDamage(float amount)
    {
        powerAmount -= amount;
    }

	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(powerAmount);
        if (powerAmount <= 0 && !GameManager.Instance.Reseting)
        {
            powerAmount = 0;
            GameManager.Instance.ResetToCheckpoint();
            TypewriterEffect.Instance.UpdateText(Mathf.FloorToInt(powerAmount));
        }
        else
        {
            TypewriterEffect.Instance.UpdateText(Mathf.FloorToInt(powerAmount));
        }
	}

    public void MoveShip(Vector2 v)
    {
        for (int i = 0; i < SpringRigidBodies.Length; i++)
        {
            SpringRigidBodies[i].AddForce(v);
        }
        //rb.AddForce(v);
    }

    public void SaveState()
    {
        savedPower = powerAmount;
    }

    public void ResetState()
    {
        powerAmount = savedPower;
    }
}
