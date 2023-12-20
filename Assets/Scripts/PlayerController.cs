using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winText;

    public float timeLeft;
    // Start is called before the first frame update
    private Rigidbody rb;
    public GameObject player;
    public GameObject ramp;
    private int count;
    private float movementX;
    private float movementY;
    private Vector3 jump  = new Vector3(0f,2f,0f);
    public bool onGround;
    public AudioSource pickUp;
    public AudioSource grow;
    public AudioSource shrink;
    public AudioSource rez;
    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;
        timeLeft = 101;
        SetCountText();
        timerText.text = "Time Left: ";
        winText.enabled = false;
        ramp.SetActive(false);
    }
    private void FixedUpdate(){
        Vector3 movement = new Vector3 (movementX,0.0f, movementY);
        rb.AddForce(movement * speed);
    }
    // Update is called once per frame
    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x; 
        movementY = movementVector.y;
    }
    void Slow(){
        if(Input.GetKeyDown(KeyCode.U)){
            movementX = 0; 
            movementY = 0;
        }
    }
    void OnTriggerEnter(Collider other){
      if (other.gameObject.CompareTag("PickUp")){
        other.gameObject.SetActive(false);
        count = count + 1;
        pickUp.Play();
        SetCountText();
       }   
        
    }
    void Respawn(){
        if(player.transform.position.y < -20 || Input.GetKeyDown(KeyCode.R)){
            player.transform.position = new Vector3(0,1,0);
            rez.Play();
        }

    }
     void SetCountText(){
        countText.text = "Count: " + count.ToString();
        if(count >= 8){
            ramp.SetActive(true);
        }
        if(count >= 16){
            winText.enabled = true;
            Time.timeScale = 0;
        }
    }
    void sizeChange(){
        if(Input.GetKeyDown(KeyCode.T)){
            player.transform.localScale =  player.transform.localScale*2;
            grow.Play();
        }else if(Input.GetKeyDown(KeyCode.Y)){
            player.transform.localScale =  player.transform.localScale/2;
            shrink.Play();
        }
    }
    void Reset(){
        if(Input.GetKeyDown(KeyCode.E)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }
    void OnCollisionStay(){
        onGround = true;
    }
    void OnCollisionExit(){
        onGround = false;
    }
    void Update()
    {
       Slow();
       Reset();
       Respawn();
       sizeChange();
       if(Input.GetKey(KeyCode.LeftShift)){
            speed = 25f;
        }else{
            speed = 10f;
        }
        
        if(Input.GetKeyDown("space") && onGround == true){
            rb.AddForce(jump * 5.0f, ForceMode.Impulse);
            onGround = false;
        }
        
        if(timeLeft > 0){
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Left: " + ((int)timeLeft).ToString();
        }else{
            winText.text = "You Lose";
            winText.enabled = true;
            Time.timeScale = 0;
        }
    }
}
