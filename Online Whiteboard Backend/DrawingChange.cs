using System.Numerics;

namespace Online_Whiteboard_Backend
{
    public class DrawingChange : Change
    {
        public int Color { get; set; }
        public Coordinate[] Pixels { get; set; }
    }
}
