using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        gridPos = transform.position;
        rotRecord = new List<Rot>();
        posRecord = new List<Vector2>();
    }

    public void RecordState()
    {
        rotRecord.Add(curRot);
        posRecord.Add(gridPos);
    }

    public void Undo(int _index)
    {
        // Undo position
        gridPos = posRecord[_index];
        transform.DOMove(gridPos, 0.1667f);
        posRecord.RemoveAt(posRecord.Count - 1);

        // Undo rotation
        curRot = rotRecord[_index];
        switch (curRot)
        {
            case Rot.Right:
                transform.DORotate(new Vector3(0, 0, 0), 0.1667f);
                break;

            case Rot.Up:
                transform.DORotate(new Vector3(0, 0, 90), 0.1667f);
                break;

            case Rot.Left:
                transform.DORotate(new Vector3(0, 0, 180), 0.1667f);
                break;

            case Rot.Down:
                transform.DORotate(new Vector3(0, 0, 270), 0.1667f);
                break;
        }
        rotRecord.RemoveAt(rotRecord.Count - 1);
    }
}