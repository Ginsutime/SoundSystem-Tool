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
        [SerializeField] AudioClip[] musicLayers;
        [SerializeField] LayerType layerType = LayerType.Additive;
        [SerializeField] AudioMixerGroup mixer;

        public AudioClip[] MusicLayers => musicLayers;
        public LayerType LayerType => layerType;
        public AudioMixerGroup Mixer => mixer;

        public void Play(float fadeTime)
        {
            MusicManager.Instance.PlayMusic(this, fadeTime);
        }
    }
}
