using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
    public class SoundPool
    {
        Transform _parent;

        private Queue<AudioSource> pool = new Queue<AudioSource>();
        private Queue<AudioSource> pool2 = new Queue<AudioSource>();

        public SoundPool(Transform parent, int startSize, int startSizeLoop)
        {
            _parent = parent;

            CreateInitialPool(startSize, startSizeLoop);
        }

        void CreateInitialPool(int startingOneShotPoolSize, int startingLoopPoolSize)
        {
            for (int i = 0; i < startingOneShotPoolSize; i++)
            {
                CreatePoolObject();
            }

            for (int i = 0; i < startingLoopPoolSize; i++)
            {
                CreateLoopPoolObject();
            }
        }

        public AudioSource Get()
        {
            if (pool.Count == 0)
            {
                CreatePoolObject();
            }

            AudioSource objectFromPool = pool.Dequeue();
            objectFromPool.gameObject.SetActive(true);

            return objectFromPool;
        }

        public void Return(AudioSource objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            pool.Enqueue(objectToReturn);
        }

        private void CreatePoolObject()
        {
            GameObject newGameObject = new GameObject("SoundSource");
            AudioSource newSource = newGameObject.AddComponent<AudioSource>();

            newGameObject.transform.SetParent(_parent);
            newGameObject.gameObject.SetActive(false);
            pool.Enqueue(newSource);
        }

        public AudioSource GetLoop()
        {
            if (pool2.Count == 0)
            {
                CreateLoopPoolObject();
            }

            AudioSource objectFromPool = pool2.Dequeue();
            objectFromPool.gameObject.SetActive(true);

            return objectFromPool;
        }

        public void ReturnLoop(AudioSource objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            pool2.Enqueue(objectToReturn);
        }

        private void CreateLoopPoolObject()
        {
            GameObject newGameObject = new GameObject("SoundSourceLoop");
            AudioSource newSource = newGameObject.AddComponent<AudioSource>();

            newGameObject.transform.SetParent(_parent);
            newGameObject.gameObject.SetActive(false);
            pool2.Enqueue(newSource);
        }
    }
}
