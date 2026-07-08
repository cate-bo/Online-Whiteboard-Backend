using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class ImageWrapper
    {

        public ImageWrapper(Bild bild)
        {
            Id = bild.BilIdPk;
            Datei = bild.BilDatei;
            X = bild.BilX;
            Y = bild.BilY;
        }

        public int Id { get; set; }
        public byte[] Datei { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
