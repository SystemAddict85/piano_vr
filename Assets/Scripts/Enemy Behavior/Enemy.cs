using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Types;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum FlightMode
    {
        FLY_TO_PLAYER,
        FLY_IDLE,
        FLY_ZOOM,
        FLIGHT_MODE_COUNT
    }

    [Header("Position Data")]
    public Spawner m_spawner;
    public Vector3 m_startPos;
    public Vector3 m_endPos;
    public float m_stepSize;
    public float m_reg_speed;
    public float m_zoom_speed;
    public float m_rotateSpeed;
    public GameObject m_objective;
    public GameObject m_destroyCollider;
    public GameObject m_spawnNextEnemyCollider;
    public GameObject m_showReticleCollider;

    [Space]
    [Header("Chord Info")]
    public Chord chord;
    //public ChordType chordType = ChordType.Major;
    private MeshRenderer m_meshRenderer;
    //public List<Material> m_materials;

    public bool hasRootNoteBeenPlayed = false;
    public bool hasSecondNoteBeenPlayed = false;
    public bool hasThirdNoteBeenPlayed = false;

    [Header("Death FX")]
    public AudioClip deathSound;

    [Header("Pathing Info")]
    public IdlePath pathingBox;

    public FlightMode m_currMode = FlightMode.FLY_IDLE;
    private bool hasPassedThreshold;

    [Header("Reticle Info")]
    public bool isCurrentlyTrackedEnemy = false;
    public ShipReticle shipReticle;
    public bool leftSpawnZone = false;
    public LayerMask reticleRaycastLayerMask;


    // Start is called before the first frame update
    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        //PrintEnemy();
    }

    public void SetObjective(GameObject obj, GameObject destCollider)
    {
        m_objective = obj;
        m_destroyCollider = destCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTrigger");
        if (other.gameObject == m_destroyCollider || other.gameObject == m_objective)
        {
            //Debug.Log("Destroying...");
            PoolDestroy(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("other object: " + other.name);
        if (other.gameObject == m_spawnNextEnemyCollider)
        {
            hasPassedThreshold = true;
         //   Debug.Log("Spawning next enemy...");
            EnemyManager.Instance.GetNextEnemy();
            //GameManager.Instance.NextEnemy();
        }
        if (other.gameObject == m_showReticleCollider.gameObject)
        {
            //Debug.Log("Enemy leaving spawn box");
            GetComponent<EnemyReticle>().setEnemyLeftSpawnZone(true); // deprecated
            HasLeftSpawnZone(true);
            m_currMode = FlightMode.FLY_TO_PLAYER;

        }
    }

    public void HasLeftSpawnZone(bool b)
    {
        leftSpawnZone = b;
    }

    

    private void OnEnable()
    {
        //InputManager.Instance.currentEnemy = this;
        spawnIdleEnemy();

    }

    private void OnDisable()
    {        
        hideEnemy();

    }

    public void beginIdle()
    {
        m_currMode = FlightMode.FLY_IDLE;
    }

    public void spawnLiveEnemy()
    {
        // TODO: add an animation to target enemies that are "live"
        m_currMode = FlightMode.FLY_ZOOM;
        m_reg_speed = UnityEngine.Random.Range(LevelManager.Instance.currentStageObject.minEnemySpeed, LevelManager.Instance.currentStageObject.minEnemySpeed);
        //transform.position = m_startPos; 
        //ChangeMaterial();
        //m_endPos = m_objective.transform.position;
        //// add this enemy to m_liveEnemies queue
        //EnemyManager.Instance.AddLiveEnemy(this);
        EnemyManager.Instance.AddLiveEnemy(this);
        EnemyManager.Instance.RemoveIdleEnemy(this);
    }

    private void spawnIdleEnemy()
    {
        m_currMode = FlightMode.FLY_IDLE;
        transform.position = m_startPos;
        ChangeMaterial();
        m_endPos = m_objective.transform.position;
        // add this enemy to m_liveEnemies queue
        //EnemyManager.Instance.AddLiveEnemy(this);
        EnemyManager.Instance.AddIdleEnemy(this);
    }

    private void hideEnemy()
    {
        // place back into spawner queue
        hasRootNoteBeenPlayed = false;
        hasSecondNoteBeenPlayed = false;
        hasThirdNoteBeenPlayed = false;

        isCurrentlyTrackedEnemy = false;
        HasLeftSpawnZone(false);
        //shipReticle.SetTracking(false);
        // remove from InputManager's live queue
        EnemyManager.Instance.RemoveLiveEnemy(this);
        m_spawner.m_idleEnemies.Remove(this.gameObject);
        m_spawner.m_hiddenEnemies.Add(this.gameObject);
        EnemyManager.Instance.PoolDestroyEnemy(this);
        NotifyGameManager();
    }

    public void NotifyGameManager()
    {
        GameManager.Instance.CheckGameState(hasPassedThreshold);
        hasPassedThreshold = false;
    }

    private void ChangeMaterial()
    {
        //Debug.Log("chordType: " + chord.chordType);
        switch (chord.chordType)
        {
            case ChordType.Major:
            case ChordType.NUM_CHORDS:
            default:
                m_meshRenderer.material.color = GameManager.Instance.colors.majorColor;
                m_meshRenderer.material.SetColor("_EmissionColor", GameManager.Instance.colors.majorColor);
                break;
            case ChordType.Minor:
                m_meshRenderer.material.color = GameManager.Instance.colors.minorColor;
                m_meshRenderer.material.SetColor("_EmissionColor", GameManager.Instance.colors.minorColor);
                break;
            case ChordType.Diminished:
                m_meshRenderer.material.color = GameManager.Instance.colors.diminishedColor;
                m_meshRenderer.material.SetColor("_EmissionColor", GameManager.Instance.colors.diminishedColor);
                break;
        }
        //m_material = m_materials[(int)chord.chordType];
        //Debug.Log("m_material: " + m_material.name);
        //GetComponent<MeshRenderer>().material = m_material;
    }

    public void SetChord(MusicalNote note, ChordType chordType)
    {
        chord = new Chord(note, chordType);
    }

    public void setCurrentlyTrackedEnemy(bool b)
    {
        isCurrentlyTrackedEnemy = b;
    }

    public void UpdateReticle()
    {
        RaycastHit hit;
        Vector3 target = m_objective.transform.position - gameObject.transform.position;
        Ray landingRay = new Ray(gameObject.transform.position, target);

        Debug.DrawRay(gameObject.transform.position, target * shipReticle.maxRayDistance, Color.red);
        if (Physics.Raycast(landingRay, out hit, shipReticle.maxRayDistance, reticleRaycastLayerMask))
        {
            //Debug.Log("hit.collider.name: " + hit.collider.name);
            if (hit.collider.tag == "DestroyCollider")
            {
                //Debug.Log("hit.pt: " + hit.point);
                //DrawReticle(hit.point);
                //ShowReticle();

                SendToShip(hit.point);

            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        //Gizmos.DrawLine(transform.position, m_objective.transform.position);
    }

    public void SendToShip(Vector3 p)
    {
        shipReticle.SetTracking(true);
        shipReticle.SetCurrPoint(p);
    }

    // Update is called once per frame
    void Update()
    {

        
        if (m_currMode == FlightMode.FLY_TO_PLAYER || m_currMode == FlightMode.FLY_ZOOM)
        {
            //Debug.Log("mypos: " + gameObject.transform.position);
            
            float step = m_stepSize * Time.deltaTime;
            step *= (m_currMode == FlightMode.FLY_TO_PLAYER) ? m_reg_speed : m_zoom_speed;
            //Debug.Log("step: " + step);
            Vector3 p = Vector3.MoveTowards(gameObject.transform.position, m_objective.gameObject.transform.position, step);
            //Debug.Log("target_pos: " + m_objective.gameObject.transform.position);
            p[1] += Mathf.Sin(p[2]) / 1000;
            p[0] += Mathf.Cos(p[2]) / 1000;

            // pathing based on chord type
            //switch (chord.chordType)
            //{
            //    case ChordType.Major:
            //    case ChordType.NUM_CHORDS:
            //    default:
            //        // swirly 
            //        p[1] += Mathf.Sin(p[2]) / 30;
            //        p[0] += Mathf.Cos(p[2]) / 30;
            //        break;
            //    case ChordType.Minor:
            //        // downward descent
            //        p[1] -= Mathf.PerlinNoise(p[1], p[0]) / 20;
            //        p[0] -= Mathf.Sin(p[2]) / 20;
            //        break;
            //    case ChordType.Diminished:
            //        // ??? 
            //        p[1] += Mathf.Cos(p[2]) / 20;
            //        p[0] -= Mathf.PerlinNoise(p[1], p[2]) / 20;
            //        break;
            //}

            transform.position = p;


             //rotation towards m_objective
            float rotStep = m_rotateSpeed * Time.deltaTime;
            Vector3 targetDir = (m_objective.transform.position - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            if (isCurrentlyTrackedEnemy && leftSpawnZone)
            {
                //Debug.Log("SENDING RETICLE THE INFORMATION");
                UpdateReticle();
            }

        }
        else if (m_currMode == FlightMode.FLY_IDLE)
        {
            // handled by IdlePath class that is associated... kinda convoluted...
        }

    }

    public void PoolDestroy(bool isDamageToPlayer, bool playerDestroyedEnemy = false)
    {
        if(playerDestroyedEnemy)
        {
           StartCoroutine(ExplosionManager.Instance.ExplodeAt(transform));
        }
        if (isDamageToPlayer)
        {
            //  Debug.Log("Player Hit");
            EnemyManager.Instance.PlayHurtFX();
            LevelManager.Instance.winStreak = 0;
            LevelManager.Instance.failStreak++;
            GameManager.Instance.DamagePlayer(LevelManager.Instance.currentStageObject.enemyDamageAmount);

        }
        else
        {
            LevelManager.Instance.winStreak++;
            LevelManager.Instance.failStreak = 0;
            // TODO : activate flying animation here and place back into idle pool
        }
        GetComponent<EnemyReticle>().setEnemyLeftSpawnZone(false);
        //hideEnemy();
        //gameObject.SetActive(false);

    }

    public void PrintEnemy()
    {
        //Debug.Log("m_startPos: " + m_startPos.ToString("F4"));
        //Debug.Log("m_endPos: " + m_endPos.ToString("F4"));
    }

    public bool CheckNoteToChord(MusicalNote note)
    {
        var noteHit = false;
        if (note == chord.RootNote)
        {
            hasRootNoteBeenPlayed = true;
            noteHit = true;
            if (LevelManager.Instance.currentHandicaps.showColorOnKeysWhenHit)
            {
                GameManager.Instance.piano.keys.Where(x => x.note == chord.RootNote).First().EnableCorrectColor();
            }
        }
        else if (note == chord.SecondNote)
        {
            hasSecondNoteBeenPlayed = true;
            noteHit = true;
            if (LevelManager.Instance.currentHandicaps.showColorOnKeysWhenHit)
            {
                GameManager.Instance.piano.keys.Where(x => x.note == chord.SecondNote).First().EnableCorrectColor();
            }
        }
        else if (note == chord.ThirdNote)
        {
            hasThirdNoteBeenPlayed = true;
            noteHit = true;
            if (LevelManager.Instance.currentHandicaps.showColorOnKeysWhenHit)
            {
                GameManager.Instance.piano.keys.Where(x => x.note == chord.ThirdNote).First().EnableCorrectColor();
            }
        }
        else
        {
            if (LevelManager.Instance.currentHandicaps.showColorOnKeysWhenHit)
            {
                StartCoroutine(GameManager.Instance.piano.keys.Where(x => x.note == note).First().FlashIncorrectColor());
            }
            GameManager.Instance.DamagePlayer(LevelManager.Instance.currentStageObject.wrongNoteDamageAmount);
        }
        if (noteHit)
        {
            CheckForDeath();
        }

        return noteHit;
    }

    private void CheckForDeath()
    {
        if (hasRootNoteBeenPlayed && hasSecondNoteBeenPlayed && hasThirdNoteBeenPlayed)
        {
            GameManager.Instance.HealPlayer(LevelManager.Instance.currentStageObject.correctChordHealAmount);
            PoolDestroy(false, true);
        }
    }

    public void OnRemoved()
    {
        EnemyManager.Instance.audioSource.PlayOneShot(deathSound);
        EnemyManager.Instance.PlayExplosionFX(this.transform, 50);
    }
}