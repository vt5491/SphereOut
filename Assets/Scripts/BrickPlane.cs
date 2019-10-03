using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPlane : MonoBehaviour
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip hitBrickSound;
    [SerializeField] private GameObject brickPrefab;
    // public Brick [] Bricks;
    public GameObject [] Bricks = new GameObject[60];
    void Start()
    {
        Bricks = new GameObject[55];
        int brickCursor = 0;
        for (int col = 0; col < 4; col++)
        {
            for (int row = -5; row <= 5; row++)
            {
                var brickGo = Instantiate(brickPrefab, new Vector3(row * 1.1F, 0.25f + col * 0.8f, 12f), Quaternion.identity);
                // brickGo.name = brickCursor.ToString();
                brickGo.name = $"brick_{brickCursor}";

                var brick = brickGo.GetComponent<Brick>();
                brick.BrickIndex = brickCursor;

                Bricks[brickCursor] = brickGo;
                brickCursor++;
            }
        }
    }

    void Update()
    {
        
    }

    public void PlayBrickHitAudio() {
        soundSource.clip = hitBrickSound;
        soundSource.Play();
    }
}
