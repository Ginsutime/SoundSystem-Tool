using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioSystem;

public class AudioTester : MonoBehaviour
{
    [SerializeField] MusicEvent songA;
    [SerializeField] MusicEvent songB;
    [SerializeField] MusicEvent songC;

    [SerializeField] int decreaseMusicLayerTransitionTime = 0;
    [SerializeField] int increaseMusicLayerTransitionTime = 0;
    [SerializeField] int setMusicLayerNumber = 0;
    [SerializeField] int setMusicLayerTransitionTime = 0;

    [Header("Sound Portion: R for OneShot, T for Loop")]
    [SerializeField] SFXOneShot soundA;
    [SerializeField] SFXLoop soundB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            songA.Play();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            songB.Play();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            songC.Play();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MusicManager.Instance.StopMusic();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MusicManager.Instance.SetLayerIndex(setMusicLayerNumber, setMusicLayerTransitionTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MusicManager.Instance.DecreaseLayerIndex(decreaseMusicLayerTransitionTime);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MusicManager.Instance.IncreaseLayerIndex(increaseMusicLayerTransitionTime);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            soundA.PlayOneShot(transform.position);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            soundB.Play(transform.position);
        }
    }
}
