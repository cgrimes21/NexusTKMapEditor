using System.Drawing;

namespace MornaMapEditor.MapActions
{
    public interface IMapAction
    {
        Point Tile { get; set; }
        void Undo(Map map);
        void Redo(Map map);
    }
}
