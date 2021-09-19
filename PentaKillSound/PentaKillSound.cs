using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using Rocket.API.Collections;
using UnityEngine;
using Rocket.Unturned.Chat;

namespace PentaKillSound
{
    public class PentaKillSound : RocketPlugin
    {
        public static UnturnedPlayer asesino;
        public static PentaKillSound Instance;
        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
        }


        private void UnturnedPlayerEvents_OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {

            asesino = UnturnedPlayer.FromCSteamID(murderer);
            Data objAsesino = asesino.GetComponent<Data>();
            Data objPlayer = player.GetComponent<Data>();

            if(cause.ToString() == "GUN" || cause.ToString() == "MELEE" || cause.ToString() == "PUNCH")
            {
                objPlayer.kill = 0; objAsesino.kill++;
                if (objAsesino.kill == 5)
                {
                    EffectManager.sendEffect(12780, 50, asesino.Position);
                    ChatManager.serverSendMessage(Translate("pentakill1", asesino.CharacterName).Replace('(', '<').Replace(')', '>'), Color.white, null, null, EChatMode.SAY, null, true);
                }
                else if(objAsesino.kill == 10)
                {
                    EffectManager.sendEffect(12781, 90, asesino.Position);
                    objAsesino.kill = 0;
                    ChatManager.serverSendMessage(Translate("pentakill2", asesino.CharacterName).Replace('(', '<').Replace(')', '>'), Color.white, null, null, EChatMode.SAY, null, true);
                }
            }
        }


        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {

                    {"pentakill1", "(size=15)(color=#F70D1A)「PENTAKILL」(/color)(color=red){0}(/color) (color=#5EFB6E)ACABA DE MATAR A 5. QUE ALGUIEN LO PARE(/color)"},
                    {"pentakill2", "(size=15)(color=#F70D1A)「PENTAKILL」(/color)(color=red){0}(/color) (color=#5EFB6E)VUELVE A MATAR A 5. ES UN DIOS(/color)"},
                };
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerDeath -= UnturnedPlayerEvents_OnPlayerDeath;
        }

    }
}
