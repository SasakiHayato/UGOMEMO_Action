<h1>サウンドデータの作成について</h1>
<pre>
  Tools/Sound/
          ├ CreateAddress/
          ├ CreateDataAsset/
</pre>
<h3>CreateAddressWindowについて</h3>
<p>Add Address - アドレスの発行</p>
<pre>
  Assets/Sound/
          ├ BGM/
          ├ Environment/
          ├ SE/
</pre>
<p>上記、図に保存されているサウンドクリップと同名のアドレスを発行します。</p>
<p>Applyすることで、サウンドアドレス用.csが自動生成されます</p>
<hr>
<p>Delete Address Text - アドレス反映用テキストファイルの削除</p>
<pre>
  Assets/Sound/
          ├ Data/SoundAddress.txt
</pre>
<p>上記、図に保存されているアドレス反映用テキスト内を削除します。</p>
<hr>
<p>Delete Address Csharp - 反映されているアドレスの削除</p>
<pre>
  Assets/Scripts/
           ├ Define/SoundAddress.cs
</pre>
<p>上記、図に保存されている発行されたアドレス内を削除します。</p>
<hr>
<h3>CreateDataAssetWindowについて</h3>
<p>Create Asset - サウンドデータ用のScriptableObjectの生成</p>
<pre>
  Asset/Data/
         ├ Sound/
</pre>
<p>上記、図の中に保存されます。ディレクトリがない場合は自動生成されます。</p>
<hr>
<p>Sound Data Name - ファイル名の指定。</p>
<p>Sound Type - サウンドデータのタイプ指定</p>
<p>Clip Count - データ生成時に初期で入れ込むサウンドデータ個数</p>
<p>Sound Address - サウンドデータのアドレスを指定</p>
<p>Clip - サウンドデータのソース</p>
<hr>
