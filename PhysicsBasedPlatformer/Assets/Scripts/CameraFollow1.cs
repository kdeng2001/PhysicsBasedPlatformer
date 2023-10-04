using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow1 : MonoBehaviour
{
    public Transform target;
    public GameObject text;
    Transform  ui;
    Transform cam;
    
    void Start()
    {
        cam = Camera.main.transform;
        ui = text.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ui.position = target.position;
        ui.forward = cam.forward;
        // ui.rotation.y = 180;
        // ui.LookAt(cam);
        // ui.rotation = Quaternion.LookRotation(ui.position - cam.position);
    }

}
