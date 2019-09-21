using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickRing : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    void Start()
    {
        for (int col = 0; col < 2; col++)
        {
            for (int row = -1; row < 5; row++)
            {
                Instantiate(brickPrefab, new Vector3(row * 1.1F, 8.25f + col * 2.0f, 10f), Quaternion.identity);
            }
        }
        
    }

    void Update()
    {
        
    }
}
