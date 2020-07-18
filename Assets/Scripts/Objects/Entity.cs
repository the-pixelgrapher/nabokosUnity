using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    public Vector2 gridPos;

    public enum Rot
    {
        Right,
        Up,
        Left,
        Down
    }
    public Rot curRot;
    public List<Rot> rotRecord;
    public List<Vector2> posRecord;


    void Start()
    {
        gridPos = transform.position;
    }

    void Update()
    {
        
    }

    public void RecordState() 
    {
        rotRecord.Add(curRot);
        posRecord.Add(gridPos);
    }

    public void Undo()
    {
        Debug.Log("UndoingMove");

        // Undo position
        gridPos = posRecord[posRecord.Count - 2];
        transform.DOMove(gridPos, 0.1667f);
        posRecord.RemoveAt(posRecord.Count - 1);
    }
}
