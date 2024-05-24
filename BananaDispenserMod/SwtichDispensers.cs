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
    internal class SwtichDispensers : MonoBehaviour
    {
        public bool isBanana;
        public static bool canPress = true;

        public void SwitchDespenser()
        {
            if(!isBanana)
            {
                Plugin.instance.LoadBananaDispenser();
            }
            else
            {
                Plugin.instance.LoadPipeDispenser();
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "buttonPresserR" && canPress)
            {
                canPress = false;
                SwitchDespenser();
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.1f);
                GorillaTagger.Instance.StartVibration(false, .01f, 0.001f);
            }

            if (collider.name == "buttonPresserL" && canPress)
            {
                canPress = false;
                SwitchDespenser();
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
