using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    string enemyName;
    bool boostCharging, boostCharged, boosted, gameStarted, aiFinished;
    float cooldownTime = 2f;
    float timePassed, timePassed2, boostTime = 2,constantSpeed =5f, randomBoostTime;
    private Renderer enemyRenderer;
    private Rigidbody enemyRigid;
    private ParticleSystem fireFx;
    [SerializeField]
    ParticleSystem boxExplosion;
    TMP_Text nameText;
    LevelController levelController;
    void Start()
    {
        //Rigidbody
        enemyRigid = GetComponent<Rigidbody>();
        //FireFX
        fireFx = transform.GetChild(0).GetComponent<ParticleSystem>();
        //LevelController
        levelController = FindObjectOfType<LevelController>();
        //Bool Values
        boostCharging = true;
        //Set Enemy Name
        nameText = transform.GetChild(1).GetComponent<TMP_Text>();
        nameText.SetText(enemyName);
    }
    void Update()
    {
        if(!gameStarted)
        gameStarted = levelController.CountdownFinished();
        if(gameStarted)
        {
            enemyRigid.AddForce(Vector3.down*constantSpeed);
            timePassed2 += Time.deltaTime;
            //BOOST COOLDOWN
            if(boostCharging)
            {
                timePassed += Time.deltaTime;
                if(timePassed>boostTime)
                {
                    NotBoosted();
                }
                if(timePassed > cooldownTime)
                {
                    //Bool Values
                    boostCharging =false;
                    boostCharged = true;
                    randomBoostTime = Random.Range(5.5f,8.5f);
                    //Time Reset
                    timePassed =0;
                }
            }
            //BOOST INPUT
            if(timePassed2 > randomBoostTime && boostCharged)
            {
                //Bool Values
                boostCharging = true;
                boostCharged = false;
                timePassed2 =0f;
                //Boosted
                Boosted();
                //Direction Calculation
                //Add force(velocity)
                enemyRigid.AddForce(enemyRigid.velocity*5,ForceMode.Impulse);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
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
                enemyRigid.AddForce(dir*750);
            }
        }
        if(collision.gameObject.tag == "Ending")
        {
            if(!aiFinished)
            levelController.AddToList(enemyName);
            aiFinished = true;
            this.GetComponent<EnemyAI>().enabled = false;
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
    //Check if ai Finished
    public bool FinishedCheck()
    {
        return aiFinished;
    }
}
