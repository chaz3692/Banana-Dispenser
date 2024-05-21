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
    [BepInPlugin(GorillaTagModTemplateProject.PluginInfo.GUID, GorillaTagModTemplateProject.PluginInfo.Name, GorillaTagModTemplateProject.PluginInfo.Version)]
    public class SpawnBananaButton : BaseUnityPlugin
    {
        public static GameObject banana;
        public static Transform spawnPos1;
        public static Transform spawnPos2;
        public static float force = 1000;

        public void SpawnBanan()
        {
            spawnPos1 = Plugin.spawnPos1;
            spawnPos2 = Plugin.spawnPos2;

            banana = Instantiate(Plugin.bundle.LoadAsset<GameObject>("Banana"));

            //Set Pos To Tree
            banana.transform.position = spawnPos1.transform.position;

            //Add Force
            force = UnityEngine.Random.Range(700, 1000);
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

        public static int framePressCooldown = 0;
        private void OnTriggerEnter(Collider collider)
        {
            if (Time.frameCount >= framePressCooldown + 20 && collider.name == "buttonPresser")
            {
                SpawnBanan();
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
                framePressCooldown = Time.frameCount;
            }
        }
    }
}
