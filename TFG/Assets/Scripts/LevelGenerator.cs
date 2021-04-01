using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Floor floor;
    private List<Floor> allFloors = new List<Floor>();

    public List<Obstacle> obstacles;
    private List<Obstacle> obstaclesSpawned = new List<Obstacle>();

    public Finish finish;

    private void Awake()
    {
        Random.InitState((int) System.DateTime.Now.Ticks); 
        int length = 3;
        for (int i = 0; i < length; i++)
        {
            Floor floorToSpawn = (Floor) Instantiate(floor, this.transform.position, Quaternion.identity);

            if (i != 0)
            {
                floorToSpawn.transform.position = allFloors[i - 1].exitPoint.transform.position;  
            }

            allFloors.Add(floorToSpawn);
            
            if(i == length - 1)
            {
                Instantiate(finish, allFloors[i].exitPoint.transform.position, Quaternion.identity);
            }
        }

        Vector3 firstObstaclePosition = new Vector3(allFloors[1].exitPoint.transform.position.x, -4f, allFloors[1].exitPoint.transform.position.z);
        Obstacle firstObstacleToSpawn = (Obstacle) Instantiate(obstacles[Random.Range(0, obstacles.Count)], firstObstaclePosition, Quaternion.identity);
        obstaclesSpawned.Add(firstObstacleToSpawn);


        while(allFloors[allFloors.Count-1].exitPoint.transform.position.x - obstaclesSpawned[obstaclesSpawned.Count-1].exitPoint.position.x > 0)
        {
            Vector3 lastExitPoint = obstaclesSpawned[obstaclesSpawned.Count - 1].exitPoint.position;
            Vector3 obstaclePosition = new Vector3(Random.Range(lastExitPoint.x+7f, lastExitPoint.x+12f), lastExitPoint.y, lastExitPoint.z);
            Obstacle obstacleToSpawn = (Obstacle) Instantiate(obstacles[Random.Range(0, obstacles.Count)], obstaclePosition, Quaternion.identity);
            obstaclesSpawned.Add(obstacleToSpawn);
        }

        Destroy(obstaclesSpawned[obstaclesSpawned.Count - 1].gameObject);
        
    }
}
