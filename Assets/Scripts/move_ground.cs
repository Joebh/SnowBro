using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class move_ground : MonoBehaviour {
    
    public GameObject firstPiece;
    public GameObject secondPiece;
    public GameObject[] hazards;

    private GameObject pieceToDrawOn;
    private int objectsToCreate = 2;

    // Use this for initialization
    void Start () {
        Vector3 newVector = new Vector3(0, 50f, 0);
        Vector3 offset = firstPiece.transform.TransformVector(newVector);

        secondPiece.transform.position = new Vector3(0, 
            firstPiece.transform.position.y - offset.y, 
            firstPiece.transform.position.z - offset.z);

        AddHazards(firstPiece);

        pieceToDrawOn = firstPiece;
        AddHazards(secondPiece);
    }

    void AddHazards(GameObject piece)
    {
        for (var i = 0; i < objectsToCreate; i++)
        {
            GameObject obj = hazards[Random.Range(0, hazards.GetLength(0))];

            var yMin = (50 / objectsToCreate) * i;
            var yMax = (50 / objectsToCreate) * (i + 1);

            Vector3 vector = piece.transform.TransformVector(new Vector3(Random.Range(-6, 6), Random.Range(yMin, yMax), 1.5f));

            vector.x = piece.transform.position.x - vector.x;
            vector.y = piece.transform.position.y - vector.y;
            vector.z = piece.transform.position.z - vector.z;

            Instantiate(obj, vector, Quaternion.identity, piece.transform);
        }       
    }

    void UpdatePositionAndSpeed(GameObject pieceToMove, GameObject otherPiece)
    {
        Vector3 offset = otherPiece.transform.TransformVector(new Vector3(0, 50f, 0));

        pieceToMove.transform.position = new Vector3(0,
            otherPiece.transform.position.y - offset.y,
            otherPiece.transform.position.z - offset.z);

        // increase speed
        AutoMoveAndRotate pieceToMoveScript = pieceToMove.GetComponent<AutoMoveAndRotate>();
        pieceToMoveScript.moveUnitsPerSecond.value = new Vector3(0, pieceToMoveScript.moveUnitsPerSecond.value.y + 1, 0);

        AutoMoveAndRotate otherMoveScript = otherPiece.GetComponent<AutoMoveAndRotate>();
        otherMoveScript.moveUnitsPerSecond.value = new Vector3(0, otherMoveScript.moveUnitsPerSecond.value.y + 1, 0);

        AddHazards(pieceToMove);
    }
        
    // Update is called once per frame
    void Update () {
        // change angle of parent
        if (firstPiece.transform.parent.transform.eulerAngles.x > 350)
        {
            firstPiece.transform.parent.transform.localEulerAngles = new Vector3(
                firstPiece.transform.parent.transform.localEulerAngles.x - (.5f * Time.deltaTime), 0, 0);
        }
       
        if (firstPiece.transform.position.y > 50f)
        {
            UpdatePositionAndSpeed(firstPiece, secondPiece);
        }
        if (secondPiece.transform.position.y > 50f)
        {
            UpdatePositionAndSpeed(secondPiece, firstPiece);
        }
    }
}
