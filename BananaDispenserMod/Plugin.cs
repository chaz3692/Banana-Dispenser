using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using BananaDispenser.Resources;
using BepInEx;
using DevHoldableEngine;
using GorillaLocomotion.Swimming;
using UnityEngine;
using Utilla;

namespace BananaDispenser
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle bundle;
        public static GameObject parent;
        public static Transform spawnButton;
        public static Transform deleteButton;
        public static GameObject rightPresser;
        public static GameObject leftPresser;
        public static Transform spawnPos1;
        public static Transform spawnPos2;

        void Start() => Utilla.Events.GameInitialized += Init;

        private void Init(object sender, EventArgs e)
        {
            //Load AssetBundle
            bundle = LoadAssetBundle("BananaDispenser.AssetBundles.bananadispenser");

            //Load Parent
            parent = Instantiate(bundle.LoadAsset<GameObject>("BananaDispenserParent"));

            //Load Spawn Button
            spawnButton = parent.transform.GetChild(0);

            //Load Delete Button
            deleteButton = parent.transform.GetChild(3);

            //Load Spawn Positions
            spawnPos1 = parent.transform.GetChild(1);
            spawnPos2 = parent.transform.GetChild(2);

            //Set Parent Location
            parent.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);

            //Add Button Stuff
            spawnButton.AddComponent<SpawnBananaButton>();
            deleteButton.AddComponent<DestroyBananaButton>();
            spawnButton.gameObject.layer = 8;
            deleteButton.gameObject.layer = 8;

            //Set Objects To DontDestroyOnLoad
            DontDestroyOnLoad(spawnButton.gameObject);
            DontDestroyOnLoad(deleteButton.gameObject);
            DontDestroyOnLoad(parent.gameObject);
            DontDestroyOnLoad(spawnPos1.gameObject);
            DontDestroyOnLoad(spawnPos2.gameObject);


            //Add Button Pressing Ability
              //Right
              if (rightPresser == null)
              {
                rightPresser = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rightPresser.name = "buttonPresserR";
                rightPresser.GetComponent<SphereCollider>().isTrigger = true;
                rightPresser.layer = 11;
            }
            
              //Left
              if (leftPresser == null)
              {
                leftPresser = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leftPresser.name = "buttonPresserL";
                leftPresser.GetComponent<SphereCollider>().isTrigger = true;
                leftPresser.layer = 11;
            }
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        private void Update()
        {
            //Move Button Pressers
              //Right
              rightPresser.transform.parent = GorillaLocomotion.Player.Instance.rightControllerTransform;
              rightPresser.transform.localPosition = new Vector3(0f, -0.1f, 0f) * GorillaLocomotion.Player.Instance.scale;
              rightPresser.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * GorillaLocomotion.Player.Instance.scale;

              //Left
              leftPresser.transform.parent = GorillaLocomotion.Player.Instance.leftControllerTransform;
              leftPresser.transform.localPosition = new Vector3(0f, -0.1f, 0f) * GorillaLocomotion.Player.Instance.scale;
              leftPresser.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f) * GorillaLocomotion.Player.Instance.scale;
        }
    }
}
