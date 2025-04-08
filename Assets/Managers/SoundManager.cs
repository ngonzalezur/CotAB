
using UnityEngine;

namespace CotA.Sound
{
    public class SoundManager : MonoBehaviour
    {
        
        public void PlayMusic ()
        {
          
        }
        
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
        }


        public void StopMusic ()
        {

        }

        public void PlayHeroDamage () //Quite el float Health por no saber como usarlo
        {
            AkUnitySoundEngine.PostEvent("Play_Damage_Druid", gameObject);
        }

      
    }
}