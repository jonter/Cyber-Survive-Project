using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAbove : MonoBehaviour
{
    Transform player;
    Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        player = transform.parent;
        offset = transform.position - player.position;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            transform.position = player.position + offset;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
