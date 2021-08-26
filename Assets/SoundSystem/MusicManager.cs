using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class MusicManager : MonoBehaviour
    {
        MusicPlayer musicPlayer;
        public const int MaxLayerCount = 3;

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

        void SetupMusicPlayers()
        {
            musicPlayer = gameObject.AddComponent<MusicPlayer>();
        }

        public void PlayMusic(MusicEvent musicEvent, float fadeTime)
        {
            musicPlayer.Play(musicEvent, fadeTime);
        }
    }
}
