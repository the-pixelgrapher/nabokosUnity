using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level;
    public bool levelComplete;
    public int moveNumber;

    public List<Entity> entities;

    void Start()
    {
        // Find all entities (undoable objects)
        var tempList = FindObjectsOfType<Entity>();
        for (int i = 0; i < tempList.Length; i++)
        {
            entities.Add(tempList[i]);
        }
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GlobalUndo();
        }
    }

    public void GlobalRecordState()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].RecordState();
        }
    }

    public void GlobalUndo()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Undo();
        }
    }

}
