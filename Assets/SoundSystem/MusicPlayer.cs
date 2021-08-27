﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class MusicPlayer : MonoBehaviour
    {
        List<AudioSource> layerSources = new List<AudioSource>();
        List<float> sourceStartVolumes = new List<float>();
        MusicEvent _musicEvent = null;
        Coroutine fadeVolumeRoutine = null;

        private void Awake()
        {
            CreateLayerSources();
        }

        void CreateLayerSources()
        {
            for (int i = 0; i < MusicManager.MaxLayerCount; i++)
            {
                layerSources.Add(gameObject.AddComponent<AudioSource>());

                layerSources[i].playOnAwake = false;
                layerSources[i].loop = true;
            }
        }

        public void Play(MusicEvent musicEvent, float fadeTime)
        {
            Debug.Log("Play Music");

            _musicEvent = musicEvent;

            for (int i = 0; i < layerSources.Count 
                && ( i < musicEvent.MusicLayers.Length); i++)
            {
                if (musicEvent.MusicLayers[i] != null)
                {
                    layerSources[i].volume = 0;
                    layerSources[i].clip = musicEvent.MusicLayers[i];
                    layerSources[i].Play();
                }
            }

            FadeVolume(MusicManager.Instance.Volume, fadeTime);
        }

        public void FadeVolume(float targetVolume, float fadeTime)
        {
            targetVolume = Mathf.Clamp(targetVolume, 0, 1);
            if (fadeTime < 0) fadeTime = 0;

            if (fadeVolumeRoutine != null)
                StopCoroutine(fadeVolumeRoutine);

            if (_musicEvent.LayerType == LayerType.Additive)
            {
                StartCoroutine(LerpSourceAdditiveRoutine(targetVolume, fadeTime));
            }
            else if (_musicEvent.LayerType == LayerType.Single)
            {
                StartCoroutine(LerpSourceSingleRoutine());
            }
        }

        IEnumerator LerpSourceAdditiveRoutine(float targetVolume, float fadeTime)
        {
            SaveSourceStartVolumes();

            float startVolume;
            float newVolume;

            for (float elapsedTime = 0; elapsedTime <= fadeTime; elapsedTime += Time.deltaTime)
            {
                for (int i = 0; i < layerSources.Count; i++)
                {
                    // If active layer, fade to target
                    if (i <= MusicManager.Instance.ActiveLayerIndex)
                    {
                        startVolume = sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
                        layerSources[i].volume = newVolume;
                    }
                    // Fades to 0 from current volume if not
                    else
                    {
                        startVolume = sourceStartVolumes[i];
                        newVolume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeTime);
                        layerSources[i].volume = newVolume;
                    }
                }

                yield return null;
            }

            // If we get this far, set to target for accuracy - whole # instead of decimals
            for (int i = 0; i < layerSources.Count; i++)
            {
                if (i <= MusicManager.Instance.ActiveLayerIndex)
                {
                    layerSources[i].volume = targetVolume;
                }
                else
                {
                    layerSources[i].volume = 0;
                }
            }
        }

        private void SaveSourceStartVolumes()
        {
            sourceStartVolumes.Clear();
            for (int i = 0; i < layerSources.Count; i++)
            {
                sourceStartVolumes.Add(layerSources[i].volume);
            }
        }

        IEnumerator LerpSourceSingleRoutine()
        {
            yield return null;
        }
    }
}
