using System.Drawing;

namespace NexusTKMapEditor.MapActions
{
    public class MapActionPasteObject : IMapAction
    {
        public Point Tile { get; set; }
        private readonly int oldObjectNumber, newObjectNumber;

        public MapActionPasteObject(Point tile, int oldObjectNumber, int newObjectNumber)
        {
            Tile = tile;
            this.oldObjectNumber = oldObjectNumber;
            this.newObjectNumber = newObjectNumber;
        }

        public void Undo(Map map)
        {
            map[Tile.X, Tile.Y] = map[Tile.X, Tile.Y] ?? NexusTKMapEditor.Tile.DefaultTile;
            map[Tile.X, Tile.Y].ObjectNumber = oldObjectNumber;
            
            // Set the tile and those above it to themselves to clear their cache data
            for (int i = 0; i < 12; i++)
            {
                if (Tile.Y - i >= 0)
                {
                    var currentTileY = Tile.Y - i;
                    map[Tile.X, currentTileY] = map[Tile.X, currentTileY] ?? NexusTKMapEditor.Tile.DefaultTile;
                }
            }
        }

        public void Redo(Map map)
        {
            map[Tile.X, Tile.Y] = map[Tile.X, Tile.Y] ?? NexusTKMapEditor.Tile.DefaultTile;
            map[Tile.X, Tile.Y].ObjectNumber = newObjectNumber;
            
            // Set the tile and those above it to themselves to clear their cache data
            for (int i = 0; i < 12; i++)
            {
                if (Tile.Y - i >= 0)
                {
                    var currentTileY = Tile.Y - i;
                    map[Tile.X, currentTileY] = map[Tile.X, currentTileY] ?? NexusTKMapEditor.Tile.DefaultTile;
                }
            }
        }
    }
}
