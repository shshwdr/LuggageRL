using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class FMODEvents : Singleton<FMODEvents>
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music_biome_1 { get; private set; }
    [field: SerializeField] public EventReference music_luggage_adventure { get; private set; }
    [field: SerializeField] public EventReference music_boss { get; private set; }
    [field: SerializeField] public EventReference music_airport { get; private set; }
    [field: SerializeField] public EventReference music_death_stinger { get; private set; }
    [field: SerializeField] public EventReference music_airport_battle { get; private set; }
    [field: SerializeField] public List<EventReference> musicPlaylist { get; private set; }

    [field: Header("SFX")]

    [field: SerializeField] public EventReference sfx_effect_heal { get; private set; }
    [field: SerializeField] public EventReference sfx_enemy_ability_steal { get; private set; }
    [field: SerializeField] public EventReference sfx_enemy_action_shield { get; private set; }
    [field: SerializeField] public EventReference sfx_enemy_death { get; private set; }
    [field:SerializeField] public EventReference sfx_enemy_heavy_attack { get; private set; }
    [field:SerializeField] public EventReference sfx_enemy_light_attack { get; private set; }
    [field:SerializeField] public EventReference sfx_bow_and_arrow { get; private set; }
    [field: SerializeField] public EventReference sfx_item_break { get; private set; }
    [field: SerializeField] public EventReference sfx_item_cash_register { get; private set; }
    [field:SerializeField] public EventReference sfx_item_drag_pickup { get; private set; }
    [field:SerializeField] public EventReference sfx_item_drag_putdown { get; private set; }
    [field:SerializeField] public EventReference sfx_item_impact { get; private set; }
    [field:SerializeField] public EventReference sfx_item_piggy_bank_break { get; private set; }
    [field:SerializeField] public EventReference sfx_item_potion_drink { get; private set; }
    [field:SerializeField] public EventReference sfx_item_rocket { get; private set; }
    [field:SerializeField] public EventReference sfx_item_slide { get; private set; }
    [field:SerializeField] public EventReference sfx_item_surge { get; private set; }
    [field:SerializeField] public EventReference sfx_item_take_new { get; private set; }
    [field:SerializeField] public EventReference sfx_luggage_attack_impact { get; private set; }
    [field: SerializeField] public EventReference sfx_luggage_attack_whoosh { get; private set; }
    [field: SerializeField] public EventReference sfx_luggage_death_blow { get; private set; }
    [field: SerializeField] public EventReference sfx_luggage_up_down_swirl { get; private set; }
    [field:SerializeField] public EventReference sfx_luggage_walk { get; private set; }
    [field:SerializeField] public EventReference sfx_luggage_walk_full_loop { get; private set; }
    [field:SerializeField] public EventReference sfx_ui_hover { get; private set; }
    [field:SerializeField] public EventReference sfx_ui_select { get; private set; }
    [field:SerializeField] public EventReference sfx_random_boing { get; private set; }
    [field:SerializeField] public EventReference sfx_bomb_explode { get; private set; }

    private int nextPlaylistIndex = 0; 
    internal EventReference getNextMusicReference()
    {
        if (nextPlaylistIndex >= musicPlaylist.Count - 1)
        {
            nextPlaylistIndex = 0;
        }
        EventReference reference = musicPlaylist[nextPlaylistIndex];
        nextPlaylistIndex++;
        return musicPlaylist[nextPlaylistIndex];
    }
}




