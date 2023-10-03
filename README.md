# NF64.Exp.ChromeAddressBar
マネージドのUIAutomationを利用し、Chromeのアドレスバーにあるテキストを取得するサンプル実装。  

## 参考資料
- ChromeアドレスバーはWindowHandleを持たない
    - https://stackoverflow.com/questions/19497363/how-can-i-scrape-the-contents-of-the-chrome-address-bar-in-c
        - どこかのタイミングで仕様変更があったらしい
    - https://stackoverflow.com/questions/11645123/how-do-i-get-the-url-of-the-active-google-chrome-tab-in-windows
        - UIAutomationしか方法がないらしい

## 開発/動作確認環境
- Visual Studio Professional 2019 Version 16.11.30
- C# 7.3 / .NET Framework 4.8
- Chrome 117.0.5938.92
