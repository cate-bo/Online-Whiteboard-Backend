namespace Online_Whiteboard_Backend.Models
{
    public partial class Nutzer
    {
        public override bool Equals(object? obj)
        {
            try
            {
                if ((obj as Nutzer).NutIdPk == this.NutIdPk)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
