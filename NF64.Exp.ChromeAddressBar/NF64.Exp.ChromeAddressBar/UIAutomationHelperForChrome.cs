using System;
using System.Windows.Automation;

namespace NF64.Exp.ChromeAddressBar
{
    /// <summary>
    /// [Chrome専用] UIAutomation要素に関する処理を定義します。
    /// </summary>
    internal static class UIAutomationHelperForChrome
    {
        /// <summary>
        /// Chromeのアドレスバーに入力されているテキストを取得する。
        /// </summary>
        /// <param name="hwndChromeTab">Chromeのタブウィンドウを示すウィンドウ ハンドル</param>
        /// <param name="unsecured">
        /// アドレスバーに "保護されていない通信" のアイコンが表示されている場合に true。<br/>
        /// 一般的には http の場合に表示されるはず。
        /// </param>
        /// <returns>Chromeのアドレスバーに入力されているテキストURL。</returns>
        /// <remarks>Chrome 117.0.5938.92</remarks>
        public static string GetAddressBarTextForChrome(IntPtr hwndChromeTab, out bool unsecured)
        {
            // 0. Chromeのタブウィンドウ
            var chromeElem = AutomationElement.FromHandle(hwndChromeTab);

            // 1. 
            // Chromeタブウィンドウ直下の子要素を選別し、アドレスバーを含むツリーを辿る。
            // アドレスバーを含むツリーの辿り方：
            //   className != "Chrome_RenderWidgetHostHWND" &&
            //   frameworkId != "Win32" &&
            //   AutomationId != "TitleBar"
            // 上記の条件に当てはまらない要素が1本だけあり、それの子にアドレスバーが含まれている。
            var containsUrlBarRootElem = UIAutomationHelper.GetChild(
                    chromeElem,
                    (uiaElem) => {
                        return  uiaElem.Current.ClassName != "Chrome_RenderWidgetHostHWND" &&
                                uiaElem.Current.FrameworkId != "Win32" &&
                                uiaElem.Current.AutomationId != "TitleBar";
                    }
                );

            // 2. 
            // 1個下の階層に行く。
            var containsUrlBarChildElem1 = UIAutomationHelper.GetFirstChild(containsUrlBarRootElem);

            // 4. 子の唯一の要素としてPaneを持つノードを採択する。
            var containsUrlBarChildElem3 = UIAutomationHelper.GetChild(
                    containsUrlBarChildElem1,
                    (uiaElem) => {
                        var childElem = UIAutomationHelper.GetFirstChild(uiaElem);
                        if (childElem is null)
                            return false;

                        var programName = childElem.Current.ControlType?.ProgrammaticName;
                        return programName == "ControlType.Pane";
                    }
                );

            // 5. 
            // 子要素を持つ唯一のノードを採択する。
            var containsUrlBarChildElem4 = UIAutomationHelper.GetChild(
                    containsUrlBarChildElem3,
                    (uiaElem) => {
                        var childElem = UIAutomationHelper.GetFirstChild(uiaElem);
                        return childElem != null;
                    }
                );

            // 6. 
            // 唯一のツールバー要素を得る。
            var toolBarElem = UIAutomationHelper.GetChild(
                    containsUrlBarChildElem4,
                    (uiaElem) => {
                        var programName = uiaElem.Current.ControlType?.ProgrammaticName;
                        return programName == "ControlType.ToolBar";
                    }
                );

            // 7.
            // 1個下の階層に行く。
            var toolBarChildElem1 = UIAutomationHelper.GetFirstChild(toolBarElem);;

            // 8. 
            // 唯一のグループ要素を得る。
            var groupElem = UIAutomationHelper.GetChild(
                    toolBarChildElem1,
                    (uiaElem) => {
                        var programName = uiaElem.Current.ControlType?.ProgrammaticName;
                        return programName == "ControlType.Custom";
                    }
                );

            // 9-1.
            // Name == "保護されていない通信" があれば https ではないと判断する
            var httpsMarkElem = UIAutomationHelper.GetChild(
                    groupElem,
                    (uiaElem) => {
                        return uiaElem.Current.Name == "保護されていない通信";
                    }
                );
            unsecured = httpsMarkElem != null;

            // Name == "アドレス検索バー" を得る。
            var addressBarElem = UIAutomationHelper.GetChild(
                    groupElem,
                    (uiaElem) => {
                        return uiaElem.Current.Name == "アドレス検索バー";
                    }
                );

            // アドレスバーのテキストを取得する
            var valuePattern = addressBarElem.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            var urlText = valuePattern.Current.Value;
            return urlText;
        }
    }
}
