using System;
using System.Windows.Forms;

namespace NexusTKMapEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string fileName = args.Length == 0 ? null : args[0];
            Application.Run(FormMain.GetFormInstance(fileName));
        }
    }
}
