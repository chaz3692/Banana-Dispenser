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
        public bool isPipe;
        private const float effectVolume = 1f;
        private AudioSource audioSource;

        void Awake()
        {
            //Load imapct audio clips and add them to the List
            pipeSound = Plugin.bundle.LoadAsset<AudioClip>("MetalPipe");

            //Load Splat sound
            splatSound = Plugin.bundle.LoadAsset<AudioClip>("splat");

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
                    audioSource.PlayOneShot(pipeSound, 0.1f);
                }
                else
                {
                    //Changes pitch of splat to add veriety to the noise
                    audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
                    
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
