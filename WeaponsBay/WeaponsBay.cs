using UnityEngine;
using WeaponsBay.Extensions;
using BahaTurret;
using System.Collections.Generic;
using System;
using WeaponsBay.IRapi;

namespace WeaponsBay
{
    public class WeaponsBay : PartModule
    {



        [KSPField(isPersistant = true)]
        public bool isOpen = false;

        public float degreesPerSecond = 1f;
        public float totalRotation = 0;
        public float rotationAmount = 15f;
        private MissileFire weaponManager;
        
        

        





        [KSPAction("Open")]
        public void Open(KSPActionParam param)
        {
           OpenBay();
        }

       

        [KSPAction("Close")]
        public void Close(KSPActionParam param)
        {
            CloseBay();
        }

        [KSPEvent(guiActive = true, guiName = "Open Bay", guiActiveEditor = true)]
        public void OpenBay()
        {
              
            
            isOpen = true;


            Quaternion rotation = Quaternion.AngleAxis(120, part.transform.forward);
            part.transform.localRotation = Quaternion.Slerp(part.transform.rotation, rotation, .05f);


            // transform.localEulerAngles = new Vector3(10, 0, 0);

            // Debug.Log(open);

            Toggle("OpenBay", false);
             Toggle("CloseBay", true);
             
            
        }

        [KSPEvent(guiActive = false, guiName = "Close Bay", guiActiveEditor = false)]
        public void CloseBay()
        {

            isOpen = false;
            Quaternion rotation = Quaternion.AngleAxis(-120, part.transform.forward);
            part.transform.localRotation = Quaternion.Slerp(part.transform.rotation, rotation, .05f);

            Toggle("OpenBay", true);
            Toggle("CloseBay", false);

            
        }

       

        public override void OnStart(StartState state)
        {
            var rotation = part.transform.rotation;

            if (state != StartState.Editor)
            {
                foreach (var wm in vessel.FindPartModulesImplementing<MissileFire>())
                {
                    weaponManager = wm;

                }

            }
        }

        private void Toggle(string eventName, bool state)
        {
            Events[eventName].active = state;
            Events[eventName].externalToEVAOnly = state;
            Events[eventName].guiActiveEditor = state;
            Events[eventName].guiActive = state;
        }








        public override void OnUpdate()


          {
           
               


            if (HighLogic.LoadedSceneIsFlight)


              

            {
                if (weaponManager.radar && weaponManager.radar.locked && weaponManager.radar.lockedTarget.exists)
                {
                    if (!isOpen)
                    {
                        Debug.Log("Radar lock, opening bays.");
                        OpenBay();
                    }
                }
                else if (weaponManager.isArmed && !weaponManager.radar.locked )
                                   
                    {
                    if (isOpen){
                        Debug.Log("No lock, closing bay.");
                        CloseBay();
                    }
                        
                 }
                
            }
           
        }


       
}

}

