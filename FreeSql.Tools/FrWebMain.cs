namespace FreeSqlTools
{
    public partial class FrWebMain : DSkin.Forms.MiniBlinkForm
    {
        public FrWebMain(): base("res://FreeSqlTools/Views/mainframe.html")
        {
           
            InitializeComponent();
            Width = (int) (1366 * ZoomFactor);
            Height = (int) (768 * ZoomFactor);
            InnerDuiControl.Borders.AllWidth = 5;
        }
}
}
