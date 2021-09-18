using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add functionality to looping that allows it to 
// either be infinite or only run for x amount of cycles.
// Add anything needed to SFXLoop, not here
namespace SoundSystem
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] int startingOneShotPoolSize = 5;
        [SerializeField] int startingLoopPoolSize = 5;
        SoundPool soundPools;

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
                        GameObject singletonGO = new GameObject("SFXManager");
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
            GameObject sfxOneShotOrganizerGO = new GameObject("SFXManager_OneShots");
            sfxOneShotOrganizerGO.transform.SetParent(this.transform);
            GameObject sfxLoopOrganizerGO = new GameObject("SFXManager_Loops");
            sfxLoopOrganizerGO.transform.SetParent(this.transform);

            soundPools = new SoundPool(this.transform, sfxOneShotOrganizerGO.transform, sfxLoopOrganizerGO.transform,
                startingOneShotPoolSize, startingLoopPoolSize);
        }
        
        public void PlayOneShot(SFXOneShot soundEvent, Vector3 soundPosition)
        {
            if (soundEvent.Clip == null)
            {
                Debug.LogWarning("SoundManager.PlayOneShot: No Clip Specified");
                return;
            }

            AudioSource newSource = soundPools.Get();

            newSource.clip = soundEvent.Clip;
            newSource.outputAudioMixerGroup = soundEvent.Mixer;
            newSource.priority = soundEvent.Priority;
            newSource.volume = soundEvent.Volume;
            newSource.pitch = soundEvent.Pitch;
            newSource.panStereo = soundEvent.StereoPan;
            newSource.spatialBlend = soundEvent.SpatialBlend;

            // 3D Options - May not Need
            newSource.minDistance = soundEvent.AttenuationMin;
            newSource.maxDistance = soundEvent.AttenuationMax;

            newSource.transform.position = soundPosition;

            ActivatePooledSound(newSource);
        }

        // Find out if this is even needed
        public void PlayOneShot(AudioSource source)
        {
            if (source.clip == null)
            {
                Debug.LogWarning("SoundManager.PlayOneShot: no clip specified");
                return;
            }

            AudioSource newSource = soundPools.Get();

            newSource.clip = source.clip;
            newSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
            newSource.priority = source.priority;
            newSource.volume = source.volume;
            newSource.pitch = source.pitch;
            newSource.panStereo = source.panStereo;
            newSource.spatialBlend = source.spatialBlend;

            // 3D Options - May not Need
            newSource.minDistance = source.minDistance;
            newSource.maxDistance = source.maxDistance;

            newSource.transform.position = source.transform.position;

            ActivatePooledSound(newSource);
        }

        public void PlayLoop(SFXLoop soundEvent, Vector3 soundPosition)
        {
            if (soundEvent.Clip == null)
            {
                Debug.LogWarning("SoundManager.PlayLoop: No Clip Specified");
                return;
            }

            AudioSource newSource = soundPools.GetLoop();

            newSource.clip = soundEvent.Clip;
            newSource.outputAudioMixerGroup = soundEvent.Mixer;
            newSource.priority = soundEvent.Priority;
            newSource.volume = soundEvent.Volume;
            newSource.pitch = soundEvent.Pitch;
            newSource.panStereo = soundEvent.StereoPan;
            newSource.spatialBlend = soundEvent.SpatialBlend;

            // 3D Options - May not Need
            newSource.minDistance = soundEvent.AttenuationMin;
            newSource.maxDistance = soundEvent.AttenuationMax;

            newSource.transform.position = soundPosition;

            ActivateLoopedPooledSound(newSource); // Cancels loop currently
        }

        // Find out if this is even needed
        public void PlayLoop(AudioSource source)
        {
            if (source.clip == null)
            {
                Debug.LogWarning("SoundManager.PlayLoop: no clip specified");
                return;
            }

            AudioSource newSource = soundPools.GetLoop();

            // May need more options for loop like time before it or something else, check last semester code
            newSource.loop = true;
            newSource.clip = source.clip;
            newSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
            newSource.priority = source.priority;
            newSource.volume = source.volume;
            newSource.pitch = source.pitch;
            newSource.panStereo = source.panStereo;
            newSource.spatialBlend = source.spatialBlend;

            // 3D Options - May Not Need
            newSource.minDistance = source.minDistance;
            newSource.maxDistance = source.maxDistance;

            newSource.transform.position = source.transform.position;

            ActivateLoopedPooledSound(newSource); // Cancels loop currently - find fix
        }

        private void ActivateLoopedPooledSound(AudioSource newSource)
        {
            newSource.gameObject.SetActive(true);
            newSource.Play();

            StartCoroutine(DisableAfterCompleteLoopedRoutine(newSource));
        }

        IEnumerator DisableAfterCompleteLoopedRoutine(AudioSource source)
        {
            source.loop = false;

            float clipDuration = source.clip.length;
            yield return new WaitForSeconds(clipDuration);

            source.Stop();

            soundPools.ReturnLoop(source);
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

            soundPools.Return(source);
        }
    }
}
