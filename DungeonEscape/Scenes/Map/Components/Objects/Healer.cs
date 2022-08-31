﻿namespace Redpoint.DungeonEscape.Scenes.Map.Components.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Common.Components.UI;
    using Nez.AI.Pathfinding;
    using Nez.Tiled;
    using State;

    public class Healer : Sprite
    {
        private readonly UiSystem _ui;
        private readonly int _cost;

        public Healer(TmxObject tmxObject, SpriteState state, TmxMap map, IGame gameState, AstarGridGraph graph, UiSystem ui) : base(tmxObject, state, map, gameState, graph)
        {
            this._ui = ui;
            this._cost = tmxObject.Properties.ContainsKey("Cost") ? int.Parse(tmxObject.Properties["Cost"]) : 25;
        }
        
        public override bool OnAction(Party party)
        {
            this.GameState.IsPaused = true;
            var goldWindow = new GoldWindow(party, this._ui.Canvas, this._ui.Sounds);
            goldWindow.ShowWindow();

            var healAllCost = party.AliveMembers.Where(member => member.Health != member.MaxHealth).Sum(_ => this._cost);
            var cureCost = this._cost * 2;
            var reviveCost = this._cost * 10;
            var magicCost = party.AliveMembers.Where(member => member.Magic != member.MaxMagic).Sum(_ => this._cost*2);
            
            var options = new List<string>();
            if (party.AliveMembers.Any(member => member.Health != member.MaxHealth))
            {
                options.Add($"Heal {this._cost}");
                if (party.AliveMembers.Count(member => member.Health != member.MaxHealth) != 1)
                {
                    options.Add($"Heal All {healAllCost}");
                }
            }
            
            if (party.AliveMembers.Any(member => member.Magic != member.MaxMagic))
            {
                options.Add($"Renew Magic {magicCost}");
            }
            
            if (party.AliveMembers.Any(member => member.Status.Count != 0))
            {
                options.Add($"Cure {cureCost}");
            }
            
            if (party.Members.Any(member => member.IsDead))
            {
                options.Add($"Revive {reviveCost}");
            }

            void Done()
            {
                goldWindow.CloseWindow();
                this.GameState.IsPaused = false;
            }
            
            if (!options.Any())
            {
                new TalkWindow(this._ui).Show("You do not require any of my services.", Done);
                return true;
            }
            
            new TalkWindow(this._ui).Show($"What services do you require?", () =>
            {
                new SelectWindow<string>(this._ui, null, new Point(20, 20)).Show( options, selection =>
                {
                    if (selection == null)
                    {
                        Done();
                        return;
                    }

                    switch (selection[..selection.LastIndexOf(' ')])
                    {
                        case "Heal":
                            if (party.Gold >= this._cost)
                            {
                                void Heal(Hero hero)
                                {
                                    if (hero == null)
                                    {
                                        Done();
                                        return;
                                    }
                                    
                                    party.Gold -= healAllCost;
                                    hero.Health = hero.MaxHealth;
                                    this.GameState.Sounds.PlaySoundEffect("spell", true);
                                    new TalkWindow(this._ui).Show($"{hero.Name} has been fully healed.\nThank you come again!", Done);
                                }
                            
                                if (party.AliveMembers.Count(member => member.Health != member.MaxHealth) == 1)
                                {
                                    Heal(party.AliveMembers.First(member => member.Health != member.MaxHealth));
                                }
                                else
                                {
                                    new SelectHeroWindow(this._ui).Show(party.AliveMembers.Where(member => member.Health != member.MaxHealth),
                                        Heal);
                                }
                            }
                            else
                            {
                                new TalkWindow(this._ui).Show($"You do not have {this._cost} gold", Done);
                            }
                            break;
                            
                        case "Renew Magic":
                            if (party.Gold >= magicCost)
                            {
                                party.Gold -= magicCost;
                                foreach (var partyMember in party.AliveMembers)
                                {
                                    partyMember.Magic = partyMember.MaxMagic;
                                }
                        
                                this.GameState.Sounds.PlaySoundEffect("spell", true);
                                new TalkWindow(this._ui).Show("All party members magic has been replenished.\nThank you come again!", Done);
                            }
                            else
                            {
                                new TalkWindow(this._ui).Show($"You do not have {magicCost} gold", Done);
                            }
                            break;
                        case "Revive":
                            if (party.Gold >= reviveCost)
                            {
                                void Revive(Hero hero)
                                {
                                    if (hero == null)
                                    {
                                        Done();
                                        return;
                                    }
                                    
                                    party.Gold -= healAllCost;
                                    hero.Health = 1;
                                    this.GameState.Sounds.PlaySoundEffect("spell", true);
                                    new TalkWindow(this._ui).Show($"{hero.Name} has been revived.\nThank you come again!", Done);
                                }
                            
                                if (party.Members.Count(member => member.IsDead) == 1)
                                {
                                    Revive(party.Members.First(member => member.IsDead));
                                }
                                else
                                {
                                    new SelectHeroWindow(this._ui).Show(party.Members.Where(member => member.IsDead),
                                        Revive);
                                }
                            }
                            else
                            {
                                new TalkWindow(this._ui).Show($"You do not have {reviveCost} gold", Done);
                            }
                            break;
                        case "Cure":
                            if (party.Gold >= cureCost)
                            {
                                void Cure(Hero target)
                                {
                                    var message = "";
                                    foreach (var effect in target.Status.ToList())
                                    {
                                        message += $"{target.Name} {effect.Name} has ended\n";
                                        target.RemoveEffect(effect);
                                    }

                                    this.GameState.Sounds.PlaySoundEffect("spell", true);
                                    new TalkWindow(this._ui).Show($"{message}Thank you come again!", Done);
                                }
                                
                                if (party.AliveMembers.Count(member => member.Status.Count != 0) == 1)
                                {
                                    Cure(party.AliveMembers.First(member => member.Status.Count != 0));
                                }
                                else
                                {
                                    new SelectHeroWindow(this._ui).Show(party.AliveMembers.Where(member => member.Status.Count != 0),
                                        Cure);
                                }
                            }
                            else
                            {
                                new TalkWindow(this._ui).Show($"You do not have {cureCost} gold", Done);
                            }
                            break;
                        case "Heal All":
                            if (party.Gold >= healAllCost)
                            {
                                party.Gold -= healAllCost;
                                foreach (var partyMember in party.AliveMembers)
                                {
                                    partyMember.Health = partyMember.MaxHealth;
                                }
                        
                                this.GameState.Sounds.PlaySoundEffect("spell", true);
                                new TalkWindow(this._ui).Show("All party members have been healed.\nThank you come again!", Done);
                            }
                            else
                            {
                                new TalkWindow(this._ui).Show($"You do not have {healAllCost} gold", Done);
                            }
                            break;
                    }
                    
                });
            });
            
            return true;
        }
    }
}