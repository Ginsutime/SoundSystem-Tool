using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class MusicManager : MonoBehaviour
    {
        int activeLayerIndex = 0;
        public int ActiveLayerIndex => activeLayerIndex;

        // Around 3:30 of Video 12 explains basics of how to do object pooling instead if need be
        // This only works for 2 Music Players, no more
        MusicPlayer musicPlayer1;
        MusicPlayer musicPlayer2;

        bool isMusicPlayer1Playing = false;

        public MusicPlayer ActivePlayer => (isMusicPlayer1Playing) ? musicPlayer1 : musicPlayer2;
        public MusicPlayer InactivePlayer => (isMusicPlayer1Playing) ? musicPlayer2 : musicPlayer1;

        MusicEvent activeMusicEvent;

        public const int MaxLayerCount = 3;

        float volume = 1;
        public float Volume
        {
            get => volume;
            private set
            {
                value = Mathf.Clamp(value, 0, 1);
                volume = value;
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
                        GameObject singletonGO = new GameObject("MusicManager_singleton");
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
            }

            SetupMusicPlayers();
        }

        // 8:50 of Video 12 explains more object pooling stuff
        void SetupMusicPlayers()
        {
            musicPlayer1 = gameObject.AddComponent<MusicPlayer>();
            musicPlayer2 = gameObject.AddComponent<MusicPlayer>();
        }

        public void PlayMusic(MusicEvent musicEvent, float fadeTime)
        {
            // If empty, returns
            if (musicEvent == null) return;
            // If passing something already playing, returns
            if (musicEvent == activeMusicEvent) return;

            if (activeMusicEvent != null)
                ActivePlayer.Stop(fadeTime);

            activeMusicEvent = musicEvent;

            // 12:40ish of Video 12 object pooling mentioned
            // Toggles the state of the boolean
            isMusicPlayer1Playing = !isMusicPlayer1Playing;

            ActivePlayer.Play(musicEvent, fadeTime);
        }

        public void StopMusic(float fadeTime)
        {
            if (activeMusicEvent == null)
                return;

            activeMusicEvent = null;
            ActivePlayer.Stop(fadeTime);
        }

        public void IncreaseLayerIndex(float fadeTime)
        {
            // clamp index properly
            int newLayerIndex = activeLayerIndex + 1;
            newLayerIndex = Mathf.Clamp(newLayerIndex, 0, MaxLayerCount - 1);

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

            if (newLayerIndex == activeLayerIndex)
                return;

            activeLayerIndex = newLayerIndex;
            SetVolume(Volume, fadeTime);
        }
    }
}
