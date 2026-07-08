using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class TextWrapper
    {

        public TextWrapper(Text text)
        {
            Id = text.TexIdPk;
            Text = text.TexText;
            X = text.TexX;
            Y = text.TexY;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
