using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class SoundSystemTester : MonoBehaviour
{
    [SerializeField] MusicEvent songA;
    [SerializeField] MusicEvent songB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            songA.Play(2.5f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            songB.Play(2.5f);
        }
    }
}
