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
        public bool isOpen = false; //door starts closed

        public float degreesPerSecond = 1f; 
        public float totalRotation = 0;
        public float rotationAmount = 15f;
        private MissileFire weaponManager;
        
        

        





        [KSPAction("Open")] //for action group use
        public void Open(KSPActionParam param)
        {
           OpenBay();
        }

       

        [KSPAction("Close")] //for action group use
        public void Close(KSPActionParam param)
        {
            CloseBay();
        }

        [KSPEvent(guiActive = true, guiName = "Open Bay", guiActiveEditor = true)]
        public void OpenBay()
        {
              
            
            isOpen = true;

            //Rotation nonsense from before
            //Quaternion rotation = Quaternion.AngleAxis(120, part.transform.forward);
            //part.transform.localRotation = Quaternion.Slerp(part.transform.rotation, rotation, .05f);


            // transform.localEulerAngles = new Vector3(10, 0, 0);

            // Debug.Log(open);

            Toggle("OpenBay", false);
             Toggle("CloseBay", true);
             
            
        }

        [KSPEvent(guiActive = false, guiName = "Close Bay", guiActiveEditor = false)]
        public void CloseBay()
        {

            isOpen = false;

            //Rotation nonsense from before
            //Quaternion rotation = Quaternion.AngleAxis(-120, part.transform.forward);
           // part.transform.localRotation = Quaternion.Slerp(part.transform.rotation, rotation, .05f);

            Toggle("OpenBay", true);
            Toggle("CloseBay", false);

            
        }

       

        public override void OnStart(StartState state)
        {
          

            if (state != StartState.Editor)
            {
                foreach (var wm in vessel.FindPartModulesImplementing<MissileFire>()) //make sure a weapons manager exists on the vessel after launch
                {
                    weaponManager = wm;

                }

            }
        }

        private void Toggle(string eventName, bool state) //toggle gui buttons
        {
            Events[eventName].active = state;
            Events[eventName].externalToEVAOnly = state;
            Events[eventName].guiActiveEditor = state;
            Events[eventName].guiActive = state;
        }








        public override void OnUpdate()


          {
           
               


            if (HighLogic.LoadedSceneIsFlight) //if it's not in the editor


              

            {
                if (weaponManager.radar && weaponManager.radar.locked && weaponManager.radar.lockedTarget.exists) //make sure it has a radar, it's locked, and it has a valid target
                {
                    if (!isOpen)
                    {
                        Debug.Log("Radar lock, opening bays.");
                        OpenBay();
                    }
                }
                else if (!weaponManager.radar.locked ) //if the radar isn't locked close the bay door
                                   
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

