using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SceneController SceneController {get; set;}
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip hitBrickSound;
    public int BrickIndex;
    public bool isActive;
    void Start()
    {
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        SceneController = GameObject.Find("GameMaster").GetComponent<SceneController>();
        isActive = true;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Brick.OnTriggerEnter: entered. other={other}");
        SceneController.BallTriggerDispatcher(other);
    }

    private void OnCollisionEnter(Collision other) {
        var brickGo = gameObject;
        Brick brick = brickGo.GetComponent<Brick>();
        Debug.Log($"Brick.OnCollisionEnter: entered. other={other}, brick.Index={brick.BrickIndex}");

        SceneController.BrickCollisionDispatcher(other);
        brickGo.SetActive(false);
    }

    public void PlayBrickHitAudio() {
        soundSource.clip = hitBrickSound;
        soundSource.Play();
    }

}
