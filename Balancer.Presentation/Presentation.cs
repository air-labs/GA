using System;
using System.Windows.Forms;

namespace Presenter
{
    class Presentation
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
