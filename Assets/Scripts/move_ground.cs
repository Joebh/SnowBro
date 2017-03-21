using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class move_ground : MonoBehaviour {
    
    public GameObject firstPiece;
    public GameObject secondPiece;
    public GameObject[] hazards;
    public float speed = 0;
    public float distanceTravelled = 0;

    private float speedMagnitude = .04f;
    private int objectsToCreate = 2;

    // Use this for initialization
    void Start () {
        Vector3 newVector = new Vector3(0, 50f, 0);
        Vector3 offset = firstPiece.transform.TransformVector(newVector);

        secondPiece.transform.position = new Vector3(0, 
            firstPiece.transform.position.y - offset.y, 
            firstPiece.transform.position.z - offset.z);

        GameObject obj = hazards[Random.Range(0, hazards.GetLength(0))];

        var yMin = (50 / objectsToCreate) * 1;
        var yMax = (50 / objectsToCreate) * 2;

        Vector3 vector = firstPiece.transform.TransformVector(new Vector3(Random.Range(-6, 6), Random.Range(yMin, yMax), 1.5f));

        vector.x = firstPiece.transform.position.x - vector.x;
        vector.y = firstPiece.transform.position.y - vector.y;
        vector.z = firstPiece.transform.position.z - vector.z;

        Instantiate(obj, vector, Quaternion.identity, firstPiece.transform);
        
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

    void UpdatePosition(GameObject pieceToMove, GameObject otherPiece)
    {
        Vector3 offset = otherPiece.transform.TransformVector(new Vector3(0, 50f, 0));

        pieceToMove.transform.position = new Vector3(0,
            otherPiece.transform.position.y - offset.y,
            otherPiece.transform.position.z - offset.z);

        speedMagnitude += 0.002f;

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
            UpdatePosition(firstPiece, secondPiece);
        }
        if (secondPiece.transform.position.y > 50f)
        {
            UpdatePosition(secondPiece, firstPiece);
        }

        Vector3 offset = firstPiece.transform.TransformVector(new Vector3(0, speed * speedMagnitude, 0));

        distanceTravelled += offset.magnitude;

        firstPiece.transform.position = new Vector3(0,
            firstPiece.transform.position.y + offset.y,
            firstPiece.transform.position.z + offset.z);

        secondPiece.transform.position = new Vector3(0,
            secondPiece.transform.position.y + offset.y,
            secondPiece.transform.position.z + offset.z);

    }
}
