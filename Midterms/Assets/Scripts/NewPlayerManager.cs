using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0;
    
    private Vector3 origPos, targetPos;
    private float moveTimer = 0f;
    private Vector3 currentDirection = Vector3.zero;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        CreateBodyParts();
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        SnakeMovement();
    }

    
    void SnakeMovement()
    {
        
        //snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * 280 * Time.deltaTime;
        //if(Input.GetAxis("Horizontal") != 0)
        //    snakeBody[0].transform.Rotate(new Vector3(0, 0, -180 * Time.deltaTime * Input.GetAxis("Horizontal")));
        
        
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Vector3.down)
        {
            currentDirection = Vector3.up;
            // Debug.Log("W pressed");
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentDirection != Vector3.up)
        {
            currentDirection = Vector3.down;
            // Debug.Log("S pressed");
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentDirection != Vector3.right)
        {
            currentDirection = Vector3.left;
            // Debug.Log("A pressed");
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentDirection != Vector3.left)
        {
            currentDirection = Vector3.right;
            // Debug.Log("D pressed");
        }
        
        moveTimer += Time.deltaTime;
        if (moveTimer >= 0.2f)
        {
            moveTimer = 0f; // Reset timer after movement
            
            MarkerManager markM = snakeBody[0].GetComponent<MarkerManager>();
            markM.markerList[0].position = snakeBody[0].transform.position; 
            markM.markerList[0].rotation = snakeBody[0].transform.rotation;
            
            snakeBody[0].transform.position += currentDirection; // Move in the current direction

            if (markM.markerList.Count >= snakeBody.Count + 1)
            {
                markM.markerList.RemoveAt(0);
            }
        }
        // Start moving in the new direction
        //if (currentDirection != Vector3.zero)
        //    StartCoroutine(MovePlayer(currentDirection));
        
        
        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
                
            }
        }
        
    }
    
    
    /*
    void SnakeMovement()
    {
        // Handle input for direction
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Vector3.down)
            currentDirection = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.S) && currentDirection != Vector3.up)
            currentDirection = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.A) && currentDirection != Vector3.right)
            currentDirection = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D) && currentDirection != Vector3.left)
            currentDirection = Vector3.right;

        // Move head and store position
        moveTimer += Time.deltaTime;
        if (moveTimer >= 0.2f)
        {
            moveTimer = 0f;
            snakeBody[0].transform.position += currentDirection;

            // Store head's new position in MarkerManager
            //MarkerManager headMarkerManager = snakeBody[0].GetComponent<MarkerManager>();
            //headMarkerManager.Add(new Marker(snakeBody[0].transform.position, snakeBody[0].transform.rotation));
        }

        // Move each body part to the previous one's last position
        for (int i = 1; i < snakeBody.Count; i++)
        {
            MarkerManager previousMarkerManager = snakeBody[i - 1].GetComponent<MarkerManager>();
            MarkerManager currentMarkerManager = snakeBody[i].GetComponent<MarkerManager>();

           
                // Set position and rotation to the marker's position
                snakeBody[i].transform.position = previousMarkerManager.markerList[0].position;
                snakeBody[i].transform.rotation = previousMarkerManager.markerList[0].rotation;

                // Move marker to current segment and limit size
                //currentMarkerManager.AddMarker(previousMarkerManager.markerList[0]);
                previousMarkerManager.markerList.RemoveAt(0);

                // Optional: Limit markers stored in markerList
                if (currentMarkerManager.markerList.Count > 10) 
                    currentMarkerManager.markerList.RemoveAt(0);
            
        }
    }
    */
    
    /*
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
            Debug.Log("Elapsed Time: " + elapsedTime + "timeToMove: "+ timeToMove);
        }

        transform.position = targetPos;
        isMoving = false;
    }
    */

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
                temp1.AddComponent<MarkerManager>();
            if (!temp1.GetComponent<Rigidbody2D>())
            {
                temp1.AddComponent<Rigidbody2D>();
                temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        
        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation,transform);
            if(!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }
}
