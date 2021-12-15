using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameObject.name == "Main Camera")
            transform.position = new Vector3(player.transform.position.x, 0.0f, -10.0f);
        else if(gameObject.name == "Death Particles")
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.0f);
    }
}
