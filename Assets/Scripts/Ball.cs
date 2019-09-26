using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed {get; set;}= 1.5f; 
    private Vector3 dir = new Vector3();
    public Vector3 Velocity {get; set;}= new Vector3();
    // private GameObject paddle;
    // private SceneController sceneController;
    private SceneController SceneController {get; set;}
    // protected static SphericalPaddle SphericalPdl {get; private set;}
    void Start()
    // void Awake()
    // public void Init()
    {
        // Debug.Log("Ball.Init: entered");
        // paddle = GameObject.Find("spherical_paddle_proto");
        // Debug.Log($"velocity={Velocity}");
        // sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        SceneController = GameObject.Find("GameMaster").GetComponent<SceneController>();
        // var sphericalPaddle_go = GameObject.Find("SphericalPaddle"); 
        // SphericalPdl = sphericalPaddle_go.GetComponent<SphericalPaddleProto>();
        // SphericalPdl = GameObject.FindWithTag("SphericalPaddle").GetComponent<SphericalPaddle>();

        Init();
        Utils.DrawLine(Vector3.zero, Velocity, Color.green, 30.0f);
        // Utils.DrawLine(Vector3.zero, new Vector3(1, 1, 1), Color.blue, 30.0f);
    }

    void Update()
    {
        // Vector3 posDelta = Time.deltaTime * Velocity;
        Vector3 posDelta = Time.deltaTime * transform.forward;
        transform.position += posDelta;
        
    }

    public void Init() {
        Debug.Log($"Vector3.right={Vector3.right}, Vector3.left={Vector3.left}, Vector3.up={Vector3.up}, Vector3.forward={Vector3.forward}");
        // transform.position = new Vector3(0, 4, 5);
        // transform.position = new Vector3(5, 4, 5);
        transform.position = new Vector3(5, 0, 5);
        // reset forward position to the default
        transform.forward = Vector3.forward;

        // transform.forward = Quaternion.AngleAxis(-90, transform.forward) * transform.forward;
        transform.forward = Quaternion.AngleAxis(180, Vector3.right) * transform.forward;
        transform.forward = Quaternion.AngleAxis(45, Vector3.up) * transform.forward;
        transform.forward = Quaternion.AngleAxis(45, Vector3.right) * transform.forward;
        // transform.forward = Quaternion.AngleAxis(-45, Vector3.forward) * transform.forward;
        // works
        // transform.Rotate(0, 45, 0);
        Debug.Log($"Ball.Init: transform.forward={transform.forward}");
        // transform.position = new Vector3(0, 2, 5);
        Velocity = transform.forward *= speed;

        // // dir = (paddle.transform.position - transform.position).normalized;
        // dir = new Vector3(0, 0, -1);
        // transform.Rotate(-45, 0, 0);
        // // transform.Rotate(0, 0, 45, Space.Self);
        // // dir = Quaternion.AngleAxis(90, Vector3.right) * dir;
        // // dir = Quaternion.AngleAxis(-45, Vector3.up) * dir;
        // Debug.Log($"Ball.Init: transform.forward={transform.forward}, Vector3.forward={Vector3.forward}");
        // dir = transform.forward;
        // // dir = Quaternion.AngleAxis(-45, Vector3.y) * dir;
        // Velocity = dir *= speed;

        // Debug.Log("Ball.init: now calling sceneController.initBall");
        // sceneController.initBall();
        // Debug.Log("SceneController.initBall: entered");
        /*float rndOffset = SceneController.SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SceneController.SphericalPaddle.transform.position.x + rndOffset;*/ 
        /* 
        float rndOffset = SceneController.SphericalPaddle.Radius * Random.Range(-0.5f, 0.5f);
        float tgtPointX = SceneController.SphericalPaddle.transform.position.x; 
        var sp = SceneController.SphericalPaddle;
        Vector3 tgtPoint = new Vector3(tgtPointX, sp.transform.position.y, sp.transform.position.z);

        transform.position = new Vector3(0, 2, 5);

        var dir = (tgtPoint - transform.position).normalized;
        Velocity = dir *= speed;
        */
        // Debug.Log($"SceneController.initBall: Velocity={Velocity}");
    }
    // public void PaddleHit(Vector3 angleDelta) {
    //     // Debug.Log($"Ball.PaddleHit: entered");

    //     // velocity.x = -velocity.x;
    //     // velocity.z = -velocity.z;
    //     // Debug.Log($"Ball.PaddleHit: magnitude vel.pre={Velocity.magnitude}");
    //     // Velocity = new Vector3(
    //     //     (Velocity.x + angleDelta.x), 
    //     //     Velocity.y + angleDelta.y, 
    //     //     -(Velocity.z + angleDelta.z));
    //     // Debug.Log($"Ball.PaddleHit: magnitude vel.post={Velocity.magnitude}");
    //     Velocity = new Vector3(
    //         (-Velocity.x + angleDelta.x), 
    //         -(Velocity.y + angleDelta.y), 
    //         -(Velocity.z + angleDelta.z));
    // }

    // Send the ball back towards the game center
    public void BoundaryHit(SphericalPaddle paddle, Vector3 hitPoint) {
        dir = -1 * (transform.position - paddle.Pivot.position).normalized;
        Velocity = dir *= speed;

        transform.forward = dir;
        // Utils.DrawLine(hitPoint, hitPoint + 3.0f * Velocity, Color.green, 60);
        // Utils.DrawLine(hitPoint, hitPoint + 3.0f * Velocity, Color.green, 60);
        // transform.Rotate(0, 180f + flingAngle, 0);        
        Utils.DrawLine(transform.position, transform.position + 3.0f * Velocity, Color.green, 60);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Ball.OnTriggerEnter: entered");
        SceneController.BallTriggerDispatcher(other);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log($"Ball.OnCollisionEnter: entered");
        ContactPoint contact = other.contacts[0];

        var localContactPoint = contact.point - transform.position;
        Debug.Log($"Ball.transform.position={transform.position}");
        Debug.Log($"Ball.OnCollisionEnter: contact.point={contact.point}, localContactPoint={localContactPoint}");

        SceneController.BallCollisionDispatcher(other);
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"Ball.OnTrigerEnter: other.tag={other.tag}");
    //     if (other.tag == "SphericalPaddle") {
    //         Vector3 pc = SphericalPdl.transform.position;
    //         Vector3 bounceAngleDelta = other.transform.position - pc;
    //         BallPaddleHit(bounceAngleDelta);
    //     }
    // }
}
