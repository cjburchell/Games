﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace DungeonEscape.Scenes.Map.Components.UI
{
    public class CommandMenu: GameWindow, IUpdatable
    {
        private readonly IGame gameState;
        private readonly StatusWindow statusWindow;

        public CommandMenu(UICanvas canvas, IGame gameState, StatusWindow statusWindow) : base(canvas,"Command", new Point(30,30),100,150)
        {
            this.gameState = gameState;
            this.statusWindow = statusWindow;
        }
        
        private VirtualButton showMenuInput;
        private TextButton statusButton;
        private TextButton spellButton;
        private TextButton itemButton;
        private TextButton equipButton;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var commandTable = this.Window.AddElement(new Table());

            this.statusButton =new TextButton("Status", Skin);
            this.statusButton.GetLabel().SetFontScale(FontScale);
            this.statusButton.OnClicked += _ =>
            {
                // TODO: Show individual status window
                this.statusWindow.Show(() =>
                {
                    this.HideWindow();
                });
            };
            this.spellButton =new TextButton("Spell",Skin);
            this.spellButton.GetLabel().SetFontScale(FontScale);
            this.spellButton.OnClicked += _ =>
            {
                // TODO: Show Spell window
                this.HideWindow();
            };
            
            this.itemButton = new TextButton("Item", Skin);
            this.itemButton.GetLabel().SetFontScale(FontScale);
            this.itemButton.OnClicked += _ =>
            {
                // TODO: Show Items Window
                this.HideWindow();
            };
            this.equipButton = new TextButton("Equip", Skin);
            this.equipButton.GetLabel().SetFontScale(FontScale);
            this.equipButton.OnClicked += _ =>
            {
                // TODO: Show Equip Window
                this.HideWindow();
            };

            // layout
            commandTable.SetFillParent(true);
            commandTable.Top().PadLeft(10).PadTop(10).PadRight(10);
            commandTable.Add(this.statusButton).Height(30).Width(80);
            commandTable.Row().SetPadTop(0);
            commandTable.Add(this.spellButton).Height(30).Width(80);
            commandTable.Row().SetPadTop(0);
            commandTable.Add(this.itemButton).Height(30).Width(80);
            commandTable.Row().SetPadTop(0);
            commandTable.Add(this.equipButton).Height(30).Width(80);

            this.showMenuInput = new VirtualButton();
            this.showMenuInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.RightControl));
            this.showMenuInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.E));
            this.showMenuInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.B));
        }

        protected override void HideWindow()
        {
            base.HideWindow();
            this.Window.GetStage().SetGamepadFocusElement(null);
            this.statusButton.GamepadDownElement = null;
            this.spellButton.GamepadDownElement = null;
            this.itemButton.GamepadDownElement = null;
            this.equipButton.GamepadDownElement = null;
            this.statusButton.GamepadUpElement = null;
            this.spellButton.GamepadUpElement = null;
            this.itemButton.GamepadUpElement = null;
            this.equipButton.GamepadUpElement = null;
            this.gameState.IsPaused = false;
        }

        protected override void ShowWindow()
        {
            base.ShowWindow();
            this.Window.GetStage().SetGamepadFocusElement(this.statusButton);
            this.statusButton.GamepadDownElement = this.spellButton;
            this.spellButton.GamepadDownElement = this.itemButton;
            this.itemButton.GamepadDownElement = this.equipButton;
            this.equipButton.GamepadDownElement = this.statusButton;
            this.statusButton.GamepadUpElement = this.equipButton;
            this.spellButton.GamepadUpElement = this.statusButton;
            this.itemButton.GamepadUpElement = this.spellButton;
            this.equipButton.GamepadUpElement = this.itemButton;
            this.gameState.IsPaused = true;
        }

        public override void OnRemovedFromEntity()
        {
            this.showMenuInput.Deregister();
        }

        public void Update()
        {
            if (!this.showMenuInput.IsPressed)
            {
                return;
            }

            if (!this.Window.IsVisible())
            {
                this.ShowWindow();
            }
            else
            {
                this.HideWindow();
            }
        }
    }
}