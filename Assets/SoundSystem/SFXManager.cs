using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] int startingPoolSize = 5;
        SoundPool soundPool;

        private static SFXManager instance;
        public static SFXManager Instance
        {
            get
            {
                // Lazy Instantiation
                if (instance == null)
                {
                    instance = FindObjectOfType<SFXManager>();

                    if (instance == null)
                    {
                        GameObject singletonGO = new GameObject("SFXManager_singleton");
                        instance = singletonGO.AddComponent<SFXManager>();

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

            Initialize();
        }

        void Initialize()
        {
            soundPool = new SoundPool(this.transform, startingPoolSize);
        }
        
        public void PlayOneShot(SFXOneShot soundEvent, Vector3 soundPosition)
        {
            if (soundEvent.Clip == null)
            {
                Debug.LogWarning("SoundManager.PlayOneShot: No Clip Specified");
                return;
            }

            AudioSource newSource = soundPool.Get();

            newSource.clip = soundEvent.Clip;
            newSource.outputAudioMixerGroup = soundEvent.Mixer;
            newSource.priority = soundEvent.Priority;
            newSource.volume = soundEvent.Volume;
            newSource.pitch = soundEvent.Pitch;
            newSource.panStereo = soundEvent.StereoPan;
            newSource.spatialBlend = soundEvent.SpatialBlend;

            newSource.minDistance = soundEvent.AttenuationMin;
            newSource.maxDistance = soundEvent.AttenuationMax;

            newSource.transform.position = soundPosition;

            ActivatePooledSound(newSource);
        }

        public void PlayOneShot(AudioSource source)
        {
            if (source.clip == null)
            {
                Debug.LogWarning("SoundManager.PlayOneShot: no clip specified");
                return;
            }

            AudioSource newSource = soundPool.Get();

            newSource.clip = source.clip;
            newSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
            newSource.priority = source.priority;
            newSource.volume = source.volume;
            newSource.pitch = source.pitch;
            newSource.panStereo = source.panStereo;
            newSource.spatialBlend = source.spatialBlend;

            newSource.minDistance = source.minDistance;
            newSource.maxDistance = source.maxDistance;

            newSource.transform.position = source.transform.position;

            ActivatePooledSound(newSource);
        }

        private void ActivatePooledSound(AudioSource newSource)
        {
            newSource.gameObject.SetActive(true);
            newSource.Play();

            StartCoroutine(DisableAfterCompleteRoutine(newSource));
        }

        IEnumerator DisableAfterCompleteRoutine(AudioSource source)
        {
            source.loop = false;

            float clipDuration = source.clip.length;
            yield return new WaitForSeconds(clipDuration);

            source.Stop();
            soundPool.Return(source);
        }
    }
}
