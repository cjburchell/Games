﻿using DungeonEscape.Scenes.Common.Components.UI;
using DungeonEscape.State;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace DungeonEscape.Scenes.Map.Components.UI
{
    public class InventoryWindow : SelectWindow<ItemInstance>
    {
        public InventoryWindow(UISystem ui) : base(ui, "Inventory", new Point(150, 30))
        {
        }
        
        protected override Button CreateButton(ItemInstance item)
        {
            var table = new Table();
            var image = new Image(item.Image).SetAlignment(Align.Left);
            var equipSymbol = item.IsEquipped?"(E)":string.Empty;
            var equip = new Label(equipSymbol, Skin).SetAlignment(Align.Left);
            var itemName = new Label(item.Name, Skin).SetAlignment(Align.Left);
            table.Add(image).Width(32);
            table.Add(equip).Width(32);
            table.Add(itemName).Width(100);

            var button = new Button(Skin);
            button.Add(table);
            return button;
        }
    }
}