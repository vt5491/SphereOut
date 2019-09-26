using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // public static SphericalPaddleProto SphericalPdl {get; private set;}
    public static SphericalPaddle SphericalPaddle {get; private set;}
    public static Ball Ball {get; private set;}
    // void Start()
    void Awake()
    {
        // Debug.Log("SceneController.Awake: entered");
        // var sphericalPaddle_go = GameObject.Find("spherical_paddle_proto"); 
        var sphericalPaddle_go = GameObject.Find("SphericalPaddle"); 
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddleProto>();
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddle>();
        SphericalPaddle = GameObject.FindWithTag("SphericalPaddle").GetComponent<SphericalPaddle>();

        Ball = GameObject.Find("Ball").GetComponent<Ball>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SphericalPaddle.Init();
            Ball.Init();
            // transform.position = sc.Rotate(0f, 0f).toCartesian;
            // SphericalPaddle.transform.position = sc.Rotate(0f, 0f).toCartesian;
        }
        
    }

    public void DoItController() {
        Debug.Log("SceneController.DoItController: about to call sphericalPaddle.DoIt");
        // SphericalPaddle.DoIt();
    }

    public void initBall() {
        // point the velocity to somewhere within the spherical paddle's
        // radius;
        // Debug.Log("SceneController.initBall: entered");
        float rndOffset = SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SphericalPaddle.transform.position.x + rndOffset; 
        var sp = SphericalPaddle;
        Vector3 tgtPoint = new Vector3(tgtPointX, sp.transform.position.y, sp.transform.position.z);

        Ball.transform.position = new Vector3(0, 2, 5);

        var dir = (tgtPoint - Ball.transform.position).normalized;
        Ball.Velocity = dir *= Ball.speed;
        // Debug.Log($"SceneController.initBall: Velocity={Ball.Velocity}");
    }

    // public void BallPaddleHit(Collider other) {
    //     if (other.tag == "Ball") {
    //         // SphericalPaddle sp = SphericalPaddle;
    //         // Vector3 pc = SphericalPdl.GetPaddleCtr();
    //         Vector3 pc = SphericalPdl.transform.position;
    //         Vector3 bounceAngleDelta = other.transform.position - pc;
    //         Ball.BallPaddleHit(bounceAngleDelta);
    //     }
    // }
    // private void OnTriggerEnter(Collider other)
    public void BallTriggerDispatcher(Collider other)
    {
        Debug.Log($"SceneController.OnTrigerEnter: other.tag={other.tag}, other.pos={other.transform.position}");
        if (other.tag == "SphericalPaddle") {
            // Vector3 paddleCenter = SphericalPaddle.transform.position;
            // // Vector3 pc = SphericalPaddle.Pivot.transform.position;
            // Vector3 bounceAngleDelta = other.transform.position - paddleCenter;
            // Ball.PaddleHit(bounceAngleDelta);

            // Vector3 velOut = SphericalPaddle.VelocityOut(other.transform.position, Ball.Velocity, "CenterZero_TipNinety");
            // Ball.Velocity = velOut;
        }
        else if ( other.transform.parent.tag == "Boundary" && other.transform.position != null ) {
            Ball.BoundaryHit(SphericalPaddle, other.transform.position);
        }
        else if ( other.transform.parent.tag == "Brick") {
            Ball.BoundaryHit(SphericalPaddle, other.transform.position);
            Debug.Log("SceneController.TriggerDispatcher: Brick Hit");
        }
    }

    public void BallCollisionDispatcher(Collision other)
    {
        ContactPoint contact = other.contacts[0];

        Debug.Log($"SceneController.BallCollisionDispatcher: sp.sc.toCartesian={SphericalPaddle.sc.toCartesian}");
        var paddleCenter = SphericalPaddle.PaddleCenterWorld();
        Debug.Log($"SceneController.BallCollisionDispatcher: paddleCenter={paddleCenter}");
        Debug.Log($"SceneController.BallCollisionDispatcher: contact - paddleCenter={contact.point - paddleCenter}");
        var centerDelta = contact.point - paddleCenter;
        var boundingBox = SphericalPaddle.GetComponent<BoxCollider>();
        float flingRatioWidth = centerDelta.x / boundingBox.size.x;  

        var flingAngle = 45f * flingRatioWidth;
        Debug.Log($"flingRatioWidth={flingRatioWidth}, flingAngle={flingAngle}");
        Ball.transform.Rotate(0, 180f + flingAngle, 0);        
        Debug.Log($"SceneController.BallCollisionDispatcher: transform.forward (pre)={transform.forward}, velocity (pre)={Ball.Velocity}");
        // Ball.transform.Rotate(45f, 90f, 0);        
        // Ball.Velocity = transform.forward *= Ball.speed;
        Debug.Log($"SceneController.BallCollisionDispatcher: transform.forward (post)={transform.forward}, velocity (post)={Ball.Velocity}");
    }


}
