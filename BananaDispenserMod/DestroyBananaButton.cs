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
        public void DestroyBananas()
        {
            bananaList = GameObject.FindObjectsOfType<HoldableEngine>();
            foreach(HoldableEngine erm in bananaList)
            {
                Destroy(erm.gameObject);
            }
        }

        public static int framePressCooldown = 0;
        private void OnTriggerEnter(Collider collider)
        {
            if (Time.frameCount >= framePressCooldown + 20 && collider.name == "buttonPresser")
            {
                DestroyBananas();
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
                framePressCooldown = Time.frameCount;
            }
        }
    }
}
