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

namespace BananaDispenser.Resources
{
    internal class DestroyBananaButton : MonoBehaviour
    {
        public static HoldableEngine[] bananaList;
        public static bool canPress = true;
        public void DestroyBananas()
        {
            bananaList = GameObject.FindObjectsOfType<HoldableEngine>();
            foreach(HoldableEngine erm in bananaList)
            {
                Destroy(erm.gameObject);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "buttonPresser" && canPress)
            {
                canPress = false;
                DestroyBananas();
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
