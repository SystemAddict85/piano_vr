﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Types;

public class EnemyTracker : MonoBehaviour
{
    public Enemy currentTrackingEnemy;

    public TrackingMonitor trackingMonitor;
    public NoteMonitor noteMonitor;

    [SerializeField]
    private Instrument trackingSounds;

    private AudioSource audioSource;

    public ShipReticle shipReticle;

    public string str_hardest_prompt = "Use your ears...";

    //[SerializeField]
    //private NoteMonitor noteMonitor;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // TODO: USE WHEN SWITCH IS AVAILABLE
    //public void TrackNextEnemy()
    //{
    //    if (EnemyManager.Instance.m_liveEnemies.Count > 0) {
    //        var index = EnemyManager.Instance.m_liveEnemies.IndexOf(currentTrackingEnemy);
    //        ++index;
    //        if (index == EnemyManager.Instance.m_liveEnemies.Count)
    //        {
    //            index = 0;
    //        }
    //        TrackEnemy(EnemyManager.Instance.m_liveEnemies[index]);            
    //    }
    //}

    public void TrackEnemy(Enemy e)
    {
        StopTracking();
        currentTrackingEnemy = e;
        //currentTrackingEnemy.GetComponent<EnemyReticle>().AttachReticle();
        //currentTrackingEnemy.GetComponent<EnemyReticle>().setActiveTracking(true);
        UpdateMonitors("Listening...");
        //UpdateMonitors();
        PlayTrackingSound();
        //shipReticle.SetTracking(true);
        //UpdateReticle(e);
        e.setCurrentlyTrackedEnemy(true);
        //TurnOnTrackingAndHintLights();

    }



    //Left here in case we change our minds
    //private void TurnOnTrackingAndHintLights()
    //{
    //    if (LevelManager.Instance.currentHandicaps.numberOfHintKeys > 3)
    //    {
    //        //TODO Check if the fourth note is needed to complete the chord
    //        //GameManager.Instance.piano.keys.Where(x => x.note == currentTrackingEnemy.chord.FourthNote).First().EnableLightHint();
    //    }
    //    if (LevelManager.Instance.currentHandicaps.numberOfHintKeys > 2)
    //    {
    //        GameManager.Instance.piano.keys.Where(x => x.note == currentTrackingEnemy.chord.ThirdNote).First().EnableLightHint();
    //    }
    //    if (LevelManager.Instance.currentHandicaps.numberOfHintKeys > 1)
    //    {
    //        GameManager.Instance.piano.keys.Where(x => x.note == currentTrackingEnemy.chord.SecondNote).First().EnableLightHint();
    //    }
    //    if (LevelManager.Instance.currentHandicaps.numberOfHintKeys > 0)
    //    {
    //        GameManager.Instance.piano.keys.Where(x => x.note == currentTrackingEnemy.chord.RootNote).First().EnableLightTracking();
    //    }
    //}

    private void PlayTrackingSound()
    {
        var clip = trackingSounds.GetPianoNoteAudio(currentTrackingEnemy.chord.RootNote);
        audioSource.clip = clip;
        audioSource.Play();

    }

    public void StopTracking()
    {
        audioSource.Stop();
        currentTrackingEnemy = null;
        RestoreKeyLights();

        
    }

    private void RestoreKeyLights()
    {
        foreach (var k in GameManager.Instance.piano.keys)
        {
            k.RestoreKeyColor();
        }
    }

    // called outside of class now so that the monitor and reticle are synced
    public void UpdateMonitors()
    {
        trackingMonitor.PrintTrackingToScreen(currentTrackingEnemy.chord.ToString());

        if (LevelManager.Instance.currentHandicaps.showNotesOnDisplay)
        {
            noteMonitor.UpdateNoteMonitor(currentTrackingEnemy);
        }
        else
        {
            noteMonitor.UpdateNoteMonitor(null);
        }
        //else
        //{
        //    //TODO Show blank or have stages of showing
        //}
    }

    // Writing to monitors in between the time the enemy is spawned and the actual tracking occurs
    public void UpdateMonitors(string str)
    {

        if (LevelManager.Instance.currentHandicaps.showUpdatedChordsOnDisplay)
        {
            trackingMonitor.PrintTrackingToScreenEmpty(str);
            noteMonitor.WriteNoteMonitor(str);
        }
        else
        {
            trackingMonitor.PrintTrackingToScreenEmpty(str_hardest_prompt);
        }
    }

    public void CheckNoteToEnemy(MusicalNote note)
    {
        if (currentTrackingEnemy && currentTrackingEnemy.CheckNoteToChord(note))//Correct
        {
            if (LevelManager.Instance.currentHandicaps.showUpdatedChordsOnDisplay)
            {
                noteMonitor.UpdateNoteMonitor(currentTrackingEnemy);
                Debug.Log($"Note hit: {note} ");
            }
        }
        else//Incorrect
        {
            if (LevelManager.Instance.currentHandicaps.showUpdatedChordsOnDisplay)
            {
                Debug.Log("wrong note");
            }
        }
    }



}
