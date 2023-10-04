using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.VFX;

public class RespawnManager : MonoBehaviour
{
    private Respawner[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        spawners = GetComponentsInChildren<Respawner>();
    }

    // Update is called once per frame

    public void SetNewSpawnPoint(Respawner spawner)
    {
        foreach(Respawner r in spawners)
        {
            r.respawnHere = false;
        }
        spawner.respawnHere = true;
        Debug.Log("Set new respawn");

    }
}
