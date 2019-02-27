using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    public float MoveSpeed = 30;
    Rigidbody rb;
    Renderer rd;
    public Renderer box;
    public Renderer end;
    public int MoveSpeedForPC = 10;
    
    public float JumpSpeed = 50;
    public bool JumpEnable = false;
    //Vector3 FirstPosition;
    
    void Start()
    {
        //FirstPosition = Input.acceleration;
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<Renderer>();
        box.enabled = true;
        rd.enabled = true;
        JumpEnable = true;  
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * MoveSpeedForPC);
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            GameManager.Instance.TimerStart = true;
        }
        //rb.AddForce(new Vector3(Input.acceleration.x - FirstPosition.x, 0, -Input.acceleration.z + FirstPosition.z) * MoveSpeed);

        //Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && (JumpEnable == true))
        {
            rb.AddForce(Vector3.up * JumpSpeed);
            JumpEnable = false;
        }
        GameFinish();
    }
    //yer teması kontrolü
    private void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.tag == "Ground" )
        {
            JumpEnable = true;
        }
    }
    //oyun bitişi
    private void OnTriggerEnter(Collider endGame)
    {
        if (endGame.gameObject.tag == "Finish")
        {
            GameManager.Instance.IsPlaying = false;
            GameManager.Instance.EndGame = true;
            GameManager.Instance.ShowGui = true;
            GameManager.Instance.ShowBackButton = false;
            GameManager.Instance.IsNameEntered = true;
            rd.enabled = false;
            box.enabled = false;
            end.enabled = false;
        }
    }

    private void GameFinish()
    {
        if ((GameManager.Instance.EndGame) && Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.ShowScores)
        {
            GameManager.Instance.playerT.position = new Vector3(0, 2, 0);
            GameManager.Instance.TimeCounter = 0;
            GameManager.Instance.IsPlaying = true;
            GameManager.Instance.EndGame = false;
            GameManager.Instance.ShowGui = false;
            GameManager.Instance.IsNameEntered = false;
            GameManager.Instance.TimerStart = false;
            rd.enabled = true;
            box.enabled = true;
            end.enabled = true;
        }
    }

}
