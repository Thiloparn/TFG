using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public static LevelGenerator sharedInstance;

    public List<Floor> floors = new List<Floor>();
    public List<Floor> floorsSpawned = new List<Floor>();
    private List<Floor> nextFloorsSpawned = new List<Floor>();

    public List<Background> backgrounds = new List<Background>();
    private List<Background> backgroundSpawned = new List<Background>();
    private List<Background> nextBackgroundSpawned = new List<Background>();

    public List<Obstacle> obstacles;
    public List<Obstacle> obstaclesSpawned = new List<Obstacle>();
    private List<Obstacle> nextObstaclesSpawned = new List<Obstacle>();
    private int previousIndex = 10, prePreviousIndex = 10;

    public Stairs stairs;
    private List<Stairs> stairsSpawned = new List<Stairs>();
    private List<Stairs> nextStairsSpawned = new List<Stairs>();
    private List<int> stairsPositionSpawned = new List<int>();

    public List<Shortcut> shortcuts = new List<Shortcut>();
    private Shortcut shorcutSpawned;
    private Shortcut nextShorcutSpawned;

    public Finish finish;

    public string zone;
    public int level;
    public bool changeLevel = false;


    private void Awake()
    {
        sharedInstance = this;
        Random.InitState((int) System.DateTime.Now.Ticks);
        zone = "Community";
        level = 4;

        spawnFloorsAndBackgrounds(this.transform.position);

        spawnObstacles();

        spawnStairs();
        
        spawnShortcut();
        
        clearLevel();
    }

    private void Update()
    {
        float playerPosition = Player.sharedInstance.transform.position.x;
        float endOfLevel = floorsSpawned[floorsSpawned.Count - 2].transform.position.x;

        if(zone != "Metropolis")
        {
            if (changeLevel)
            {
                useStairOrElevator();
            } 
            
            if (zone == "Community" && level == 0  && playerPosition >= endOfLevel && nextFloorsSpawned.Count == 0) 
            {
                zone = "Metropolis";

                spawnFloorsAndBackgrounds(floorsSpawned[floorsSpawned.Count - 1].exitPoint.transform.position);

                spawnObstacles();

                spawnShortcut();
            }
        }
        else
        {
            if (changeLevel)
            {
                clearLevel();
                zone = "Community";
                useStairOrElevator();
            }

            if (playerPosition >= endOfLevel && nextFloorsSpawned.Count == 0)
            {
                zone = "University";

                spawnFloorsAndBackgrounds(floorsSpawned[floorsSpawned.Count - 1].exitPoint.transform.position);
                
                spawnObstacles();

                spawnStairs();

                spawnShortcut();
            }
        }

        if (nextFloorsSpawned.Count > 0)
        {
            if (playerPosition >= nextFloorsSpawned[2].transform.position.x)
            {
                clearLevel();
            }
        }
    }


    void spawnFloorsAndBackgrounds(Vector3 initialPoint)
    {
        Floor floor;
        Background background;
        int length;

        if(zone == "Community")
        {
            floor = floors[0];
            background = backgrounds[0];
            length = 12;
        } 
        else if(zone == "Metropolis")
        {
            floor = floors[1];
            background = backgrounds[1];
            length = 40;
        } 
        else
        {
            floor = floors[2];
            background = backgrounds[2];
            length = 12;
        }

        Floor firstFloorToSpawn = (Floor)Instantiate(floor, initialPoint, Quaternion.identity);

        Vector3 backgroundInitialPosition = new Vector3(initialPoint.x, initialPoint.y + 64, initialPoint.z);
        Background firstBackgroundToSpawn = (Background)Instantiate(background, backgroundInitialPosition, Quaternion.identity);

        nextFloorsSpawned.Add(firstFloorToSpawn);
        nextBackgroundSpawned.Add(firstBackgroundToSpawn);

        int i = 1;
        while (i < length)
        {
            Floor floorToSpawn = (Floor)Instantiate(floor, nextFloorsSpawned[i - 1].exitPoint.transform.position, Quaternion.identity);
            nextFloorsSpawned.Add(floorToSpawn);

            Background backgroundToSpawn = (Background)Instantiate(background, nextBackgroundSpawned[i - 1].exitPoint.transform.position, Quaternion.identity);
            nextBackgroundSpawned.Add(backgroundToSpawn);

            i++;
        }
    }


    void spawnObstacles()
    {
        List<Obstacle> obstaclesOfZone = obstacles.FindAll(x => x.zone.Contains(zone));

        Vector3 firstObstaclePosition = new Vector3(nextFloorsSpawned[1].exitPoint.transform.position.x, -96f, nextFloorsSpawned[1].exitPoint.transform.position.z);

        Obstacle firstObstacleToSpawn = (Obstacle)Instantiate(obstaclesOfZone[randomIndex(obstaclesOfZone)], firstObstaclePosition, Quaternion.identity);
        firstObstacleToSpawn.Initialize();
        nextObstaclesSpawned.Add(firstObstacleToSpawn);


        while (nextFloorsSpawned[nextFloorsSpawned.Count - 1].transform.position.x - nextObstaclesSpawned[nextObstaclesSpawned.Count - 1].exitPoint.position.x > 0)
        {
            Vector3 lastExitPoint = nextObstaclesSpawned[nextObstaclesSpawned.Count - 1].exitPoint.position;
            Vector3 obstaclePosition = new Vector3(Random.Range(lastExitPoint.x + 128f, lastExitPoint.x + 160f), lastExitPoint.y, lastExitPoint.z);
            Obstacle obstacleToSpawn = (Obstacle)Instantiate(obstaclesOfZone[randomIndex(obstaclesOfZone)], obstaclePosition, Quaternion.identity);
            obstacleToSpawn.Initialize();
            nextObstaclesSpawned.Add(obstacleToSpawn);
        }

        Destroy(nextObstaclesSpawned[nextObstaclesSpawned.Count - 1].gameObject);
        nextObstaclesSpawned.RemoveAt(nextObstaclesSpawned.Count - 1);
    }


    private int randomIndex(List<Obstacle> list)
    {
        int res = Random.Range(0, list.Count);

        while(res == previousIndex || res == prePreviousIndex){
            res = Random.Range(0, list.Count);
        }

        prePreviousIndex = previousIndex;
        previousIndex = res;

        return res;
    }

    
    void spawnStairs()
    {
        int num = Random.Range(1, 5);

        for(int i = 0; i < num; i++)
        {

            int position = Random.Range(1, nextFloorsSpawned.Count);

            while (stairsPositionSpawned.Contains(position))
            {
                position = Random.Range(1, nextFloorsSpawned.Count);
            }

            stairsPositionSpawned.Add(position);

            Vector3 stairPosition = new Vector3(nextFloorsSpawned[position].transform.position.x, -96f, nextFloorsSpawned[position].transform.position.z);
            Stairs stairsToSpawn = (Stairs)Instantiate(stairs, stairPosition, Quaternion.identity);

            if(nextStairsSpawned.Count == 0)
            {
                if(zone == "Community" && level != 0)
                {
                    stairsToSpawn.goToLevel = level - 1;
                    stairsToSpawn.text.text = stairsToSpawn.goToLevel.ToString();
                } 
                else if(zone == "University" && level != 4)
                {
                    stairsToSpawn.goToLevel = level + 1;
                    stairsToSpawn.text.text = stairsToSpawn.goToLevel.ToString();
                }
            }

            checkStairsPosition(stairsToSpawn);
            nextStairsSpawned.Add(stairsToSpawn); 
        }
    }

    private void checkStairsPosition(Stairs stairs)
    {
        for (int i = 0; i < nextObstaclesSpawned.Count; i++)
        {
            Obstacle obstacleChecked = nextObstaclesSpawned[i];

            bool b1 = stairs.transform.position.x >= obstacleChecked.transform.position.x;
            bool b2 = stairs.transform.position.x <= obstacleChecked.transform.position.x;
            bool b3 = stairs.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;

            bool b4 = stairs.exitPoint.transform.position.x >= obstacleChecked.transform.position.x;
            bool b5 = stairs.exitPoint.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;
            bool b6 = stairs.exitPoint.transform.position.x >= obstacleChecked.exitPoint.transform.position.x;

            if ((b1 && b3) || (b4 && b5) || (b2 && b5) || (b3 && b6))
            {
                stairs.transform.position = new Vector3(obstacleChecked.exitPoint.transform.position.x + 32f, stairs.transform.position.y, stairs.transform.position.z);
                break;
            }
        }
    }


    void spawnShortcut()
    {
        if (Random.Range(0, 2) == 0)
        {
            if (zone == "Metropolis")
            {
                int start;
                if (shortcuts[0].start == "Beginning")
                {
                    start = Random.Range(0, 21);
                }
                else
                {
                    start = Random.Range(20, 41);
                }

                Vector3 shortcutPosition = new Vector3(nextFloorsSpawned[start].transform.position.x, -96f, nextFloorsSpawned[start].transform.position.z);
                nextShorcutSpawned = (Shortcut)Instantiate(shortcuts.Find(x => x.zone == zone), shortcutPosition, Quaternion.identity);
                checkShortcutPosition();
            }
            else
            {
                if((zone == "Community" && level == 4) || (zone == "University" && level == 2)) 
                {
                    int randomIndex = Random.Range(0, nextFloorsSpawned.Count);
                    while (stairsPositionSpawned.Contains(randomIndex))
                    {
                        randomIndex = Random.Range(0, nextFloorsSpawned.Count);
                    }
                    
                    Vector3 shortcutPosition = new Vector3(nextFloorsSpawned[randomIndex].transform.position.x, -96f, nextFloorsSpawned[randomIndex].transform.position.z);
                    nextShorcutSpawned = (Shortcut)Instantiate(shortcuts.Find(x => x.zone == zone), shortcutPosition, Quaternion.identity);
                    checkShortcutPosition();
                }
            }
        }    
    }


    private void checkShortcutPosition()
    {
        for (int i = 0; i < nextObstaclesSpawned.Count; i++)
        {
            Obstacle obstacleChecked = nextObstaclesSpawned[i];

            bool b1 = nextShorcutSpawned.transform.position.x >= obstacleChecked.transform.position.x;
            bool b2 = nextShorcutSpawned.transform.position.x <= obstacleChecked.transform.position.x;
            bool b3 = nextShorcutSpawned.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;

            bool b4 = nextShorcutSpawned.exitPoint.transform.position.x >= obstacleChecked.transform.position.x;
            bool b5 = nextShorcutSpawned.exitPoint.transform.position.x <= obstacleChecked.exitPoint.transform.position.x;
            bool b6 = nextShorcutSpawned.exitPoint.transform.position.x >= obstacleChecked.exitPoint.transform.position.x;

            if ((b1 && b3) || (b4 && b5) || (b2 && b5) || (b3 && b6))
            {
                nextShorcutSpawned.transform.position = new Vector3(obstacleChecked.exitPoint.transform.position.x + 32f, nextShorcutSpawned.transform.position.y, nextShorcutSpawned.transform.position.z);
                break;
            }
        }
    }

    private void clearLevel()
    {
        if(floorsSpawned.Count > 0)
        {
            destroyFloors(floorsSpawned);
            destroyBackgrounds(backgroundSpawned);
            destroyObstacles(obstaclesSpawned);
            destroyStairs(stairsSpawned);
            if(shorcutSpawned != null)
            {
                Destroy(shorcutSpawned.gameObject);
                shorcutSpawned = null;
            }
        }

        floorsSpawned.AddRange(nextFloorsSpawned);
        backgroundSpawned.AddRange(nextBackgroundSpawned);
        obstaclesSpawned.AddRange(nextObstaclesSpawned);
        stairsSpawned.AddRange(nextStairsSpawned);
        shorcutSpawned = nextShorcutSpawned;

        nextFloorsSpawned.Clear();
        nextBackgroundSpawned.Clear();
        nextObstaclesSpawned.Clear();
        nextStairsSpawned.Clear();
        stairsPositionSpawned.Clear();
        nextShorcutSpawned = null;
    }

    private void destroyFloors(List<Floor> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }

        list.Clear();
    }

    private void destroyBackgrounds(List<Background> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }

        list.Clear();
    }

    private void destroyObstacles(List<Obstacle> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }

        list.Clear();
    }

    private void destroyStairs(List<Stairs> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }

        list.Clear();
    }

    public void useStairOrElevator()
    {
        spawnFloorsAndBackgrounds(floorsSpawned[floorsSpawned.Count - 1].exitPoint.transform.position);

        spawnObstacles();

        spawnStairs();

        spawnShortcut();

        if (zone == "University" && level == 4)
        {
            finish = (Finish)Instantiate(finish, nextFloorsSpawned[nextFloorsSpawned.Count - 1].exitPoint.transform.position, Quaternion.identity);
        }

        int sartingFloor = (level == 0 || level == 4) ? 0 : nextFloorsSpawned.Count - 1;

        float positionXToGo = (nextFloorsSpawned[sartingFloor].exitPoint.transform.position.x + nextFloorsSpawned[sartingFloor].transform.position.x) / 2;
        Player.sharedInstance.transform.position = new Vector3(positionXToGo, -95f, nextFloorsSpawned[sartingFloor].transform.position.z);
        CameraFollow.sharedInstance.transform.position = new Vector3(Player.sharedInstance.transform.position.x, 0.12f, -10f);

        clearLevel();

        changeLevel = false;
    }

}
