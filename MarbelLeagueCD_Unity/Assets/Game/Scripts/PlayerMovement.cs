using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    string playerName;
    MaterialHolder materialHolder;
    bool boostCharging, boostCharged, boosted, gameStarted, playerFinished;
    float cooldownTime = 2f;
    float timePassed, boostTime = 2,constantSpeed =5f;
    private Renderer playerRenderer;
    private Rigidbody playerRigid;
    private ParticleSystem fireFx;
    [SerializeField]
    ParticleSystem boxExplosion;
    TMP_Text nameText;
    LevelController levelController;
    void Start()
    {
        //Materials
        materialHolder = FindObjectOfType<MaterialHolder>();
        //LevelController
        levelController = FindObjectOfType<LevelController>();
        //Renderer
        playerRenderer = GetComponent<Renderer>();
        //Rigidbody
        playerRigid = GetComponent<Rigidbody>();
        //FireFX
        fireFx = transform.GetChild(0).GetComponent<ParticleSystem>();
        //Get and Set Text
        nameText = transform.GetChild(1).GetComponent<TMP_Text>();
        nameText.SetText(playerName);
        //Bool Values
        boostCharging = true;
    }
    void Update()
    {
        if(!gameStarted)
        gameStarted = levelController.CountdownFinished();
        if(gameStarted)
        {
            playerRigid.AddForce(Vector3.down*constantSpeed);
            //BOOST COOLDOWN
            if(boostCharging)
            {
                timePassed += Time.deltaTime;
                if(timePassed>boostTime)
                {
                    NotBoosted();
                }
                if(!boostCharged)
                {
                    //Change Color Overtime
                    var lerp = Mathf.PingPong (timePassed, cooldownTime) / cooldownTime;
                    playerRenderer.material.Lerp(materialHolder.ReturnMaterial("Yellow"),materialHolder.ReturnMaterial("Red"),lerp);
                }
                if(timePassed > cooldownTime)
                {
                    playerRenderer.material = materialHolder.ReturnMaterial("Red");
                    //Bool Values
                    boostCharging =false;
                    boostCharged = true;
                    //Time Reset
                    timePassed =0;
                }
            }
            //BOOST INPUT
            if(Input.GetMouseButtonDown(0) && boostCharged)
            {
                playerRenderer.material = materialHolder.ReturnMaterial("Yellow");
                //Bool Values
                boostCharging = true;
                boostCharged = false;
                //Boosted
                Boosted();
                //Direction Calculation
                //Add force(velocity)
                playerRigid.AddForce(playerRigid.velocity*5,ForceMode.Impulse);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        //Check block collision
        if(collision.gameObject.tag == "Block")
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 dir = contact.point;
            if(boosted)
            {
                Instantiate(boxExplosion,dir,Quaternion.identity);
                Destroy(collision.gameObject);
            }
            else if(!boosted)
            {
                dir = -dir.normalized;
                playerRigid.AddForce(dir*750);
            }
        } 
        //Check ending collision
        if(collision.gameObject.tag == "Ending")
        {
            if(!playerFinished)
            levelController.AddToList(playerName);
            playerFinished = true;
            levelController.PlayerFinished();
            GetComponent<PlayerMovement>().enabled = false;
        }
    }
    void Boosted()
    {
        boosted = true;
        //FireFX
        fireFx.Play();
    }
    void NotBoosted()
    {
        boosted = false;
        //FireFX
        fireFx.Stop();
    }
}
