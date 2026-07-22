using NuGet.Protocol;
using Online_Whiteboard_Backend.Models;
using System.Buffers.Text;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace Online_Whiteboard_Backend
{
    public class WhiteboardState
    {
        private Whiteboard _whiteboard;
        private Dictionary<string, UserWrapper> _editors;
        private List<string> _viewers;
        private Bitmap _drawing;

        public WhiteboardState(Whiteboard whiteboard)
        {
            _editors = new Dictionary<string, UserWrapper>();
            _viewers = new List<string>();
            _whiteboard = whiteboard;
            _drawing = Bitmap.FromStream(new MemoryStream(_whiteboard.WhiZeichnung)) as Bitmap;
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

        public Whiteboard Whiteboard {
            get
            {
                return _whiteboard;
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
                    userWrapper.id = user.Nutzer.NutIdPk;
                    userWrapper.name = user.Nutzer.NutAnzeigename;
                    _editors.Add(connectionId, userWrapper);
                }
            }
            var stream = new MemoryStream();
            _drawing.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            var thing = Convert.ToBase64String(stream.ToArray());
            return new OpenWhiteboardResponse(_whiteboard, thing, CurrentEditors);
        }

        public void Dissconnect(string id)
        {
            _editors.Remove(id);
            _viewers.Remove(id);
        }
    }
}
