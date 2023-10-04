using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakHinge : MonoBehaviour
{
    public HingeJoint weakHinge;
    public bool trapEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        //var hinges = GetComponents<HingeJoint>();
        //foreach(HingeJoint hinge in hinges)
        //{
        //    if( hinge.breakForce < Mathf.Infinity)
        //    {
        //        weakHinge = hinge;
        //    }
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(trapEnabled && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DelayBreak());
        }
    }

    IEnumerator DelayBreak()
    {
        yield return new WaitForSeconds(.8f);
        Debug.Log("hinge break");
        Destroy(weakHinge);
    }
}
