using System.Collections;
using UnityEngine;

public class PauseSpawnerButton : MonoBehaviour
{
    public float buttonDelay = .5f;
    private bool isReadyToPush = true;

    public void OnTriggerEnter(Collider other)
    {
        if (isReadyToPush)
        {
            StartCoroutine(WaitToPushAgain());
            var hand = other.gameObject.GetComponent<PianoHand>();
            if(hand)
                hand.HapticVibration();
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("isSquish");

            Debug.Log("pause");

            if (GameManager.Instance.isGameActive)
                GameManager.Instance.StopSpawning();
        }
    }

    IEnumerator WaitToPushAgain()
    {
        isReadyToPush = false;
        yield return new WaitForSeconds(buttonDelay);
        isReadyToPush = true;
    }
}
