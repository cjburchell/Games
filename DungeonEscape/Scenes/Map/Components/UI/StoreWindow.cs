﻿namespace Redpoint.DungeonEscape.Scenes.Map.Components.UI
{
    using System;
    using Common.Components.UI;
    using Microsoft.Xna.Framework;

    public enum StoreAction
    {
        Buy,
        Sell
    }

    public class StoreWindow : TextWindow    
    {
        private readonly bool _sell;
        private readonly string _text;

        public StoreWindow(UiSystem ui, bool sell = true, string text = null) : base(ui, null, new Point(10, MapScene.ScreenHeight / 3 * 2))
        {
            this._sell = sell;
            this._text = text ?? "Welcome to my store.\nI buy and sell items what can I do for you?";
        }

        public void Show(Action<StoreAction?> doneAction)
        {
            var buttonText = this._sell ? new[] {"Buy", "Sell"} : new[] {"Buy"};
            base.Show(this._text, choice =>
            {
                doneAction(choice switch
                {
                    "Sell" => StoreAction.Sell,
                    "Buy" => StoreAction.Buy,
                    _ => null
                });
                
            },  buttonText);
        }
    }
}