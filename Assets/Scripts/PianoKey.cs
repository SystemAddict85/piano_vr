using System.Collections;
using Types;
using UnityEngine;
using TMPro;

public class PianoKey : MonoBehaviour
{
    [HideInInspector]
    public AudioSource source;

    public MusicalNote note;
    [SerializeField]
    private float minimumWaitToPlay = .2f;
    public bool readyToPlay = true;

    public bool soundStarted = false;
    
    private Color originalColor;

    [SerializeField]
    private float touchLightIntensity = 1.2f;
    [SerializeField]
    private float lightIntensityDuration = .5f;
    private float startingIntensity;
    private float incorrectLightFlashDuration = 1f;

    private Vector3 offsetColor;

    public TextMeshProUGUI noteDisplay;
    private bool isFlashing = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        readyToPlay = true;

        startingIntensity = GetComponentInChildren<MeshRenderer>().material.GetFloat("_Intensity");

        originalColor = GetComponentInChildren<Renderer>().material.GetColor("_Color2");
        var color2 = GetComponentInChildren<Renderer>().material.GetColor("_Color1");

        var off1 = GetHSVFromRGB(originalColor);
        var off2 = GetHSVFromRGB(color2);
        offsetColor = off2 - off1;

        noteDisplay = GetComponentInChildren<TextMeshProUGUI>();
        ShowNote();
    }

    Vector3 GetHSVFromRGB(Color color)
    {
        float h, s, v;
        Color.RGBToHSV(originalColor, out h, out s, out v);
        return new Vector3(h, s, v);
    }

    Color ApplyOffsetColor(Color color)
    {
        var hsv = GetHSVFromRGB(color);
        var newHsv = hsv + offsetColor;
        return Color.HSVToRGB(newHsv.x, newHsv.y, newHsv.z);
    }

    void Update()//TODO Take out of Update
    {
        if (LevelManager.Instance.currentHandicaps.showNotesOnKeys ^ noteDisplay.gameObject.activeSelf)//XOR
        {
            noteDisplay.gameObject.SetActive(!noteDisplay.gameObject.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("HandController") && readyToPlay)
        {
            var hand = collision.gameObject.GetComponent<PianoHand>();
            StartCoroutine(ChangeLightIntensityOnTouch());
            if (hand && hand.readyToPlay)
            {
                readyToPlay = false;
                hand.readyToPlay = false;
                hand.HapticVibration();
                if (hand.currentSource != null && hand.currentSource != source)
                {
                    hand.currentSource.Stop();
                }
                hand.currentSource = source;
                source.Play();
                EnemyManager.Instance.tracker.CheckNoteToEnemy(note);
                StartCoroutine(WaitToPlayAgain(hand));
            }
        }

    }

    IEnumerator WaitToPlayAgain(PianoHand hand)
    {
        yield return new WaitForSeconds(minimumWaitToPlay);
        readyToPlay = true;
        hand.readyToPlay = true;
    }

    //private void Update()
    //{
    //    if (soundStarted && !source.isPlaying)
    //    {
    //        soundStarted = false;
    //        physicalKey.layer = LayerMask.NameToLayer("PianoKeys");
    //    }
    //}

    //public void CheckForSound(PianoHand hand)
    //{
    //    if (hand.readyToPlay)
    //    {
    //        hand.readyToPlay = false;
    //        if (!source.isPlaying)
    //            source.Play();
    //        else
    //        {
    //            source.Stop();
    //            source.Play();
    //        }
    //        soundStarted = true;
    //    }
    //}

    public void EnableCorrectColor()
    {
        GetComponentInChildren<Renderer>().material.SetColor("_Color2", GameManager.Instance.colors.correctColor);
        GetComponentInChildren<Renderer>().material.SetColor("_Color1", ApplyOffsetColor(GameManager.Instance.colors.correctColor));
        //GetComponent<Renderer>().material.color = GameManager.Instance.colors.correctColor;
        //GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.Instance.colors.correctColor);
    }

    public IEnumerator FlashIncorrectColor()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            GetComponentInChildren<Renderer>().material.SetColor("_Color2", GameManager.Instance.colors.incorrectColor);
            GetComponentInChildren<Renderer>().material.SetColor("_Color1", ApplyOffsetColor(GameManager.Instance.colors.incorrectColor));
            //GetComponent<Renderer>().material.color = GameManager.Instance.colors.incorrectColor;
            //GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.Instance.colors.incorrectColor);
            yield return new WaitForSeconds(incorrectLightFlashDuration);
            RestoreKeyColor();
            isFlashing = false;
        }
    }

    public void RestoreKeyColor()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_Intensity", startingIntensity);
        GetComponentInChildren<Renderer>().material.SetColor("_Color2", originalColor);
        GetComponentInChildren<Renderer>().material.SetColor("_Color1", ApplyOffsetColor(originalColor));
    }

    IEnumerator ChangeLightIntensityOnTouch()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_Intensity", touchLightIntensity);
        yield return new WaitForSeconds(lightIntensityDuration);
        GetComponentInChildren<Renderer>().material.SetFloat("_Intensity", startingIntensity);
    }

    void ShowNote()
    {
        noteDisplay.gameObject.SetActive(true);

        string noteString = note.ToString();
        if (noteString.Length > 1)
        {
            noteString = noteString[0] + "#";
        }
        noteDisplay.text = noteString;
    }
}
