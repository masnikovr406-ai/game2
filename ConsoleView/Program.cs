using BusinessLogic;
using ConsoleView;
using Ninject;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Каталог видеоигр";

IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
Logic logic = ninjectKernel.Get<Logic>();

new GameConsoleApp(logic, new ConsoleRenderer()).Run();
