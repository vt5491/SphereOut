using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlanarPaddle : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Vector2 paddleAction;
    [SerializeField] private float moveSpeed = 5.0f;
    void Start()
    {
        
    }

    public void Init()  {

    }

    void Update()
    {
        var axis = paddleAction.GetAxis(SteamVR_Input_Sources.RightHand);
        var cx = axis.x;
        var cy = axis.y;
 
        var h = cx*cx > cy*cy ? cx : cy;
 
        if( h*h > .1f )
        {
            transform.position = transform.position + new Vector3(cx * moveSpeed * Time.deltaTime, cy * moveSpeed * Time.deltaTime, 0);
        }
        
    }
}
