using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;


public class NPC_Base : MonoBehaviour {

        [SerializeField] NPC_Type npcType;
        [SerializeField] List<Ability> abilityList;

        public NPC_Type GetNPCType() {
                return npcType;
        }
        public List<Ability> GetAbilityList() {
                return abilityList;
        }
}
public enum NPC_Type {
        None, Vampire, Demon, Werewolf
}
public enum Ability {
        None, AirJump, AirFloat, Dash
}