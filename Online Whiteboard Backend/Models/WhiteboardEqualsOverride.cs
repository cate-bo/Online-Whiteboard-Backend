namespace Online_Whiteboard_Backend.Models
{
    public partial class Whiteboard
    {
        public override bool Equals(object? obj)
        {
            try
            {
                if ((obj as Whiteboard).WhiIdPk == this.WhiIdPk)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
