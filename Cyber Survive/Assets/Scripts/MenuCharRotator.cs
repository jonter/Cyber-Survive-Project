using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharRotator : MonoBehaviour
{
    [SerializeField] float rotSpeed = 200;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotSpeed, 0);    
    }
}
