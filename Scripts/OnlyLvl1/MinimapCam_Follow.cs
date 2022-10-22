using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam_Follow : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
