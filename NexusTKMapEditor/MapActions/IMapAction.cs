using System.Drawing;

namespace BaramMapEditor.MapActions
{
    public interface IMapAction
    {
        Point Tile { get; set; }
        void Undo(Map map);
        void Redo(Map map);
    }
}
