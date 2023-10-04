using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Sweeping : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    //public float maxDisplacement;
    public Vector3 targetPosition;
    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;
    [SerializeField] public bool sweepEnabled = true;
    [SerializeField]  public float waitAtTargetTime = 0.2f;
    [SerializeField] public bool waitForPlayer = false;

    private GameObject player;
    private bool playerOnSweeper = false;
    private bool blocked = false;
    private Vector3 sweeperStartPosition;
    private RigidbodyConstraints rbConstraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    [SerializeField] private Rigidbody sweeper;

    void Start()
    {
        sweeperStartPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");

        if(freezeX)
        {
            rbConstraints = rbConstraints | RigidbodyConstraints.FreezePositionX;
        }
        if (freezeY)
        {
            sweeper.constraints = rbConstraints | RigidbodyConstraints.FreezePositionY;
        }
        if (freezeZ)
        {
            sweeper.constraints = rbConstraints | RigidbodyConstraints.FreezePositionZ;
        }
        StartCoroutine(OnTargetChange());
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForPlayer && !playerOnSweeper)
        {
            // do nothing
        }
        else
        {
            var step = speed * Time.deltaTime;
            sweeper.position = Vector3.MoveTowards(sweeper.position, targetPosition, step);
            if (playerOnSweeper && !blocked && !sweepEnabled)
            {
                Debug.Log("Moving player");
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, step);

            }
            if (Vector3.Distance(sweeper.position, targetPosition) <= .5)
            {
                (sweeperStartPosition, targetPosition) = (targetPosition, sweeperStartPosition);
                StartCoroutine(OnTargetChange());
            }
        }
        
    }

    IEnumerator OnTargetChange()
    {
        //Debug.Log("waiting " + waitAtTargetTime);
        var holdSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(waitAtTargetTime);
        //Debug.Log("done waiting");
        speed = holdSpeed;
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerOnSweeper = true;
        }
        else
        {
            blocked = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnSweeper = false;
        }
        else
        {
            blocked = false;
        }
    }

}



