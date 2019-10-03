using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject paddleGameObject; 
    private IPaddle paddle;
    // private Paddle paddle;
    // [SerializeField] public IPaddle paddle2; 
    // public static SphericalPaddleProto SphericalPdl {get; private set;}
    // public static SphericalPaddle SphericalPaddle {get; private set;}
    // public static IPaddle paddle {get; private set;}
    // public IPaddle Paddle; 
    private static SphericalPaddle SphericalPaddle {get; set;}
    private static PlanarPaddle PlanarPaddle {get; set;}
    public static Ball Ball {get; private set;}
    public static BrickPlane BrickPlane {get; private set;}
    // void Start()
    void Awake()
    {
        Debug.Log($"SceneController.Awake: entered, paddle.GetType={paddleGameObject.GetType()}");
        // var sphericalPaddle_go = GameObject.Find("spherical_paddle_proto"); 
        // var sphericalPaddle_go = GameObject.Find("SphericalPaddle"); 
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddleProto>();
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddle>();
        // SphericalPaddle = GameObject.FindWithTag("SphericalPaddle").GetComponent<SphericalPaddle>();
        if (paddleGameObject.GetComponent<SphericalPaddle>() != null) {
            SphericalPaddle = paddleGameObject.GetComponent<SphericalPaddle>();
            paddle = (IPaddle) SphericalPaddle;
        }
        if (paddleGameObject.GetComponent<PlanarPaddle>() != null) {
            PlanarPaddle = paddleGameObject.GetComponent<PlanarPaddle>();
            // paddle = (IPaddle) PlanarPaddle;
            paddle = (IPaddle) PlanarPaddle;
            // Debug.Log($"SceneController: paddle.DoIt={paddle.DoIt()}");
            // Debug.Log($"SceneController: paddle.DoIt={PlanarPaddle.DoIt()}");
        }

        Ball = GameObject.Find("Ball").GetComponent<Ball>();

        var brickPlaneGo = GameObject.FindWithTag("BrickPlane");
        if (brickPlaneGo != null) {
            BrickPlane = brickPlaneGo.GetComponent<BrickPlane>();
        }
        
        // Debug.Log($"SceneController.Awake: entered, SphericalPaddle.GetType()={SphericalPaddle.GetType()}");
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log($"SceneController.Update: space pressed");
            if (SphericalPaddle)
                SphericalPaddle.Init();
            if (PlanarPaddle)
                PlanarPaddle.Init();
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
        float rndOffset = SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SphericalPaddle.transform.position.x + rndOffset; 
        var sp = SphericalPaddle;
        Vector3 tgtPoint = new Vector3(tgtPointX, sp.transform.position.y, sp.transform.position.z);

        Ball.transform.position = new Vector3(0, 2, 5);

        var dir = (tgtPoint - Ball.transform.position).normalized;
        Ball.Velocity = dir *= Ball.Speed;
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
        if (other.transform.parent != null) 
            Debug.Log($"SceneController.OnTrigerEnter: other.parent.tag={other.transform.parent.tag}");

        if (other.tag == "SphericalPaddle") {
            // Vector3 paddleCenter = SphericalPaddle.transform.position;
            // // Vector3 pc = SphericalPaddle.Pivot.transform.position;
            // Vector3 bounceAngleDelta = other.transform.position - paddleCenter;
            // Ball.PaddleHit(bounceAngleDelta);

            // Vector3 velOut = SphericalPaddle.VelocityOut(other.transform.position, Ball.Velocity, "CenterZero_TipNinety");
            // Ball.Velocity = velOut;
        }
        else if ( other.transform.parent.tag == "Boundary" && other.transform.position != null ) {
            if (SphericalPaddle != null) {
                Ball.BoundaryHit(SphericalPaddle, other.transform.position);
            }
            if (PlanarPaddle != null) {
                Ball.BoundaryHit(PlanarPaddle, other.transform.position);
            }
        }
        else if ( other.transform.parent.tag == "Brick") {
            // Ball.BrickHit(paddle, other);
            var brickGo = other.gameObject;
            // var brick = brickGo.GetComponent<Brick>();
            string brickKey = brickGo.name;
            Debug.Log($"SceneController.TriggerDispatcher: Brick Hit, index={brickKey}");

            if (BrickPlane != null) {
                BrickPlane.PlayBrickHitAudio();
            }
        }
        else if ( other.tag == "PlanarPaddle") {
            Debug.Log("SceneController.TriggerDispatcher: Planar paddle Hit");
        }
    }

    public void BallCollisionDispatcher(Collision other)
    {
        if (other.gameObject.tag == "SphericalPaddle")
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
            // Ball.LastForward = Ball.transform.forward;
        }
        else if (other.gameObject.tag == "PlanarPaddle")
        {
            Debug.Log($"SceneController: planar paddle hit");
            PlanarPaddle.PlayPaddleHitAudio();
            ContactPoint contact = other.contacts[0];
            var centerDelta = contact.point - PlanarPaddle.transform.position;

            Debug.Log($"SC: centerDelta={centerDelta}, PlanarPaddle.Width={PlanarPaddle.Width}");
            var widthFactorAngle = 45f * centerDelta.x / PlanarPaddle.Width;
            Debug.Log($"SC: widthFactorAngle={widthFactorAngle}");
            // var widthAngleFactor =
            Ball.transform.Rotate(0, 180f + widthFactorAngle, 0);
            // Ball.LastForward = Ball.transform.forward;
        }
        else if (other.gameObject.tag == "SideWall") {
            var fwd = Ball.transform.forward;
            Ball.transform.forward =  new Vector3(-fwd.x, fwd.y, fwd.z);
            // Ball.LastForward = Ball.transform.forward;
        }
    }

    public void BrickCollisionDispatcher(Collision other) {
        if(other.gameObject.tag == "Ball") {
            Ball.BrickBounce();
            BrickPlane.PlayBrickHitAudio();
        }

    }

    public bool BallExceededPaddleBoundary() {
        return paddle.ExceededPaddleBoundary(Ball.transform.position);
    }


}
