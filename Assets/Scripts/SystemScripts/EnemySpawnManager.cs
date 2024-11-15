using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    ///<summary>
    /// This manager should handle the logic of when and how frequently mobs spawn.
    ///Need a list of enemies that can be spawned
    ///enemy type doesn't change from day->night, only their behavior
    ///do we want "rarer" mobs? 
    /// I don't think so, but maybe not in theat sense, probably just by weights?
    /// Could do a pair with <enemy, weight> for spawning..
        /// Lets try that first
    

    /// </summary>    
    
    [Serializable]
    public class Pair //based this works
    {
        public GameObject first;
        public int second;
    }

    [SerializeField] private List<Pair> enemySpawns;
    [SerializeField] private int nightSpawnChance;
        [SerializeField] private int daySpawnChance;
    [SerializeField] private int minSpawnRange;
    [SerializeField] private int maxSpawnRange;

    private System.Random random = new System.Random(); //Random object to roll chances
    

    private GameObject playerPos; 
    //need this to determine invalid span spawn area, range within a certain distance
    //private GameObject BaseCore //will eventually need this so mobs cant spawn in the base
    //for now lets just do clamp on a random range
    //this is gonna get complicated, will eventually need to check whether spawn position is on top of a gameobject, will likely need raycast

    void Start() {
        playerPos = GameObject.Find("Player");
    }
    

    //flow
    /// <summary>
    /// every update, determine a chance
    /// if that chance succeeds, attempt to spawn an enemy
    /// generate a random position between x and y from the player
    /// check if it's a valid position
    /// if not, failed, try again next time
    /// </summary>

    void Update()
    {
        //need to decide how enemy spawning is going to work
        //random position in the world?
        //spawn "areas"?
        //is there a cap? probably?
        //for now since we dont have biomes, we'll just do randomly.. outside of the players screen
        //(and later, not inside their build zone)
        if(rollSpawn())
        {
            Vector2 position = rollPosition(); //now we have a position! need to check whether its on top of a gameobject
            print("Checking position " + position);
            if(!TileOccupancyChecker.instance.IsWorldPosOccupied(position))
            {
                print("Valid position at " + position);
                SpawnEnemy(position);
            }
            
        }
    }


    private bool rollSpawn()
    {
        int diceRoll = random.Next(1, nightSpawnChance);
        if(diceRoll == 1)
        {
            print("Rolled a spawn!");
            return true;
        }
        return false;
    }

    Vector2 rollPosition()
    {
        print("Rolling position");
        //from player position, some arbitrary maxRange and minRange
        //generate random X and y, can we clamp a vector2? yes!

        //this ugly ass declaration is
        //random position within spawn range
            //add the players position
            //clamp within world boundaries
        Vector2 position = 
        new Vector2(    Mathf.Clamp(random.Next(minSpawnRange, maxSpawnRange)+playerPos.transform.position.x, -GridGenerator.instance.gridSizeX, GridGenerator.instance.gridSizeX),
                        Mathf.Clamp(random.Next(minSpawnRange, maxSpawnRange)+playerPos.transform.position.y, -GridGenerator.instance.gridSizeY, GridGenerator.instance.gridSizeY));
        
        print(position);
        return position;
    }


    private void SpawnEnemy(Vector2 position) {
        //will eventually need logic for picking which spawnpool to pull from based on which biome is at the decided location
        //first need to randomly pick one, add up all their weights and pick a number from 1-weighttotal
        //then.. if statement thing
        int weightTotal = 0;
        for(int i = 0; i < enemySpawns.Count; ++i) {
            weightTotal += enemySpawns[i].second;
        }
        //now we have total weight value
        int val = random.Next(1, weightTotal);
        //now some sort of loop for weight checking..?

        int curr = 0;
        for(int i = 0; i < enemySpawns.Count; ++i) {
            curr += enemySpawns[i].second;
            if (val < curr){
                //rolled this weight
                print("Spawning a " + enemySpawns[i].first.name);
                Instantiate(enemySpawns[i].first, position, Quaternion.identity);
            }
        }
    }
    
}
 