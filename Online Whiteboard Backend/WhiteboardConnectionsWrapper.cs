using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class WhiteboardConnectionsWrapper
    {
        private (string connectionId, UserWrapper)? _owner;
        private Dictionary<string, UserWrapper> _editors;
        private List<string> _viewers;

        public bool HasConnections
        {
            get
            {
                if (_owner != null) return true;
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
                if(_owner != null) temp.Add(_owner.Value.connectionId);
                return temp;
            }
        }

        public WhiteboardConnectionsWrapper()
        {
            _editors = new Dictionary<string, UserWrapper>();
            _viewers = new List<string>();
        }

        public void ConnectOwner(string id)
        {
            _owner = id;
        }

        public void ConnectEditor(string id)
        {
            _editors.Add(id);
        }

        public void ConnectViewer(string id)
        {
            _viewers.Add(id);
        }

        public void Dissconnect(string id)
        {
            if(_owner != null && _owner == id)
            {
                _owner = null;
            }
            _editors.RemoveAll(s => s == id);
            _viewers.RemoveAll(s => s == id);
        }
    }
}
