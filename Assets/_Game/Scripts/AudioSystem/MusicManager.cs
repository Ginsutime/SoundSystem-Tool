﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class MusicManager : MonoBehaviour
    {
        int activeLayerIndex = 0;
        public int ActiveLayerIndex => activeLayerIndex;

        MusicPlayer musicPlayer1;
        MusicPlayer musicPlayer2;

        bool isMusicPlayer1Playing = false;

        public MusicPlayer ActivePlayer => (isMusicPlayer1Playing) ? musicPlayer1 : musicPlayer2;
        public MusicPlayer InactivePlayer => (isMusicPlayer1Playing) ? musicPlayer2 : musicPlayer1;

        MusicEvent activeMusicEvent;

        public const int MaxLayerCount = 3;

        public float Volume
        {
            get => activeMusicEvent.MasterVolume;
            private set
            {
                Mathf.Clamp(activeMusicEvent.MasterVolume, 0, 1);
            }
        }

        private static MusicManager instance;
        public static MusicManager Instance
        {
            get
            {
                // Lazy Instantiation
                if (instance == null)
                {
                    instance = FindObjectOfType<MusicManager>();

                    if (instance == null)
                    {
                        GameObject singletonGO = new GameObject("MusicManager");
                        instance = singletonGO.AddComponent<MusicManager>();

                        DontDestroyOnLoad(singletonGO);
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(instance.gameObject);
            }

            SetupMusicPlayers();
        }

        void SetupMusicPlayers()
        {
            musicPlayer1 = gameObject.AddComponent<MusicPlayer>();
            musicPlayer2 = gameObject.AddComponent<MusicPlayer>();
        }

        public void PlayMusic(MusicEvent musicEvent, float crossFadeTime)
        {
            // If empty, returns
            if (musicEvent == null) return;
            // If passing something already playing, returns
            if (musicEvent == activeMusicEvent) return;

            // If music player already playing, uses crossFadeTime
            if (activeMusicEvent != null)
            {
                ActivePlayer.Stop(crossFadeTime);

                activeMusicEvent = musicEvent;

                // Toggles the state of the boolean
                isMusicPlayer1Playing = !isMusicPlayer1Playing;

                ActivePlayer.Play(musicEvent, crossFadeTime);
            }
            // If first time music player is playing, uses initial fadein time
            else
            {
                activeMusicEvent = musicEvent;

                // Toggles the state of the boolean
                isMusicPlayer1Playing = !isMusicPlayer1Playing;
                ActivePlayer.Play(musicEvent, activeMusicEvent.InitialFadeInTime);
            }
        }

        public void StopMusic()
        {
            if (activeMusicEvent == null)
                return;

            ActivePlayer.Stop(activeMusicEvent.StopSongFadeOutTime);
            activeMusicEvent = null;
        }

        public void IncreaseLayerIndex(float fadeTime)
        {
            // clamp index properly
            int newLayerIndex = activeLayerIndex + 1;
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

            if (activeMusicEvent == null)
                return;

            // If next layer is empty, stop it from playing
            if (activeMusicEvent.MusicLayers[newLayerIndex] == null)
            {
                Debug.LogWarning("MusicEvent.IncreaseLayerIndex(): Reached empty music layer at layer "
                    + newLayerIndex + ". Can go no further.");
                return;
            }

            // Trying to increase it but already at max leads to this
            if (newLayerIndex == activeLayerIndex)
                return;

            activeLayerIndex = newLayerIndex;
            ActivePlayer.FadeVolume(Volume, fadeTime);
        }

        public void DecreaseLayerIndex(float fadeTime)
        {
            int newLayerIndex = activeLayerIndex - 1;
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

            if (activeMusicEvent == null)
                return;

            if (newLayerIndex == activeLayerIndex)
                return;

            activeLayerIndex = newLayerIndex;
            ActivePlayer.FadeVolume(Volume, fadeTime);
        }

        public void SetVolume(float newVolume, float fadeTime)
        {
            Volume = newVolume;
            ActivePlayer.FadeVolume(Volume, fadeTime);
        }

        public void SetLayerIndex(int newLayerIndex, float fadeTime)
        {
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

            if (activeMusicEvent == null)
                return;

            if (newLayerIndex == activeLayerIndex)
                return;

            activeLayerIndex = newLayerIndex;
            SetVolume(Volume, fadeTime);
        }
    }
}
