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
        public bool isCat;
        private const float effectVolume = 1f;
        private List<AudioClip> audioClips = new List<AudioClip>();
        private AudioSource audioSource;
        static System.Random rnd = new System.Random();

        void Awake()
        {
            //Load imapct audio clips and add them to the List
            audioClips.Add(Plugin.bundle.LoadAsset<AudioClip>("body_medium_impact_hard2"));
            audioClips.Add(Plugin.bundle.LoadAsset<AudioClip>("body_medium_impact_hard3"));
            audioClips.Add(Plugin.bundle.LoadAsset<AudioClip>("body_medium_impact_hard5"));
            audioClips.Add(Plugin.bundle.LoadAsset<AudioClip>("body_medium_impact_hard6"));

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
                if(isCat)
                {
                    //Gets a random audio clip from the list
                    int r = rnd.Next(audioClips.Count);

                    //Plays the set audio clip
                    audioSource.PlayOneShot(audioClips[r], effectVolume);
                }
                else
                {
                    //Sets audio sources clip to splat sound
                    audioSource.clip = splatSound;
                    
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
