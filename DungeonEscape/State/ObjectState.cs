﻿namespace Redpoint.DungeonEscape.State
{
    public class ObjectState
    {
        public int Id { get; set; }
        public bool? Collideable { get; set; }
        public bool? IsOpen { get; set; }
        
        public int? Gold { get; set; }
        
        public int? ItemId { get; set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool IsActive { get; set; } = true;
    }
}