﻿using DungeonEscape.Scenes.Map.Components.UI;
using DungeonEscape.State;
using Nez.AI.Pathfinding;
using Nez.Tiled;

namespace DungeonEscape.Scenes.Map.Components.Objects
{
    public class KeyStore : Sprite
    {
        private readonly QuestionWindow questionWindow;
        private readonly TalkWindow talkWindow;
        private Item key;

        public KeyStore(TmxObject tmxObject, TmxMap map, IGame gameState, AstarGridGraph graph, QuestionWindow questionWindow, TalkWindow talkWindow) : base(tmxObject, map, gameState, graph)
        {
            this.questionWindow = questionWindow;
            this.talkWindow = talkWindow;
            var cost = tmxObject.Properties.ContainsKey("Cost") ? int.Parse(tmxObject.Properties["Cost"]) : 250;
            var level= tmxObject.Properties.ContainsKey("Level") ? int.Parse(tmxObject.Properties["Level"]) : 0;
            this.key = new Item("Content/images/items/key.png", "Key", ItemType.Key, cost, level);
        }
        
        public override bool OnAction(Player player)
        {
            this.gameState.IsPaused = true;
            this.questionWindow.Show($"Would you like to buy a key\nFor {this.key.Gold} gold?", accepted =>
            {
                if (accepted)
                {
                    if (player.Gold >= this.key.Gold)
                    {
                        player.Gold -= this.key.Gold;
                        player.Items.Add(this.key);
                        this.talkWindow.ShowText("Thank you come again!", () => { this.gameState.IsPaused = false;});
                    }
                    else
                    {
                        this.talkWindow.ShowText($"You do not have {this.key.Gold} gold", () => { this.gameState.IsPaused = false;});
                    }
                }
                else
                {
                    this.gameState.IsPaused = false;
                }
            });
            
            return true;
        }
    }
}    