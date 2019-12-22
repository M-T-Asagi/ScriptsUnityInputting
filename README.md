# ScriptsUnityInputting
Unityで使える入力系の処理

## TouchClickManager 
https://github.com/M-T-Asagi/ScriptsUnityInputting/blob/master/Scripts/TouchClickManager.cs

画面のタッチ（クリック）した時の動きから実行された操作を判定しイベントを発行する。

現在は「タッチ開始（マウス押下）」・「タッチ解放（マウス解放）」・「タップ」・「ホールド」・
「ホールドキャンセル」・「スワイプ」・「スワイプ開始」・「スワイプ終了」のイベントが使える。

## RaycastTouchPosition
https://github.com/M-T-Asagi/ScriptsUnityInputting/blob/master/Scripts/RaycastTouchPosition.cs

画面をタッチ（クリック）してその座標を送るとRaycastを発行してくれる。

## GazeInputManager
https://github.com/M-T-Asagi/ScriptsUnityInputting/blob/master/Scripts/GazeInputManager.cs

視線入力処理。視線がオブジェクトに乗ったときイベントを吐く。

## GazeTargetMarkerManager
https://github.com/M-T-Asagi/ScriptsUnityInputting/blob/master/Scripts/GazeTargetMarkerManager.cs

視線入力のマーカーを配置する処理。PositionとRotation.

## GazeTargetObjectManager
https://github.com/M-T-Asagi/ScriptsUnityInputting/blob/master/Scripts/GazeTargetObjectManager.cs

GazeInputManagerと連動し、アタッチされたオブジェクトに視線が乗ったとき、および視線が乗ったまま一定時間が経過したときにイベントを発行する。