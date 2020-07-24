using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level;
    public bool levelComplete;

    [SerializeField]
    private int moveNumber = -1;

    public List<Entity> entities;

    private void Start()
    {
        // Find all entities (undoable objects)
        var tempList = FindObjectsOfType<Entity>();
        for (int i = 0; i < tempList.Length; i++)
        {
            entities.Add(tempList[i]);
        }

        GlobalRecordState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GlobalUndo();
        }
    }

    // Record state for every Entity for undo
    public void GlobalRecordState()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].RecordState();
        }

        moveNumber++;
    }

    // Undo state of every Entity
    public void GlobalUndo()
    {
        if (moveNumber > 0)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Undo(moveNumber - 1);
            }

            moveNumber--;
        }
    }
}