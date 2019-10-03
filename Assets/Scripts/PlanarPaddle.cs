using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// [RequireComponent(typeof(Paddle))]
public class PlanarPaddle : MonoBehaviour, IPaddle
// public class PlanarPaddle : Paddle
{
    [SerializeField] private SteamVR_Action_Vector2 paddleAction;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip hitPaddleSound;

    public float Width {get; set;}
    public float Height {get; set;}
    
    
    void Start()
    {
        Init();
        
    }

    public void Init()  {
        var boundingBox = this.GetComponent<BoxCollider>();
        Width = boundingBox.size.x;
        Height = boundingBox.size.y;
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

    // protected string DoIt() {
    //     return "abc";
    // }

    public bool ExceededPaddleBoundary(Vector3 pos) {
        bool exceededBoundary = false;
        if (pos.z < transform.position.z)
            exceededBoundary = true;

        return exceededBoundary;
    }

    public void PlayPaddleHitAudio() {
        soundSource.clip = hitPaddleSound;
        soundSource.Play();
    }
}
