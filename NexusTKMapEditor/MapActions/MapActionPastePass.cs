﻿using System.Drawing;

namespace NexusTKMapEditor.MapActions
{
    public class MapActionPastePass : IMapAction
    {
        public Point Tile { get; set; }
        private readonly bool newPass;

        public MapActionPastePass(Point tile, bool newPass)
        {
            Tile = tile;
            this.newPass = newPass;
        }

        public void Undo(Map map)
        {
            map[Tile.X, Tile.Y] = map[Tile.X, Tile.Y] ?? NexusTKMapEditor.Tile.DefaultTile;
            map[Tile.X, Tile.Y].Passable = !newPass;
        }

        public void Redo(Map map)
        {
            map[Tile.X, Tile.Y] = map[Tile.X, Tile.Y] ?? NexusTKMapEditor.Tile.DefaultTile;
            map[Tile.X, Tile.Y].Passable = newPass;
        }
    }
}
