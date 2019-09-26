using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SphericalPaddle : MonoBehaviour
{
    [Header("Action Mappings")]
    public SteamVR_Action_Vector2 movePaddle;
    [Header("General Parms")]
    public float rotateSpeed = 1f;
    // public Transform pivot {get; set;}
    public Transform Pivot;
    public SphericalCoordinates sc;
    private SphericalPaddleInternal sphericalPaddleInternal; 

    // private float lastPolar;
    // private float lastElevation;
    // public float Radius {get; private set;} 
    public float Radius {get; set;} 
    public float paddleHeight {get; set;}
    public float paddleWidth {get; set;}
    void Start()
    {
        Debug.Log($"SphericalPaddle.Start: entered");
        Init();
        // Radius = 1.0f;
        // sc = new SphericalCoordinates(transform.position, 0f, 6f, 
        //     -Mathf.PI * 1f, Mathf.PI / 1f,  
        //     -Mathf.PI / 1f, Mathf.PI / 1f);
        // sc.loopElevation = true;
        // sc.loopPolar = true;
        // // Initialize  position
        // transform.position = sc.toCartesian + Pivot.position;
        // // this.lastPolar = sc.polar;
        // // this.lastElevation = sc.elevation;

        // // Debug.Log($"Start: forward={transform.forward}");
        // sphericalPaddleInternal = new SphericalPaddleInternal();
        // // Debug.Log($"SphericalPaddle.Start: sphericalPaddleInternal.ThetaOut={sphericalPaddleInternal.ThetaOut()}");
        // // paddle width and height are just the size of the bounding box
        // var boundingBox = this.GetComponent<BoxCollider>();
        // paddleWidth = boundingBox.size.x;
        // paddleHeight = boundingBox.size.y;

        // Debug.Log($"SphericalPaddle: pwidth={paddleWidth}, ph={paddleHeight}");

    }
    
    public void Init() {
        Radius = 1.0f;
        sc = new SphericalCoordinates(transform.position, 0f, 6f, 
            -Mathf.PI * 1f, Mathf.PI / 1f,  
            -Mathf.PI / 1f, Mathf.PI / 1f);
        sc.loopElevation = true;
        sc.loopPolar = true;
        // Initialize  position
        transform.position = sc.toCartesian + Pivot.position;
        // this.lastPolar = sc.polar;
        // this.lastElevation = sc.elevation;

        // Debug.Log($"Start: forward={transform.forward}");
        sphericalPaddleInternal = new SphericalPaddleInternal();
        // Debug.Log($"SphericalPaddle.Start: sphericalPaddleInternal.ThetaOut={sphericalPaddleInternal.ThetaOut()}");
        // paddle width and height are just the size of the bounding box
        var boundingBox = this.GetComponent<BoxCollider>();
        paddleWidth = boundingBox.size.x;
        paddleHeight = boundingBox.size.y;

        Debug.Log($"SphericalPaddle: pwidth={paddleWidth}, ph={paddleHeight}, Pivot.position={Pivot.position}");
    }

    void Update()
    {
        var axis = movePaddle.GetAxis(SteamVR_Input_Sources.RightHand);
        var cx = axis.x;
        var cy = axis.y;
 
        var h = cx*cx > cy*cy ? cx : cy;
 
        if( h*h > .1f )
        {
            transform.position = sc.Rotate( cx * rotateSpeed * Time.deltaTime, cy * rotateSpeed * Time.deltaTime ).toCartesian + Pivot.position;

            transform.LookAt( Pivot.transform , Vector3.up );
            transform.Rotate( Vector3.up, 180f);

        }

        if (Input.GetKeyDown("space"))
        {
            transform.position = sc.Rotate(0f, 0f).toCartesian;
        }
        
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"SphericalPaddle.OnTrigerEnter: other.tag={other.tag}");
    // }

    // public void doIt() {
    //     Debug.Log($"SphericalPaddle.doIt: Radius={Radius}, pos={transform.position]");
    // }
    public Vector3 VelocityOut(Vector3 collisionPoint, Vector3 velocityIn, string strategy) {
        // Debug.Log($"SphericalPaddle.VelocityOut: Radius={Radius}, pos={transform.position}");
        Debug.Log($"SphericalPaddle.VelocityOut: collisionPoint={collisionPoint}");
        // return 1f;
        var r = Radius;
        SphericalCoordinates velInSc = new SphericalCoordinates(velocityIn, 0, 6f, -Mathf.PI, Mathf.PI, -Mathf.PI, Mathf.PI);
        // Note: Vert and Horiz are "logical" not physical.  Thus vert is always "up" and horizontial is
        // "sideways" even if the paddle is up at the poles.
        // delta is distance from the center.
        // convert collision point into spherical coords.
        // sc = new SphericalCoordinates(collisionPoint, 0f, 6f, 
        //     -Mathf.PI * 1f, Mathf.PI / 1f,  
        //     -Mathf.PI / 1f, Mathf.PI / 1f);
        // SphericalCoordinates collisonPointSc = sc.Clone();
        // SphericalCoordinates collisonPointSc = Instantiate(sc);
        // collisonPointSc = sc.FromCartesian(collisionPoint - Pivot.position);
        // SphericalCoordinates collisonPointSc = new SphericalCoordinates(collisionPoint - Pivot.position);
        Debug.Log($"collisionPoint - Pivot.position={collisionPoint - Pivot.position}");
        Vector3 tmpVec = collisionPoint - Pivot.position;
        // SphericalCoordinates collisonPointSc = new SphericalCoordinates(collisionPoint - Pivot.position);
        SphericalCoordinates collisonPointSc = new SphericalCoordinates(tmpVec, 0, 6f, -Mathf.PI, Mathf.PI, -Mathf.PI, Mathf.PI);
        Debug.Log($"sc={sc.ToString()}");
        Debug.Log($"cpSc={collisonPointSc.ToString()}");
        // SphericalCoordinates collisionPointSc = new SphericalCoordinates();
        // collisionPointSc.SetRadius(sc.radius);
        // collisionPointSc.SetRadius(sc.radius);
        // float deltaHeight = paddleHeight / 2 - 
        // var deltaElevationAngle = collisonPointSc.elevation - sc.elevation;
        var deltaElevationAngle = Mathf.DeltaAngle(collisonPointSc.elevation, sc.elevation);

        // var paddleHeightAngle = paddleHeight / (Radius *  Mathf.PI / 2.0f);
        var paddleHeightAngle = paddleHeight / r;
        Debug.Log($"deltaElevation={deltaElevationAngle * Mathf.Rad2Deg}, paddleHeightAngle={paddleHeightAngle * Mathf.Rad2Deg}");
        // 90 for tips, 0 at center
        var vertGapAngle = ((Mathf.PI / 2.0f) * deltaElevationAngle / paddleHeightAngle) % paddleHeightAngle; 
        Debug.Log($"vertGapAngle={vertGapAngle * Mathf.Rad2Deg}");

        // var velOut = new Vector3(0, 0, 0);
        // var velOutSc = new SphericalCoordinates(velOut);
        // var velOutSc = new SphericalCoordinates(r, collisonPointSc.polar, collisonPointSc.elevation + vertGapAngle, 0, 6f, -Mathf.PI, Mathf.PI, -Mathf.PI, Mathf.PI);
        // velOutSc.Rotate(0, vertGapAngle);
        SphericalCoordinates velOutSc;
        // if(vertGapAngle < 0) {
        if(collisonPointSc.elevation > sc.elevation) {
            velOutSc = new SphericalCoordinates(r, -velInSc.polar, -velInSc.elevation - vertGapAngle, 0, 6f, -Mathf.PI, Mathf.PI, -Mathf.PI, Mathf.PI);
            Debug.Log("path-a");
        }
        else {
            velOutSc = new SphericalCoordinates(r, -velInSc.polar, -velInSc.elevation + vertGapAngle, 0, 6f, -Mathf.PI, Mathf.PI, -Mathf.PI, Mathf.PI);
            Debug.Log("path-b");
        }
        Debug.Log($"VelocityOut: velOut.elevation={velOutSc.elevation * Mathf.Rad2Deg}");
        // return new Vector3(0,0,0);

        return velOutSc.toCartesian;

    }

    public Vector3 PaddleCenterWorld() {
        return sc.toCartesian + Pivot.position;
    }
}

public class SphericalPaddleInternal {
    // public float ThetaOut() {
    //     return 1f;
    // }
}
