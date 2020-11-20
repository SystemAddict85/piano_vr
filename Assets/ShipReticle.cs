using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipReticle : MonoBehaviour
{
    [Header("Reticle Info")]
    public GameObject Screen;
    public bool isTracking = false;
    public float trackingSpeed = 0.05f;
    public float wanderSpeed = 0.05f;
    public Vector3 currPoint = Vector3.zero;

    public GameObject DestroyCollider = null;
    public Vector3 wanderPt;

    public float wanderRadius = 1.0f;
    public float maxRayDistance = 200.0f;
    private float threshhold = 0.05f;

    public Material wanderMat = null;
    public Material trackMat = null;

    private void Start()
    {
        wanderPt = DestroyCollider.gameObject.transform.position;
        gameObject.transform.position = DestroyCollider.gameObject.transform.position;
        SetWanderMat();
        SetTracking(false);
    }

    public void SetWanderMat()
    {
        gameObject.GetComponent<MeshRenderer>().material = wanderMat;
    }

    public void SetTrackMat()
    {
        gameObject.GetComponent<MeshRenderer>().material = trackMat;
    }

    public void Activate(bool b)
    {
        gameObject.SetActive(b);
    }

    public void SetTracking(bool b) 
    {
        
        isTracking = b;

        if (isTracking)
        {
            // hide note display entirely if there will be nothing shown on it
            if (LevelManager.Instance.currentHandicaps.showNotesOnDisplay)
            {
                Screen.SetActive(true);
            }
            else
            {
                Screen.SetActive(false);
            }

            EnemyManager.Instance.tracker.UpdateMonitors();
            SetTrackMat();
        }
        else
        {
            Screen.SetActive(false);
            SetWanderMat();
        }
        currPoint = DestroyCollider.gameObject.transform.position;
    }


    private void GoToCurrentPoint()
    {
        //Debug.Log("currPos: " + currPoint);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currPoint, trackingSpeed);
    }

    public void SetCurrPoint(Vector3 p)
    {
        // someone else will set this outside this class

        currPoint = p;
    }

    public void Wander()
    {
        // at wander pt
        if (Vector3.Distance(transform.position, wanderPt) < threshhold)
        {
            Vector2 pt = Random.insideUnitCircle;
            wanderPt.x = pt.x + DestroyCollider.gameObject.transform.position.x;
            wanderPt.y = pt.y + DestroyCollider.gameObject.transform.position.y;
            //Debug.Log("wander pt: " + pt);
        }
        // wandering
        else
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, wanderPt, wanderSpeed);
        }
        
    }

    public void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(DestroyCollider.transform.position, wanderRadius);
        Gizmos.DrawWireSphere(DestroyCollider.gameObject.transform.position, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracking)
        {
            //Debug.Log("isTracking");
            GoToCurrentPoint();
        }
        else
        {
            Wander();
        }
    }
}
