using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    public TextMeshProUGUI timerText;
    public GameObject gameOverText;
    public Animator animator;
    public ThirdPersonController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //Update timeText
        var minutes = (int) timeLimit / 60;
        var seconds = (int) (timeLimit - minutes * 60);
        timerText.SetText(minutes.ToString() + ":" + seconds.ToString("00"));
        if((timeLimit -= Time.deltaTime) <= 0)
        {
            timerText.SetText(minutes.ToString() + ":00");
            StartCoroutine(TimeOut());
        }
    }

    IEnumerator TimeOut()
    {
        gameOverText.SetActive(true);
        // ragdoll
        controller.enabled = false;
        animator.enabled = false;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
