using BusinessLogic;
using Ninject;

namespace WinFormsView;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
        Logic logic = ninjectKernel.Get<Logic>();

        Application.Run(new MainForm(logic));
    }
}
