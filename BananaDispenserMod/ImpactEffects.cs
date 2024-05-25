using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BananaDispenser
{
    internal class ImpactEffects : MonoBehaviour
    {
        private const float velocityForEffects = 15f;
        private AudioClip splatSound;
        private AudioClip pipeSound;
        private AudioClip corruptedSound;
        public bool isPipe;
        public bool isCorrupted;
        private const float effectVolume = 1f;
        private AudioSource audioSource;

        void Awake()
        {
            //Load Pipe sound
            pipeSound = Plugin.bundle.LoadAsset<AudioClip>("MetalPipe");

            //Load Splat sound
            splatSound = Plugin.bundle.LoadAsset<AudioClip>("splat");

            //Load Corrupted sound
            corruptedSound = Plugin.bundle.LoadAsset<AudioClip>("WindowsError");

            //Get audio source from gameObject
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        void OnCollisionEnter(Collision collision)
        {
            //Check if Velocity of Banana/Cat is higher then set ammound
            if (collision.relativeVelocity.magnitude > velocityForEffects)
            {
                //Checks if its the cat that has this script
                if(isPipe)
                {
                    //Plays the set audio clip
                    audioSource.PlayOneShot(pipeSound, 1f);
                }
                else
                {
                    if(isCorrupted)
                    {
                        //Changes pitch of splat to add veriety to the noise
                        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);

                        //Plays splat sound
                        audioSource.PlayOneShot(corruptedSound, 3);

                        //Spawns particles and sets pos to current banana pos
                        GameObject corruptedParticle = Instantiate(Plugin.bundle.LoadAsset<GameObject>("CorruptedParticle"));
                        corruptedParticle.transform.position = gameObject.transform.position;
                    }
                    else
                    {
                        //Changes pitch of splat to add veriety to the noise
                        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);

                        //Plays splat sound
                        audioSource.PlayOneShot(splatSound, effectVolume);

                        //Spawns particles and sets pos to current banana pos
                        GameObject particle = Instantiate(Plugin.bundle.LoadAsset<GameObject>("SplatParticle"));
                        particle.transform.position = gameObject.transform.position;
                    }
                }
            }
        }
    }
}
