using System;
using ConeFabric.FairyTales.Acceptance;
using WatiN.Core;
using WatiN.Core.Interfaces;

public class AcceptanceConsole
{
    [STAThread]
    public static int Main()
    {
        var input = GetInput();

        var runner = new AcceptanceRunner(input) {FairyTalesUrl = "http://localhost:666/ft/"};
        using (IBrowser browser = new IE())
        {
            browser.ShowWindow(NativeMethods.WindowShowStyle.Maximize);
            runner.Run(browser);
        }
        
        int ok = 0;
        foreach (string command in runner.Results.Keys)
        {
            bool result = runner.Results[command];
            Console.WriteLine("Test  {0} : [{1}]", command, result ? "Ok" : "Failed");
            if (result)
                ok++;
        }
        Console.WriteLine("Total: {0}/{1} was Ok", ok, runner.Results.Count);
        return ok == runner.Results.Count ? 0 : -1;
    }

    private static string[] GetInput()
    {
        //var read = Console.In.ReadToEnd();
        //return read.Split(new[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        return new[] {"open ProjectPage"};
    }
}