using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public static LevelGenerator sharedInstance;

    public Floor floor;
    public List<Floor> floorsSpawned = new List<Floor>();

    public Background background;
    private List<Background> backgroundSpawned = new List<Background>();

    public List<Obstacle> obstacles;
    private List<Obstacle> obstaclesSpawned = new List<Obstacle>();
    private int previousIndex = 10, prePreviousIndex = 10;

    public List<Shortcut> shortcuts;
    private Shortcut shorcutSpawned;

    public Finish finish;


    private void Awake()
    {
        sharedInstance = this;
        Random.InitState((int) System.DateTime.Now.Ticks);

        spawnFloorsAndBackgounds();

        spawnObstacles();

        spawnShortcut();  
    }


    void spawnFloorsAndBackgounds()
    {
        int length = 60;
        for (int i = 0; i < length; i++)
        {
            Floor floorToSpawn = (Floor)Instantiate(floor, this.transform.position, Quaternion.identity);

            Vector3 backgroundInitialPosition = new Vector3(this.transform.position.x, this.transform.position.y + 64, this.transform.position.z);
            Background backgroundToSpawn = (Background)Instantiate(background, backgroundInitialPosition, Quaternion.identity);

            if (i != 0)
            {
                floorToSpawn.transform.position = floorsSpawned[i - 1].exitPoint.transform.position;
                backgroundToSpawn.transform.position = backgroundSpawned[i - 1].exitPoint.transform.position;
            }

            floorsSpawned.Add(floorToSpawn);
            backgroundSpawned.Add(backgroundToSpawn);

            if (i == length - 1)
            {
                Instantiate(finish, floorsSpawned[i].exitPoint.transform.position, Quaternion.identity);
            }
        }
    }


    void spawnObstacles()
    {
        Vector3 firstObstaclePosition = new Vector3(floorsSpawned[1].exitPoint.transform.position.x, -96f, floorsSpawned[1].exitPoint.transform.position.z);

        Obstacle firstObstacleToSpawn = (Obstacle)Instantiate(obstacles[randomIndex()], firstObstaclePosition, Quaternion.identity);
        firstObstacleToSpawn.Initialize();
        obstaclesSpawned.Add(firstObstacleToSpawn);


        while (floorsSpawned[floorsSpawned.Count - 1].exitPoint.transform.position.x - obstaclesSpawned[obstaclesSpawned.Count - 1].exitPoint.position.x > 0)
        {
            Vector3 lastExitPoint = obstaclesSpawned[obstaclesSpawned.Count - 1].exitPoint.position;
            Vector3 obstaclePosition = new Vector3(Random.Range(lastExitPoint.x + 96f, lastExitPoint.x + 160f), lastExitPoint.y, lastExitPoint.z);
            Obstacle obstacleToSpawn = (Obstacle)Instantiate(obstacles[randomIndex()], obstaclePosition, Quaternion.identity);
            obstacleToSpawn.Initialize();
            obstaclesSpawned.Add(obstacleToSpawn);
        }

        Destroy(obstaclesSpawned[obstaclesSpawned.Count - 1].gameObject);
    }


    private int randomIndex()
    {
        int res = Random.Range(0, obstacles.Count);
        Debug.Log(res);

        while(res == previousIndex || res == prePreviousIndex){
            res = Random.Range(0, obstacles.Count);
            Debug.Log("Reroll: " + res);
        }

        prePreviousIndex = previousIndex;
        previousIndex = res;

        return res;
    }


    void spawnShortcut()
    {
        if (/*Random.Range(0, 2) == 0*/ true)
        {
            int random;
            if (shortcuts[0].start == "Beginning")
            {
                random = Random.Range(0, 21);
            }
            else
            {
                random = Random.Range(20, 41);
            }

            Vector3 shortcutPosition = new Vector3(floorsSpawned[random].transform.position.x, -96f, floorsSpawned[Random.Range(0, 21)].transform.position.z);
            shorcutSpawned = (Shortcut)Instantiate(shortcuts[0], shortcutPosition, Quaternion.identity);
            checkShortcutPosition();
        }
    }


    private void checkShortcutPosition()
    {
        for (int i = 0; i < obstaclesSpawned.Count; i++)
        {
            Obstacle obstacleChecked = obstaclesSpawned[i];

            if (!obstacleChecked.isStatic)
            {
                break;
            }

            bool b1 = shorcutSpawned.transform.position.x >= obstacleChecked.transform.position.x;
            bool b2 = shorcutSpawned.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;
            bool b3 = shorcutSpawned.exitPoint.transform.position.x >= obstacleChecked.transform.position.x;
            bool b4 = shorcutSpawned.exitPoint.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;

            if(!b2 || !b3)
            {
                break;
            }

            if((b1 && b2) || (b3 && b4))
            {
                shorcutSpawned.transform.position = new Vector3(obstacleChecked.exitPoint.transform.position.x + 32f, shorcutSpawned.transform.position.y, shorcutSpawned.transform.position.z);
                break;
            } 
        }
    }
}
