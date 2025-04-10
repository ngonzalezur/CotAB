
using UnityEngine;

namespace CotA.Sound
{
    public class SoundManager : MonoBehaviour
    {
        /// ---------------------------- LEER ANTES DE LLAMAR FUNCIONES -------------------------------------
        // M A U R I: Cambie la función "PlayHeroDamage" por la función "PlayDruidDamage" 
        // Entonces tienes que cambiar la función que llamas cuando le hacen daño a la druida por la nueva que puse
        //  TQM, piquito en las nalgas




       // public void PlayMusic ()   
        //{  
        //}

        //public void StopMusic ()
        //{
        //}
        
   //      -------------------        MÚSICA      -------------------------    
        public void ChangeSoundtrackToMidLifeMode () //Activar cuando el heroe este a la mitad de la vida
        {
          /// Combat 01 Callar
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Violin_lead", gameObject); //Calla violin
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Lento", gameObject); //Calla Contrabajo lento
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Rapido", gameObject); //Calla Contrabajo rapido
          /// Combat 02 Sonar
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Violin_lead02", gameObject); //Suena violin 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Lento02", gameObject); //Suena Contrabajo lento 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Rapido02", gameObject); //Suena Contrabajo rapido 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Pa02", gameObject); //Suena Contrabajo Paneado 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Drums02", gameObject); //Suena Drums 02
        }


//       --------------       DAÑOS A PERSONAJES      ----------------------------
        public void PlayDruidDamage () //Le hacen daño a la --- DRUIDA ---
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Druid", gameObject);
        }

        public void PlayRobotDamage () //Le hacen daño al --- ROBOT ---
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Robot", gameObject);
        }

//        ------------------   ATAQUES Y HABILIDADES   -----------------------------

//                     ------  DRUIDA   -----
        public void PlayFirewallDruid () //Druida hace el muro de fuego
        {
            AkUnitySoundEngine.PostEvent("Play_Firewall_Druid", gameObject);
        }
        public void PlayGridDruid () //Druida cambia las casillas de enemigo a amigo
        {
            AkUnitySoundEngine.PostEvent("Play_Grid_Druid", gameObject);
        }
        public void PlayMeleeDruid () //Druida hace ataque Dash y Melee
        {
            AkUnitySoundEngine.PostEvent("Play_Melee_Druid", gameObject);
        }
        public void PlayPoisonDruid () //Druida hace ataque de area de veneno
        {
            AkUnitySoundEngine.PostEvent("Play_Posion_Druid", gameObject);
        }

//                     ------  ROBOT   -----
        public void PlayBroteRobot () //Robot invoca cualquiera de las dos flores
        {
            AkUnitySoundEngine.PostEvent("Play_Brote_Robot", gameObject);
        }
        public void PlayParryRobot () //Robot bloquea el ataque de enmemigo
        {
            AkUnitySoundEngine.PostEvent("Play_Parry_Robot", gameObject);
        }
        public void PlaySmiteRobot () //Robot hace el ataque de area
        {
            AkUnitySoundEngine.PostEvent("Play_Parry_Robot", gameObject);
        }
      
    }
}