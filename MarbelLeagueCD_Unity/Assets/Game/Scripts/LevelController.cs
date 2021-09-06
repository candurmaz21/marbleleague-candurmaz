using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelController : MonoBehaviour
{
    [SerializeField]
    Camera startCam;
    [SerializeField]
    GameObject darkScene,clickToStartText, scoreboard, swapButton, restartButton;
    [SerializeField]
    GameObject[] standPoints;
    [SerializeField]
    ParticleSystem standPointFX;
    [SerializeField]
    TMP_Text[] scoreboardTexts;
    float countdownCount;
    bool countdownFinished, startCountdown,started;
    //List to collect names
    List<string> scoreboardList;
    void Start()
    {
        //Initial Values
        scoreboard.SetActive(false);
        swapButton.SetActive(false);
        restartButton.SetActive(false);
        scoreboardList = new List<string>();
    }
    void Update()
    {
        //Get Pressed to Start
        if(Input.GetMouseButtonDown(0) && !started)
        {
            clickToStartText.SetActive(false);
            darkScene.SetActive(false);
            startCam.gameObject.SetActive(false);
            startCountdown = true;
        }
        //Countdown Calculate
        if(startCountdown)
        countdownCount += Time.deltaTime;
        //3 Seconds delay to start
        if(countdownCount > 3 && !started)
        {
            countdownFinished = true;
            for(int i = 0; i< standPoints.Length; i++)
            {
            Instantiate(standPointFX,standPoints[i].transform.position,Quaternion.identity);
            Destroy(standPoints[i].gameObject);
            }
            started = true;
            startCountdown = false;
        }
        //Ending calculations
        if(scoreboardList.Count >4)
        {
            for(int i= 0 ; i<scoreboardList.Count ; i++)
            {
                scoreboardTexts[i].SetText((i+1)+" . "+ scoreboardList[i]);
            }
            scoreboardList.Clear();
            scoreboard.SetActive(true);
            restartButton.SetActive(true);
            swapButton.SetActive(false);
            startCam.gameObject.SetActive(true);
        }
    }
    //Send countdown finished info
    public bool CountdownFinished()
    {
        return countdownFinished;
    }
    //Add passing names to the list
    public void AddToList(string name)
    {
        scoreboardList.Add(name);
    }
    //Player finished the game
    public void PlayerFinished()
    {
        darkScene.SetActive(true);
        swapButton.SetActive(true);
    }
    //Restart game button
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
