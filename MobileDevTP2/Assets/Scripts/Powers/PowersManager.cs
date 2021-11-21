using System.Collections.Generic;
using UnityEngine;

public class PowersManager : MonoBehaviour
{
    [SerializeField] PowersData data;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] Transform wallsEmpty;
    [SerializeField] List<GameObject> activeWalls;
    [SerializeField] List<GameObject> disabledWalls;

    private void Start()
    {
        for (int i = 0; i < data.wallsAtTheSameTime; i++)
        {
            disabledWalls.Add(Instantiate(wallPrefab, wallsEmpty)); //create
            disabledWalls[i].SetActive(false); //disable
            disabledWalls[i].GetComponent<WallController>().WallDestroyed += OnWallDestroyed; //link
        }
    }

    //Event Receivers
    public void OnWallInputReceived(Vector2 midPos, Vector2 direction, float length)
    {
        if (disabledWalls.Count < 1) return;

        //Set
        Vector3 newScale = wallPrefab.GetComponent<SpriteRenderer>().size;
        newScale.x = length > data.maxWallLength ? data.maxWallLength : length; //set length inside range
        disabledWalls[0].GetComponent<SpriteRenderer>().size = newScale;
        disabledWalls[0].transform.position = midPos;
        disabledWalls[0].transform.up = direction;

        //Update Collider
        disabledWalls[0].GetComponent<BoxCollider2D>().size = newScale;

        //Update Data
        disabledWalls[0].GetComponent<WallController>().SetData(data.wallData);

        //Activate
        disabledWalls[0].SetActive(true);
        
        //Change list
        activeWalls.Add(disabledWalls[0]);
        disabledWalls.RemoveAt(0);
    }
    void OnWallDestroyed(GameObject wallDestroyed)
    {
        activeWalls.Remove(wallDestroyed);
        disabledWalls.Add(wallDestroyed);
    }
}