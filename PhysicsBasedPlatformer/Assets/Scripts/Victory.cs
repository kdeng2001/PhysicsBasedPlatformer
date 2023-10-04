using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    private GameObject character;
    public float winRadius;
    public LayerMask playerLayer;
    public GameObject congratsText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, winRadius);
    }

    private bool CheckPlayer()
    {
        return Physics.CheckSphere(transform.position, winRadius, playerLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (CheckPlayer())
        {
            StartCoroutine(ShowCongrats());

        }
    }

    IEnumerator ShowCongrats()
    {
        congratsText.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
