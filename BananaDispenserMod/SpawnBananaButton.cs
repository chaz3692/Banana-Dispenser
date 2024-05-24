using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using BepInEx;
using DevHoldableEngine;
using GorillaLocomotion.Swimming;
using UnityEngine;
using Utilla;

namespace BananaDispenser
{
    public class SpawnBananaButton : MonoBehaviour
    {
        public static GameObject banana;
        public static GameObject rBanana;
        public static GameObject pipe;
        public static Transform spawnPos1;
        public static Transform spawnPos2;
        public static float force = 1000;
        public static bool canPress = true;
        public bool isPipe;

        public void SpawnBanan()
        {
            spawnPos1 = Plugin.spawnPos1;
            spawnPos2 = Plugin.spawnPos2;

            if(!isPipe)
            {
                banana = Instantiate(Plugin.bundle.LoadAsset<GameObject>("Banana"));

                //Set Pos To Tree
                banana.transform.position = spawnPos1.transform.position;

                //Add Force
                if (force < 850)
                {
                    banana.GetComponent<Rigidbody>().AddForce(spawnPos2.transform.up * force);
                }
                if (force > 850)
                {
                    banana.GetComponent<Rigidbody>().AddForce(spawnPos1.transform.up * force);
                }

                //Add Physics
                var impact = banana.AddComponent<ImpactEffects>();
                impact.isPipe = false;
                var holdable = banana.AddComponent<HoldableEngine>();
                holdable.Rigidbody = banana.GetComponent<Rigidbody>();
                holdable.PickUp = true;
                banana.AddComponent<RigidbodyWaterInteraction>();

                //Make Collisions Work
                banana.layer = 8;
            }
            else
            {
                pipe = Instantiate(Plugin.bundle.LoadAsset<GameObject>("Pipe"));

                //Set Pos To Tree
                pipe.transform.position = spawnPos1.transform.position;

                //Add Force
                if (force < 850)
                {
                    pipe.GetComponent<Rigidbody>().AddForce(spawnPos2.transform.up * force);
                }
                if (force > 850)
                {
                    pipe.GetComponent<Rigidbody>().AddForce(spawnPos1.transform.up * force);
                }

                //Add Physics
                var impact = pipe.AddComponent<ImpactEffects>();
                impact.isPipe = true;
                var holdable = pipe.AddComponent<HoldableEngine>();
                holdable.Rigidbody = pipe.GetComponent<Rigidbody>();
                holdable.PickUp = true;
                pipe.AddComponent<RigidbodyWaterInteraction>();

                //Make Collisions Work
                pipe.layer = 8;
            }
        }

        public void SpawnRainbow()
        {
            spawnPos1 = Plugin.spawnPos1;

            rBanana = Instantiate(Plugin.bundle.LoadAsset<GameObject>("RainbowBanana"));

            //Set Pos To Tree
            rBanana.transform.position = spawnPos1.transform.position;

            //Add Force
            force = UnityEngine.Random.Range(700, 1000);

            rBanana.GetComponent<Rigidbody>().AddForce(spawnPos1.transform.up * force);

            //Add Physics
            var impact = rBanana.AddComponent<ImpactEffects>();
            impact.isPipe = false;
            var holdable = rBanana.AddComponent<HoldableEngine>();
            holdable.Rigidbody = rBanana.GetComponent<Rigidbody>();
            holdable.PickUp = true;
            rBanana.AddComponent<RigidbodyWaterInteraction>();

            //Make Collisions Work
            rBanana.layer = 8;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "buttonPresserR" && canPress)
            {
                //Get Random Force
                force = UnityEngine.Random.Range(700, 1000);

                //Make So You Cant Press
                canPress = false;

                if (isPipe)
                {
                    SpawnBanan();
                }
                else
                {
                    //See If Lower Than 720 Then Spawn Rainbow Banana
                    if (force < 705)
                    {
                        SpawnRainbow();
                    }
                    else
                    {
                        SpawnBanan();
                    }
                }

                //Play Click Sound And Vibrate Controller
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
            }

            if (collider.name == "buttonPresserL" && canPress)
            {
                //Get Random Force
                force = UnityEngine.Random.Range(700, 1000);

                //Make So You Cant Press
                canPress = false;

                if (isPipe)
                {
                    SpawnBanan();
                }
                else
                {
                    //See If Lower Than 720 Then Spawn Rainbow Banana
                    if (force < 705)
                    {
                        SpawnRainbow();
                    }
                    else
                    {
                        SpawnBanan();
                    }
                }

                //Play Click Sound And Vibrate Controller
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, true, 0.1f);
                GorillaTagger.Instance.StartVibration(true, .01f, 0.001f);
            }
        }
        private void OnTriggerExit(Collider collider)
        {
            if (collider.name == "buttonPresserR" && !canPress)
            {
                canPress = true;
            }

            if (collider.name == "buttonPresserL" && !canPress)
            {
                canPress = true;
            }
        }
    }
}
