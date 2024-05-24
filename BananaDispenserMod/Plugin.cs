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
        public static Plugin instance;
        public static GameObject bananaDispenserParent;
        public static GameObject pipeDispenserParent;
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

            //Load Banana Dispenser
            LoadBananaDispenser();


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

        public void LoadBananaDispenser()
        {
            //Destroy Pipe Dispenser
            Destroy(pipeDispenserParent);

            //Load Parent
            bananaDispenserParent = Instantiate(bundle.LoadAsset<GameObject>("BananaDispenserParent"));

            //Load Spawn Button
            spawnButton = bananaDispenserParent.transform.GetChild(0);

            //Load Delete Button
            deleteButton = bananaDispenserParent.transform.GetChild(3);

            //Load Switch Button
            var switchButton = bananaDispenserParent.transform.GetChild(4);

            //Load Spawn Positions
            spawnPos1 = bananaDispenserParent.transform.GetChild(1);
            spawnPos2 = bananaDispenserParent.transform.GetChild(2);

            //Set Parent Location
            bananaDispenserParent.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);

            //Add Button Stuff
            spawnButton.AddComponent<SpawnBananaButton>();
            deleteButton.AddComponent<DestroyBananaButton>();
            var switchScript = switchButton.AddComponent<SwtichDispensers>();
            switchScript.isBanana = true;
            spawnButton.gameObject.layer = 8;
            deleteButton.gameObject.layer = 8;
            switchButton.gameObject.layer = 8;

            //Set Objects To DontDestroyOnLoad
            DontDestroyOnLoad(spawnButton.gameObject);
            DontDestroyOnLoad(deleteButton.gameObject);
            DontDestroyOnLoad(bananaDispenserParent.gameObject);
            DontDestroyOnLoad(spawnPos1.gameObject);
            DontDestroyOnLoad(spawnPos2.gameObject);
        }

        public void LoadPipeDispenser()
        {
            //Destroy Banana Dispenser
            Destroy(bananaDispenserParent);

            //Load Parent
            pipeDispenserParent = Instantiate(bundle.LoadAsset<GameObject>("PipeDispenserParent"));

            //Load Spawn Button
            spawnButton = pipeDispenserParent.transform.GetChild(0);

            //Load Delete Button
            deleteButton = pipeDispenserParent.transform.GetChild(3);

            //Load Switch Button
            var switchButton = pipeDispenserParent.transform.GetChild(4);

            //Load Spawn Positions
            spawnPos1 = pipeDispenserParent.transform.GetChild(1);
            spawnPos2 = pipeDispenserParent.transform.GetChild(2);

            //Set Parent Location
            pipeDispenserParent.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);

            //Add Button Stuff
            var pipeSpawn = spawnButton.AddComponent<SpawnBananaButton>();
            pipeSpawn.isPipe = true;
            deleteButton.AddComponent<DestroyBananaButton>();
            var switchScript = switchButton.AddComponent<SwtichDispensers>();
            switchScript.isBanana = false;
            spawnButton.gameObject.layer = 8;
            deleteButton.gameObject.layer = 8;
            switchButton.gameObject.layer = 8;

            //Set Objects To DontDestroyOnLoad
            DontDestroyOnLoad(spawnButton.gameObject);
            DontDestroyOnLoad(deleteButton.gameObject);
            DontDestroyOnLoad(pipeDispenserParent.gameObject);
            DontDestroyOnLoad(spawnPos1.gameObject);
            DontDestroyOnLoad(spawnPos2.gameObject);
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

        public void Awake()
        {
            instance = this;
        }
    }
}
