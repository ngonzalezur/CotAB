
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
        //  música de las cinematicas se activa desde un componente en el SoundManager, pero no se controla desde aca
        // dialogos si inician desde aca, con un signal emitter

        public void Awake()
        {
            // Reinicia toda la música al inicio de cada escena
            AkUnitySoundEngine.StopAll();

        }



// ----------------------------------------- M Ú S I C A -----------------------------------------------------------------------------
    // ------------------- CAMBIO DE MÚSICA -------------------
        public void ChangeSoundtrackToMidLifeMode()
        {
            /// Combat 01 to Combat 02
            AkUnitySoundEngine.PostEvent("ChangeCombatMusic", gameObject); //Suena Combat 02, todos los cambios suceden desde aqui


            /// Boss Music --- Cambio de Boss Music 01 a Boss Music 02
            AkUnitySoundEngine.PostEvent("PlayBossMusic02", gameObject); //Suena Boss Music 02
        }

// ---------------------------------------- H E R O E S -------------------------------------------------------------------------------------------
    // ----------------------- DAÑO A HEROES ---------------------------
        public void PlayDruidDamage() //Le hacen daño a la Druida
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Druid", gameObject);
        }

        public void PlayRobotDamage() //Le hacen daño a la Robot
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Robot", gameObject);
        }

    // ------------------- Ataques y Habilidades Heroes ---------------
        //                   ---- DRUIDA ----
        public void PlayFirewallDruid() //Druida hace el ataque muro de fuego
        {
            AkUnitySoundEngine.PostEvent("Play_Firewall_Druid", gameObject);
        }
        public void PlayGridDruid() //Druida cambias las casillas de enemigas a amigas
        {
            AkUnitySoundEngine.PostEvent("Play_Grid_Druid", gameObject);
        }
        public void PlayMeleeDruid() //Druida hace dash y ataque melee
        {
            AkUnitySoundEngine.PostEvent("Play_Melee_Druid", gameObject);
        }
        public void PlayPoisonDruid() //Druida lanza ataque de area veneno
        {
            AkUnitySoundEngine.PostEvent("Play_Posion_Druid", gameObject);  //se que esta escrito mal posion, asi quedo el event en wwise despues lo corrijo
        }
        public void PlayGanchoDruid() //Druida lanza ataque de gancho
        {
            AkUnitySoundEngine.PostEvent("Play_Gancho_Druid", gameObject);
        }
      

        //                   ---- ROBOT ----
        public void PlayBroteRobot() //Robot invoca cualquiera de las 2 flores
        {
            AkUnitySoundEngine.PostEvent("Play_Brote_Robot", gameObject);
        }
        public void PlayParryRobot() //Robot bloquea ataque de enemigos
        {
            AkUnitySoundEngine.PostEvent("Play_Parry_Robot", gameObject);
        }
        public void PlaySmiteRobot() //Robot hace ataque de area
        {
            AkUnitySoundEngine.PostEvent("Play_Smite_Robot", gameObject);
        }
        public void PlayFlorAttackRobot() //La flor del robot hace ataque
        {
            AkUnitySoundEngine.PostEvent("Play_Flor_attack_Robot", gameObject);
        }
  

// ------------------------------------------------------- E N E M I G O S ------------------------------------------------------------------------------
        // ------------------- Ataques y Habilidades Enemigos ---------------

        //                   ---- ENEMIGOS ----
        public void PlayAreaDistanceEnemy() //Area a distancia
        {
            AkUnitySoundEngine.PostEvent("Play_Area_distance_Enemy", gameObject);
        }
        public void PlayAreaFrontEnemy() //Area de frente
        {
            AkUnitySoundEngine.PostEvent("Play_Area_front_Enemy", gameObject);
        }
        public void PlayAtractEnemy() //Atraer Heroe, NO es gancho
        {
            AkUnitySoundEngine.PostEvent("Play_Atract_Enemy", gameObject);
        }
        public void PlayGanchoEnemy() //Gancho enemigo
        {
            AkUnitySoundEngine.PostEvent("Play_Gancho_Enemy", gameObject);
        }
      public void PlayEnemy() //Grid Teleport enemigo
        {
            AkUnitySoundEngine.PostEvent("Play_Grid_Teleport_Enemy", gameObject);
        }

        //                   ---- BOSS ----
        public void PlayBarridoColumnaBoss() //Boss hace ataque de barrido en columna
        {
            AkUnitySoundEngine.PostEvent("Play_Barrido_Columna_Boss", gameObject);
        }
        public void PlayBarridoFilaBoss() //Boss hace ataque de barrido en columna
        {
            AkUnitySoundEngine.PostEvent("Play_Barrido_Columna_Boss", gameObject);
        }
        public void PlayCasillaBoss() //Boss hace ataque en una casilla aleatoria
        {
            AkUnitySoundEngine.PostEvent("Play_Casilla_X_Boss", gameObject);
        }



    // ------------------- DAÑO A ENEMIGOS ---------------------------
        public void PlayDamageHongo() //Le hacen daño al hongo
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Hongo", gameObject);
        }
        public void PlayDamageMurcielago() //Le hacen daño al murcielago
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Murcielago", gameObject);
        }
        public void PlayDamageEsqueleto() //Le hacen daño al esqueleto
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Skeleton", gameObject);
        }
        public void PlayDamageBoss() //Le hacen daño al boss
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Boss", gameObject);
        }

        // ------------------- Dialogos Cinematicas ---------------
        //                  ---- DRUIDA ----
        public void PlayCine01Druid() //Cinematica 01 Druida vs esqueleto
        {
            AkUnitySoundEngine.PostEvent("Play_Cine01Druid", gameObject);
        }
        public void PlayCine02Druid() //Cinematica 02 Druida vs hongo
        {
            AkUnitySoundEngine.PostEvent("Play_Cine02Druid", gameObject);
        }
        public void PlayCine03Druid() //Cinematica 03 Druida vs murcielago
        {
            AkUnitySoundEngine.PostEvent("Play_Cine03Druid", gameObject);
        }
        public void PlayCine04Druid() //Cinematica 04 Druida vs boss
        {
            AkUnitySoundEngine.PostEvent("Play_Cine04Druid", gameObject);
        }
        //                  ---- CABALLERO ----
        public void PlayCine01Caballero() //Cinematica 01 Caballero vs esqueleto
        {
            AkUnitySoundEngine.PostEvent("Play_Cine01Caballero", gameObject);
        }
        public void PlayCine02Caballero() //Cinematica 02 Caballero vs hongo
        {
            AkUnitySoundEngine.PostEvent("Play_Cine02Caballero", gameObject);
        }
        public void PlayCine03Caballero() //Cinematica 03 Caballero vs murcielago
        {
            AkUnitySoundEngine.PostEvent("Play_Cine03Caballero", gameObject);
        }
        public void PlayCine04Caballero() //Cinematica 04 Caballero vs boss
        {
            AkUnitySoundEngine.PostEvent("Play_Cine04Caballero", gameObject);
        }




        

      
    }
}