using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    
    public float respawnRadius;
    public LayerMask playerLayer;
    public bool respawnHere = false;
    public float deathElevation;
    public Animator characterAnim;
    public ThirdPersonController characterController;

    private GameObject character;
    private Rigidbody characterRb;

    private Vector3 respawnPosition;
    private RespawnManager rm;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        characterRb = character.GetComponent<Rigidbody>();
        rm = GetComponentInParent<RespawnManager>();
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (respawnHere)
        {
            //Debug.Log("can respawn");
            if (character.transform.position.y < deathElevation)
            {
                StartCoroutine(DeathRagdoll());
                Debug.Log("respawned");

            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, respawnRadius);
    }

    private bool CheckPlayer()
    {
        return Physics.CheckSphere(transform.position, respawnRadius, playerLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!respawnHere && CheckPlayer())
        {
            rm.SetNewSpawnPoint(this);
        }
    }

    IEnumerator DeathRagdoll()
    {
        // turn player to ragdoll
        //characterRb.isKinematic = true;
        //var velocity = characterRb.velocity;
        //characterRb.velocity = Vector3.zero;
        character.transform.position = respawnPosition;
        characterAnim.enabled = false;
        //characterController.enabled = false;

        yield return new WaitForSeconds(3f);
        Debug.Log("bruh");
        //characterRb.isKinematic = false;
        //characterRb.velocity = velocity;
        characterAnim.enabled = true;
        //characterController.enabled = true;
        // turn player back to normal

    }
}
