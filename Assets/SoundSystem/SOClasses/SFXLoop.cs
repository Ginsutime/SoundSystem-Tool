using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    [CreateAssetMenu(menuName = "SoundSystem/SFX Looped", fileName = "SFX_LP_")]
    public class SFXLoop : SFXEvent
    {
        [Header("Test")]
        public int NumCycles = 0;
        public bool isLoopedInfinitely;

        public void Play(Vector3 position)
        {
            SetVariationValues();

            if (Clip == null)
            {
                Debug.LogWarning("SFXLoop.Play: No Clips Specified");
                return;
            }

            SFXManager.Instance.PlayLoop(this, position);
        }

        public void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}
