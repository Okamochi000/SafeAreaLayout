# SafeAreaLayout
【Unity】セーフエリアを考慮したヘッダー、フッター、ボディレイアウト
![safearealayout_image](https://user-images.githubusercontent.com/49199105/125981900-debd9fab-abdb-44fe-8286-ea336a91c484.jpg)

縦持ち、横持ちのセーフエリアを制御するスクリプトです。<br>

・SafeAreaLayout.cs<br>
  自動でセーフエリアのサイズに調整します。<br>
  top,bottom,left,rightに参照を追加している場合は、参照先のサイズに合わされます。<br>
  
・IgnoneSafeAreaLayout.cs<br>
  セーフエリア内のオブジェクトサイズをセーフエリア外まで広げます。<br>
  SafeAreaLayoutより下の階層のオブジェクトに付けてください。<br>
  
・HorizontalLayoutGrounp.cs<br>
  左右のセーフエリア外のサイズに調整します。<br>
  IsVerticalSafeAreaにチェックを入れると、縦の長さはセーフエリア内に収めるように調整されます。<br>
  
・VerticalLauoutGroup.cs<br>
  上下のセーフエリア外のサイズに調整します。<br>
  IsHorizontalSafeAreaにチェックを入れると、横の長さはセーフエリア内に収めるように調整されます。<br>
