using System;
using System.Collections.Generic;
using System.Collections;
using Rocket.API;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned;
using Steamworks;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using UnityEngine;
using SDG.Unturned;

namespace DudeTaser
{
    public class Taser : RocketPlugin<TaserConfig>
    {
        public static Taser instance;
        public static HashSet<CSteamID> TasedPlayers;
        Dictionary<ushort, Tased> TaseDict;

        protected override void Load()
        {
            instance = this;
            Logger.Log("#----------------------------------------#", ConsoleColor.Green);
            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded", ConsoleColor.Green);
            Logger.Log("Author: Dudewithoutname#3129 | Edited by: JonHosting.com", ConsoleColor.Green);
            Logger.Log("#----------------------------------------#", ConsoleColor.Green);

            TasedPlayers = new HashSet<CSteamID>();
            TaseDict = new Dictionary<ushort, Tased>();
            foreach(Tased TaseConf in Configuration.Instance.TasedL)
            {
                TaseDict[TaseConf.ID] = TaseConf;
            }

            UnturnedEvents.OnPlayerDamaged += OnPlayerDamage;
            
        }

        protected override void Unload()
        {
            instance = null;
            TasedPlayers = null;

            UnturnedEvents.OnPlayerDamaged -= OnPlayerDamage;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnPlayerDamage(UnturnedPlayer victim, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer attacker, ref Vector3 direction, ref float damage, ref float times, ref bool canDamage)
        {
            if ( attacker != null && cause == EDeathCause.GUN && attacker.Player.equipment.IsEquipAnimationFinished && !TasedPlayers.Contains(attacker.CSteamID) && !victim.HasPermission(Configuration.Instance.NoTasePerm))
            {
                ushort iID = attacker.Player.equipment.itemID;
                float MovementMultiplier = -1;
                float TasedTime = 0;
                EPlayerStance Stance = EPlayerStance.CLIMB;
                EPlayerGesture Gesture = EPlayerGesture.NONE;
                TaseDict.TryGetValue(iID, out Tased Gg);

                if (attacker.Player.equipment.itemID == Configuration.Instance.TaserId) {
                    MovementMultiplier = Configuration.Instance.MovementMultiplier;
                    TasedTime = Configuration.Instance.TasedTime;
                    Stance = Configuration.Instance.TasedStance;
                    Gesture = Configuration.Instance.TasedGesture;
                }
                else
                if (Gg != null) {
                    MovementMultiplier = Gg.MovementMultiplier;
                    TasedTime = Gg.TaserTime;
                    Stance = Gg.Stance;
                    Gesture = Gg.Gesture;
                }
                if (MovementMultiplier != -1)
                {
                    victim.Player.equipment.dequip();
                    victim.Player.movement.sendPluginSpeedMultiplier(MovementMultiplier);
                    victim.Player.movement.sendPluginJumpMultiplier(MovementMultiplier);
                    if (Stance != EPlayerStance.CLIMB)
                    {
                        victim.Player.stance.stance = Stance;
                        victim.Player.stance.checkStance(Stance);
                    }
                    if (Gesture != EPlayerGesture.NONE && victim.Player.animator.gesture != EPlayerGesture.ARREST_START) victim.Player.animator.sendGesture(Gesture, true);
                    TasedPlayers.Add(victim.CSteamID);
                    StartCoroutine(RemoveFromTased(victim, TasedTime));
                    StartCoroutine(CheckTased(victim, Stance, Gesture));
                    damage = 0;
                    canDamage = false;
                }
                return;
            }
        }
        private IEnumerator CheckTased(UnturnedPlayer victim, EPlayerStance Stance, EPlayerGesture Gesture)
        {
            while (TasedPlayers.Contains(victim.CSteamID))
            {
                victim.Player.equipment.dequip();
                if (Stance != EPlayerStance.CLIMB)
                {
                    victim.Player.stance.stance = Stance;
                    victim.Player.stance.checkStance(Stance);
                }
                if(Gesture != EPlayerGesture.NONE && victim.Player.animator.gesture != EPlayerGesture.ARREST_START) victim.Player.animator.sendGesture(Gesture, true);

                yield return new WaitForSeconds(0.3f);
            }
            yield break;
        }

        private IEnumerator RemoveFromTased(UnturnedPlayer victim, float TasedTime = 0)
        {
            yield return new WaitForSeconds(TasedTime);

            victim.Player.movement.sendPluginSpeedMultiplier(1f);
            victim.Player.movement.sendPluginJumpMultiplier(1f);
            TasedPlayers.Remove(victim.CSteamID);
            yield break;
        }
    }
}
