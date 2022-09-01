﻿// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Redpoint.DungeonEscape.State
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class ItemInstance
    {
        public ItemInstance(Item item)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Item = item;
        }

        // ReSharper disable once UnusedMember.Global
        public ItemInstance()
        {
        }

        public Item Item { get; set; }
        public string Id { get; set; }
        public string EquippedTo { get; set; }
        public bool IsEquipped { get; set; }

        [JsonIgnore]
        public Nez.Textures.Sprite Image => this.Item.Image;
        
        [JsonIgnore]
        public Rarity Rarity => this.Item.Rarity;

        [JsonIgnore]
        public int MinLevel => this.Item.MinLevel;
        
        [JsonIgnore]
        public ItemType Type => this.Item.Type;
        
        [JsonIgnore]
        public List<Slot> Slots => this.Item.Slots;
        
        [JsonIgnore]
        public string Name => this.Item.Name;

        [JsonIgnore] public string NameWithStats => this.Item.NameWithStats;
        
        [JsonIgnore]
        public int Gold => this.Item.Cost;
        
        [JsonIgnore]
        public int Agility => this.Item.GetAttribute(StatType.Agility);

        [JsonIgnore]
        public int Attack => this.Item.GetAttribute(StatType.Attack);
        
        [JsonIgnore]
        public int Defence => this.Item.GetAttribute(StatType.Defence);
        
        [JsonIgnore]
        public int MagicDefence => this.Item.GetAttribute(StatType.MagicDefence);

        [JsonIgnore]
        public int Health => this.Item.GetAttribute(StatType.Health);
        
        [JsonIgnore]
        public int Magic => this.Item.GetAttribute(StatType.Magic);

        [JsonIgnore] public IReadOnlyCollection<Class> Classes => this.Item.Classes;
        
        [JsonIgnore]
        public bool IsEquippable => Item.EquippableItems.Contains(this.Type);
        
        public void UnEquip(IEnumerable<Hero> heroes)
        {
            if (!this.IsEquipped || string.IsNullOrEmpty(this.EquippedTo))
            {
                return;
            }

            var equippedHero = heroes.FirstOrDefault(hero => hero.Id == this.EquippedTo);
            equippedHero?.UnEquip(this);
        }
    }
}