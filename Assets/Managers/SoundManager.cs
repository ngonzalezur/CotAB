
using UnityEngine;

namespace CotA.Sound
{
    public class SoundManager : MonoBehaviour
    {
        // ---------------- LEER ANTES ---------------
        // Este script es el encargado de controlar la música y los SFX del juego
        // La música de los niveles emnpieza a sonar desde un componente en el SoundManager, aqui se controla el cambio de música
        // Los SFX si se les da play desde aca
        // La música del Boss solo suena en el ultimo nivel, de resto es Combat 01 y Combat 02
        


        // ----------------------- MÚSICA ----------------------------
        public void ChangeSoundtrackToMidLifeMode ()
        {
          /// Combat 01 Callar
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Violin_lead", gameObject); //Calla violin
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Lento", gameObject); //Calla Contrabajo lento
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Rapido", gameObject); //Calla Contrabajo rapido
          /// Combat 02 Sonar - Suena cuando el heroe este al 60% de su vida
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Violin_lead02", gameObject); //Suena violin 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Lento02", gameObject); //Suena Contrabajo lento 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Rapido02", gameObject); //Suena Contrabajo rapido 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Contra_Bajo_Pa02", gameObject); //Suena Contrabajo Paneado 02
          AkUnitySoundEngine.PostEvent("Set_Voice_Volume_Drums02", gameObject); //Suena Drums 02

          /// Boss Music --- Cambio de Boss Music 01 a Boss Music 02
          AkUnitySoundEngine.PostEvent("PlayBossMusic02", gameObject); //Suena Boss Music 02
        }


        // ----------------------- DAÑO A HEROES ---------------------------
        public void PlayDruidDamage () //Le hacen daño a la Druida
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Druid", gameObject);
        }

        public void PlayRobotDamage () //Le hacen daño a la Robot
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Robot", gameObject);
        }

        // ------------------- Ataques y Habilidades Heroes ---------------
        //                   ---- DRUIDA ----
        public void PlayFirewallDruid () //Druida hace el ataque muro de fuego
        {
            AkUnitySoundEngine.PostEvent("Play_Firewall_Druid", gameObject);
        }
        public void PlayGridDruid () //Druida cambias las casillas de enemigas a amigas
        {
            AkUnitySoundEngine.PostEvent("Play_Grid_Druid", gameObject);
        }
        public void PlayMeleeDruid () //Druida hace dash y ataque melee
        {
            AkUnitySoundEngine.PostEvent("Play_Melee_Druid", gameObject);
        }
        public void PlayPoisonDruid () //Druida lanza ataque de area veneno
        {
            AkUnitySoundEngine.PostEvent("Play_Posion_Druid", gameObject);  //se que esta escrito mal posion, asi quedo el event en wwise despues lo corrijo
        }

        //                   ---- ROBOT ----
        public void PlayBroteRobot () //Robot invoca cualquiera de las 2 flores
        {
            AkUnitySoundEngine.PostEvent("Play_Brote_Robot", gameObject);
        }
        public void PlayParryRobot () //Robot bloquea ataque de enemigos
        {
            AkUnitySoundEngine.PostEvent("Play_Parry_Robot", gameObject);
        }
        public void PlaySmiteRobot () //Robot hace ataque de area
        {
            AkUnitySoundEngine.PostEvent("Play_Smite_Robot", gameObject);
        }
        

      
    }
}