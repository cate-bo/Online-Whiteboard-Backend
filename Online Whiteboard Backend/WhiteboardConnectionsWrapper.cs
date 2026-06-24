using Online_Whiteboard_Backend.Models;

namespace Online_Whiteboard_Backend
{
    public class WhiteboardConnectionsWrapper
    {
        private string? _owner;
        private List<string> _editors;
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
                List<string> temp = _editors.Concat(_viewers).ToList();
                if(_owner != null) temp.Add(_owner);
                return temp;
            }
        }

        public WhiteboardConnectionsWrapper()
        {
            _editors = new List<string>();
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
