using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Floor floor;
    private List<Floor> allFloors = new List<Floor>();

    public List<Obstacle> obstacles;
    private List<Obstacle> obstaclesSpawned = new List<Obstacle>();
    private int previousIndex;

    public Finish finish;

    private void Awake()
    {
        Random.InitState((int) System.DateTime.Now.Ticks); 
        int length = 30;
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

        Obstacle firstObstacleToSpawn = (Obstacle) Instantiate(obstacles[randomIndex()], firstObstaclePosition, Quaternion.identity);
        firstObstacleToSpawn.Initialize();
        obstaclesSpawned.Add(firstObstacleToSpawn);


        while(allFloors[allFloors.Count-1].exitPoint.transform.position.x - obstaclesSpawned[obstaclesSpawned.Count-1].exitPoint.position.x > 0)
        {
            Vector3 lastExitPoint = obstaclesSpawned[obstaclesSpawned.Count - 1].exitPoint.position;
            Vector3 obstaclePosition = new Vector3(Random.Range(lastExitPoint.x+10f, lastExitPoint.x+20f), lastExitPoint.y, lastExitPoint.z);
            Obstacle obstacleToSpawn = (Obstacle) Instantiate(obstacles[randomIndex()], obstaclePosition, Quaternion.identity);
            obstacleToSpawn.Initialize();
            obstaclesSpawned.Add(obstacleToSpawn);
        }

        Destroy(obstaclesSpawned[obstaclesSpawned.Count - 1].gameObject);
    }

    private int randomIndex()
    {
        int res = Random.Range(0, obstacles.Count);

        while(res == previousIndex){
            res = Random.Range(0, obstacles.Count);
        }

        previousIndex = res;

        return res;
    }
}
