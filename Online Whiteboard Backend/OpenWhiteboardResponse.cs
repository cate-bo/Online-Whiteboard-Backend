using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class OpenWhiteboardResponse
    {

        public OpenWhiteboardResponse(Whiteboard board, List<UserWrapper> currentUsers, int lastUpdate)
        {
            Id = board.WhiIdPk;
            OwnerId = board.WhiBesitzerIdFk;
            Name = board.WhiName;
            Drawing = board.WhiZeichnung;
            CurrentEditors = currentUsers;
            Texts = new List<TextWrapper>();
            foreach(Text text in board.Text)
            {
                Texts.Add(new TextWrapper(text));
            }
            Images = new List<ImageWrapper>();
            LastUpdate = lastUpdate;
        }

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public byte[] Drawing { get; set; }
        public List<UserWrapper> CurrentEditors { get; set; }
        public List<TextWrapper> Texts { get; set; }
        public List<ImageWrapper> Images { get; set; }
        public int LastUpdate { get; set; }
    }
}
