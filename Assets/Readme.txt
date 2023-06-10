-------ディレクトリ構造-------

Assets/
   ├ Plugins/
   ├ Resources/
   ├ UGOMEMO/
   │     ├ Scenes/
   │     ├ Scripts/
   │     └ Test/
   │
   ├ UGOMEMO_Master/
   │     ├ Data
   │     └ MasterData/
   │
   └ UGOMEMO_Resource/
          ├ Animation/
          ├ Prefabs/
          └ Sprite/


------コピペ用-------

├ ┤
│ ─ 
┌  ┐
└  ┘


-----------
このプロジェクトでは全てモデルごとに分けてください。

・UGOMEMO
　　・Scenes   - 各シーン
　　・Scripts  - スクリプト
　　・Test     - テスト用のあれこれ 


・UGOMEMO_Master
　　・Data　　   - データに関するものはここ。 
                    info. (Json, ScriptableObject, csv) etc.

    ・MasterData - 自分以外に触ってほしくないもはここ


・UGOMEMO_Resource
    ・Animation  - アニメーション関連 
    ・Prefabs    - プレファブ
    ・Sprite     - スプライト


-------------------------------------
マスターデータ

https://docs.google.com/spreadsheets/d/1GQGkF1ay2nWhLkMJ-SPAlVd57IRiY53Iz1oQFQ2jSp8/edit#gid=0