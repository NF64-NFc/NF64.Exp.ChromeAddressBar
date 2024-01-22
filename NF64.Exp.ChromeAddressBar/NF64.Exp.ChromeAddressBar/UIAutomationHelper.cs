using System;
using System.Windows.Automation;

namespace NF64.Exp.ChromeAddressBar
{
    /// <summary>
    /// UIAutomation要素に関する処理を定義します。
    /// </summary>
    public static class UIAutomationHelper
    {
        /// <summary>
        /// 対象要素の子となる要素を取得します。
        /// </summary>
        /// <param name="uiaElem">UIA要素</param>
        /// <returns>対象要素の子となる要素。ない場合は null。</returns>
        public static AutomationElement GetFirstChild(AutomationElement uiaElem)
        {
            var walker = TreeWalker.ControlViewWalker;
            return walker.GetFirstChild(uiaElem);
        }

        /// <summary>
        /// 条件に一致する子要素を取得します。
        /// </summary>
        /// <param name="uiaElem">UIA要素</param>
        /// <param name="condition">条件のコールバック関数</param>
        /// <returns>対象要素の子となる要素。ない場合は null。</returns>
        public static AutomationElement GetChild(AutomationElement uiaElem, Func<AutomationElement, bool> condition)
        {
            var walker = TreeWalker.ControlViewWalker;
            var childElem = walker.GetFirstChild(uiaElem);
            while (true)
            {
                if (childElem is null)
                    break;

                if (condition(childElem))
                    return childElem;

                childElem = walker.GetNextSibling(childElem);
            }
            return null;
        }
    }
}
