using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReticle : MonoBehaviour
{
    [SerializeField]
    bool enemyLeftSpawn = false;
    bool currentlyTracking = false;
    public GameObject reticle;
    public GameObject objective;
    public float reticleSpeedLimiter;

    [SerializeField]
    public float maxRayDistance = 300.0f;

    [Header("Ship Reticle")]
    [SerializeField]
    public ShipReticle shipReticle;
    
    public void AttachHiddenReticle()
    {

        // allows reticle to be seen but hides initially because the reticle
        // is initially spawned on the enemy object
        if (reticle != null)
        {
            reticle.gameObject.SetActive(true);
            reticle.GetComponent<Renderer>().enabled = false;
        }
        
    }

    public void ShowReticle()
    {

        reticle.GetComponent<Renderer>().enabled = true;
    }

    public void RemoveReticle()
    {
        if (reticle != null)
        {
            reticle.gameObject.SetActive(false);
        }
        
    }

    public void setActiveTracking(bool b)
    {
        currentlyTracking = b;
        checkStatus();

        //shipReticle.SetTracking(b);
    }

    public void setEnemyLeftSpawnZone(bool eLeftZone)
    {
        enemyLeftSpawn = eLeftZone;
        // placing back in zone
        if (!enemyLeftSpawn)
        {
            RemoveReticle();
        }
        checkStatus();
        
    }

    public void checkStatus()
    {
        if (currentlyTracking && enemyLeftSpawn)
        {
            AttachHiddenReticle();
        }
    }

    public void SendToShip(Vector3 p)
    {
        shipReticle.SetCurrPoint(p);
    }

    public void DrawOnScreen()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(gameObject.transform.position, transform.forward);

        Debug.DrawRay(gameObject.transform.position, transform.forward * maxRayDistance);
        if (Physics.Raycast(landingRay, out hit, maxRayDistance))
        {
            if (hit.collider.tag == "DestroyCollider")
            {
                Debug.Log("hit.pt: " + hit.point);
                //DrawReticle(hit.point);
                //ShowReticle();

                SendToShip(hit.point);

            }
        }

    }

    public void DrawReticle(Vector3 p)
    {
        //reticle.transform.position = Vector3.MoveTowards(reticle.transform.position, p, 1.0f);
        reticle.transform.position = p;
    }

    private void Update()
    {
        GameObject obj = GetComponent<Enemy>().m_objective;
        //Debug.Log("enemyLeftSpawn: " + enemyLeftSpawn + " | currentlyTracking: " + currentlyTracking);
        if ((enemyLeftSpawn) && (currentlyTracking) && (obj != null))
        {


            //Debug.Log("enemyLeftSpawn: " + enemyLeftSpawn);
            //reticle.gameObject.transform.LookAt(obj.transform.position);
            //reticle.gameObject.transform.Rotate(new Vector3(0, 1, 0), 90.0f);
            //Vector3 p = reticle.gameObject.transform.position;
            //p.y += Mathf.Sin(Time.time) / reticleSpeedLimiter;
            //p.x += Mathf.Cos(Time.time) / reticleSpeedLimiter ;


            //reticle.gameObject.transform.position = p;
            DrawOnScreen();

        }


        
    }
}
