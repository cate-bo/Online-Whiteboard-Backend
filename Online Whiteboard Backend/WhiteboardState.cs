using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class WhiteboardState
    {
        private Whiteboard _whiteboard;
        private Dictionary<string, UserWrapper> _editors;
        private List<string> _viewers;

        public WhiteboardState(Whiteboard whiteboard)
        {
            _editors = new Dictionary<string, UserWrapper>();
            _viewers = new List<string>();
            _whiteboard = whiteboard;
        }

        public bool HasConnections
        {
            get
            {
                if(_editors.Count > 0) return true;
                if(_viewers.Count > 0) return true;
                return false;
            }
        }

        public List<string> All
        {
            get
            {
                List<string> temp = _viewers;
                foreach(var thing in _editors)
                {
                    temp.Add(thing.Key);
                }
                return temp;
            }
        }

        public List<UserWrapper> CurrentEditors
        {
            get
            {
                List<UserWrapper> users = new List<UserWrapper>();
                foreach(var user in _editors)
                {
                    users.Add(user.Value);
                }
                return users;
            }
        }

        public OpenWhiteboardResponse Connect(AspNetUsers? user, string connectionId)
        {
            if(user != null)
            {
                if(user.Nutzer.NutIdPk == _whiteboard.WhiBesitzerIdFk || _whiteboard.BeaNutzerIdFk.Contains(user.Nutzer))
                {
                    UserWrapper userWrapper = new UserWrapper();
                    userWrapper.Id = user.Nutzer.NutIdPk;
                    userWrapper.Name = user.Nutzer.NutAnzeigename;
                    _editors.Add(connectionId, userWrapper);
                }
            }
            return new OpenWhiteboardResponse(_whiteboard, CurrentEditors);
        }

        public void Dissconnect(string id)
        {
            _editors.Remove(id);
            _viewers.Remove(id);
        }
    }
}
