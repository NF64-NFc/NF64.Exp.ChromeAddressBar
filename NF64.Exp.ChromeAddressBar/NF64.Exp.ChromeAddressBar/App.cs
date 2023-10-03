using System;
using System.Diagnostics;
using System.Linq;

namespace NF64.Exp.ChromeAddressBar
{
    internal sealed class App
    {
        private static void Main()
        {
            // 有効なメインウィンドウ(タブウィンドウ)を持つ適当なChromeのプロセスを取得する。
            var chromeProc = Process.GetProcessesByName("chrome")
                    .First(p => p.MainWindowHandle != IntPtr.Zero);
            Console.WriteLine($"0x{chromeProc.MainWindowHandle.ToInt32():X}, {chromeProc.MainWindowTitle}");
            Console.WriteLine();

            var beginDateTime = DateTime.Now;

            // アドレスバーのテキストを取得して表示する。
            var addressBarText = UIAutomationHelperForChrome.GetAddressBarTextForChrome(chromeProc.MainWindowHandle, out var unsecured);
            Console.WriteLine($"addressBarText = {addressBarText}");
            Console.WriteLine($"unsecured = {unsecured}");

            // 取得に要した時間を表示する。
            var procTime = DateTime.Now - beginDateTime;
            Console.WriteLine($"time = {procTime.TotalMilliseconds}msec");

            Console.ReadKey();
        }
    }
}
