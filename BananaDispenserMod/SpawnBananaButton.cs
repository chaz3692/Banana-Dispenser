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
        public static GameObject car;
        public static Transform spawnPos1;
        public static Transform spawnPos2;
        public static float force = 1000;
        public static bool canPress = true;

        public void SpawnBanan()
        {
            spawnPos1 = Plugin.spawnPos1;
            spawnPos2 = Plugin.spawnPos2;

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
            var holdable = banana.AddComponent<HoldableEngine>();
            holdable.Rigidbody = banana.GetComponent<Rigidbody>();
            holdable.PickUp = true;
            banana.AddComponent<RigidbodyWaterInteraction>();

            //Make Collisions Work
            banana.layer = 8;
        }

        public void SpawnCar()
        {
            spawnPos1 = Plugin.spawnPos1;
            spawnPos2 = Plugin.spawnPos2;

            car = Instantiate(Plugin.bundle.LoadAsset<GameObject>("Cat"));

            //Set Pos To Tree
            car.transform.position = spawnPos1.transform.position;

            //Add Force
            force = UnityEngine.Random.Range(700, 1000);

            car.GetComponent<Rigidbody>().AddForce(spawnPos1.transform.up * force);

            //Add Physics
            var holdable = car.AddComponent<HoldableEngine>();
            holdable.Rigidbody = car.GetComponent<Rigidbody>();
            holdable.PickUp = true;
            car.AddComponent<RigidbodyWaterInteraction>();

            //Make Collisions Work
            car.layer = 8;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "buttonPresser" && canPress)
            {
                //Get Random Force
                force = UnityEngine.Random.Range(700, 1000);

                //Make So You Cant Press
                canPress = false;

                //See If Lower Than 720 Then Spawn Car
                if(force < 720)
                {
                    SpawnCar();
                }
                else
                {
                    SpawnBanan();
                }

                //Play Click Sound And Vibrate Controller
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
            }
        }
        private void OnTriggerExit(Collider collider)
        {
            if (collider.name == "buttonPresser" && !canPress)
            {
                canPress = true;
            }
        }
    }
}
