using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using System.Collections;
using Il2Cpp;

namespace BleedingRevamped
{
	public class BleedingRevamped : MelonMod
	{
		public override void OnInitializeMelon()
		{           
            Debug.Log($"[{Info.Name}] version {Info.Version} loaded!");
        }

        public static void UpdateWounds(BaseAi ba, float mins)
        {

            if (ba.IsBleedingOut())
            {
                float BleedOutMintues = ba.m_DeathAfterBleeingOutMinutes;


                ba.m_CurrentHP -= ba.m_MaxHP * mins / BleedOutMintues;

                BleedOutMintues *= Mathf.Exp(mins / (60f));
                if (BleedOutMintues > 500000f)
                {
                    ba.m_BleedingOut = false;
                    BleedOutMintues = 0;
                    ba.m_ElapsedBleedingOutMinutes = 0;
                }

                ba.m_DeathAfterBleeingOutMinutes = BleedOutMintues;

                Log(BleedOutMintues.ToString() + "     " + ba.m_CurrentHP.ToString());

                if (ba.m_CurrentHP <= 0)
                {
                    ba.m_CurrentHP = 0;
                    BaseAi.DamageSide damageSide = ba.m_LastDamageSide;

                    int Part = ba.m_LastDamageBodyPart;



                    ba.SetDamageImpactParameter(damageSide, Part, BaseAi.SetupDamageParamsOptions.None);  //a copy the game code
                    ba.SetAiMode(AiMode.Dead);

                    // __instance.m_ElapsedBleedingOutMinutes = float.PositiveInfinity;
                    //but tricking the base code did not work                 
                }
            }

            if (ba.m_Wounded)
            {
                ba.m_CurrentHP += ba.m_CurrentHP / ba.m_MaxHP * mins / 60f;   //up to one HP 1 per hour
                if (ba.m_CurrentHP > ba.m_MaxHP)
                {
                    ba.m_CurrentHP = ba.m_MaxHP;
                    ba.m_Wounded = false;
                }
            }
        }

        internal static void Log(string message)
        {
            Debug.Log(message);
        }
    }
}