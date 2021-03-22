using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Floor floor;
    private List<Floor> allFloors = new List<Floor>();

    public Finish finish;

    private void Awake()
    {
        int length = 3;
        for (int i = 0; i < length; i++)
        {
            Floor floors = (Floor) Instantiate(floor, this.transform.position, Quaternion.identity);

            if (i != 0)
            {
                floors.transform.position = allFloors[i - 1].exitPoint.transform.position;
            }

            allFloors.Add(floors);

            if(i == length - 1)
            {
                Instantiate(finish, allFloors[i].exitPoint.transform.position, Quaternion.identity);
            }
        }
    }
}
