using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleGeneration : MonoBehaviour
{
    [SerializeField] private int gridSizeX = 100;
    [SerializeField] private int gridSizeY = 100;
    public TileBase[] obstacleTiles;
    public Tilemap tilemap;
    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles() {
        float [,] noiseMap = new float[gridSizeX, gridSizeY];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);
        int startX = 0 - gridSizeX/2;
        int startY = 0 - gridSizeY/2;
        float scale = .1f;

        for(int x = startX; x < gridSizeX/2; x++) {
            for(int y = startY; y < gridSizeY/2; y++) {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x+gridSizeX/2, y+gridSizeY/2] = noiseValue;
                if(x == 1 || x == 0 || x == -1) {
                    continue;
                } else {
                    if(noiseValue < .15f){
                        Vector3Int cellPosition = new Vector3Int(x, y, 1);
                        int index = Random.Range(0, obstacleTiles.Length);
                        tilemap.SetTile(cellPosition, obstacleTiles[index]);
                    
                    } 
                } 
                
            }
        }
    }
}

    
