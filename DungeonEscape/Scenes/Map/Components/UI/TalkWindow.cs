﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace DungeonEscape.Scenes.Map.Components.UI
{
    public class TalkWindow : GameWindow, IUpdatable
    {
        private VirtualButton hideWindowInput;
        private string textToShow = "";
        private TextButton closeButton;
        private Label textLabel;
        private Action done;

        public TalkWindow(UICanvas canvas) : base(canvas, "", new Point(20, 20), 472,150)
        {
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var table = this.Window.AddElement(new Table());

            this.closeButton =new TextButton("Close", skin);
            this.closeButton.GetLabel().SetFontScale(FontScale);
            this.closeButton.OnClicked += _ =>
            {
                this.CloseWindow();
            };
            
            this.textLabel = new Label(this.textToShow);
            this.textLabel.SetFontScale(FontScale);

            // layout
            table.SetFillParent(true);
            table.Top().PadLeft(10).PadTop(10).PadRight(10);
            table.Add(this.textLabel).Height(105).Width(452);
            table.Row().SetPadTop(0);
            table.Add(this.closeButton).Height(30).Width(80);
            
            this.hideWindowInput = new VirtualButton();
            this.hideWindowInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.E));
            this.hideWindowInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.RightControl));
            this.hideWindowInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.B));
        }
        
        public override void OnRemovedFromEntity()
        {
            this.hideWindowInput.Deregister();
        }

        protected override void HideWindow()
        {
            base.HideWindow();
            this.Window.GetStage().SetGamepadFocusElement(null);
        }

        protected override void ShowWindow()
        {
            base.ShowWindow();
            this.Window.GetStage().SetGamepadFocusElement(this.closeButton);
        }

        private void CloseWindow()
        {
            if (!this.IsVisible)
            {
                return;
            }

            this.HideWindow();
            this.done?.Invoke();
        }
        

        public void Update()
        {
            if (this.hideWindowInput.IsPressed)
            {
                this.CloseWindow();   
            }
        }

        public void ShowText(string text, Action doneAction)
        {
            this.done = doneAction;
            this.textToShow = text ?? "";
            this.textLabel.SetText(this.textToShow);
            this.ShowWindow();
        }
    }
}