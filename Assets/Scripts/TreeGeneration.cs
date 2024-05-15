using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeGeneration : MonoBehaviour
{
   
    private int gridSizeX = GridGenerator.gridSizeX/7; // div by 12
    

    private int gridSizeY = GridGenerator.gridSizeY/7;
    public GameObject[] desertTrees;
    public Tilemap tilemap;
    void Start()
    {
        GenerateTrees();
    }

    void GenerateTrees() {
        float [,] noiseMap = new float[gridSizeX, gridSizeY];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);
        int startX = 0 - gridSizeX/2;
        int startY = 0 - gridSizeY/2;
        float scale = .1f;

        Debug.Log(gridSizeX);
        for(int x = startX; x < gridSizeX/2; x++) {
            for(int y = startY; y < gridSizeY/2; y++) {
                //float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                //noiseMap[x+50, y+50] = noiseValue;
                //if(x == 1 || x == 0 || x == -1) {
                //    continue;
                //} else {
                //    if(noiseValue < .15f){
                //        Vector3Int cellPosition = new Vector3Int(x, y, 1);
                //        int index = Random.Range(0, desertTrees.Length);
                //        
                //        Instantiate(desertTrees[index], cellPosition, Quaternion.identity);
                //       
                //   }
                //} Lets try something different
                float randomValue = Random.Range(0,100);
                if(randomValue > 90) {

                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    int index = Random.Range(0, desertTrees.Length);
                    Instantiate(desertTrees[index], cellPosition, Quaternion.identity);
                }                
                
            }
        }
    }
}

    
