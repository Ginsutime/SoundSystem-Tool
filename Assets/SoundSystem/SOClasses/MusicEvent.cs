using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public enum LayerType
    {
        Additive, //Combine separate tracks together in sequence
        Single //One at a time, blend or fade to next
    }

    [CreateAssetMenu(menuName = "SoundSystem/MusicEvent", fileName = "MUS_")]
    public class MusicEvent : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField] AudioClip[] musicLayers = null;

        [Tooltip("Additive = Layers Added Together, " +
            "Single = Layers Play Independently")]
        [Space(15)]
        [SerializeField] LayerType layerType = LayerType.Additive;
        [Space(15)]
        [SerializeField] AudioMixerGroup mixer;

        public AudioClip[] MusicLayers => musicLayers;
        public LayerType LayerType => layerType;
        public AudioMixerGroup Mixer => mixer;

        public void Play(float fadeTime)
        {
            if (musicLayers == null)
            {
                Debug.LogWarning("MusicEvent.Play(): No music clip specified!");
                return;
            }

            MusicManager.Instance.PlayMusic(this, fadeTime);
        }
    }
}
