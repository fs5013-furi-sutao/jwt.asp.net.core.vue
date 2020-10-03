# JWT 認証サンプル
## 構成
### ユーザ認証
- JWT

### バックエンド
- ASP.NET Core WebAPI
- SQL Server

### API Docment
http://localhost:4000/index.html
- Swashbuckle(Swagger/OpenAPI)

### フロントエンド
http://localhost:8081/
- Vue.js
- Vuex
- VeeValidate

## Get Started
### SQL Server
Docker CLI で DB コンテナを起動
```console
docker-compose up -d
```
### ASP.NET Core
バックエンドディレクトリに移動
```console
cd ./aspnet-core-3-registration-login-api
```
#### 本番環境/開発環境で DB を切り替える
開発環境では SQLite の DB を自動生成（SQL の環境構築不要）
本番環境では SQL Server の利用を想定（事前に環境構築が必要）

本番環境の DB 接続設定は、appsettings.json の ConnectionStrings.WebApiDatabase に接続先文字列を記載する

#### 環境変数の設定
すべての launch.json, launchSettings.json の以下の環境変数の値を、実施環境の設定に合わせる。

開発環境:  
```json
ASPNETCORE_ENVIRONMENT="Development"
```

本番環境:  
```json
ASPNETCORE_ENVIRONMENT="Production"
```

#### Migration の実行
##### 開発環境:
初期化  
```console
dotnet ef database update 0 --context SqliteDataContext
dotnet ef migrations remove --context SqliteDataContext
```
マイグレーション
```console
dotnet ef migrations add InitialCreate --context SqliteDataContext --output-dir Migrations/SqliteMigrations
```
テーブル生成
```console
dotnet ef database update --context SqliteDataContext
```

##### 本番環境:
初期化
```console
ASPNETCORE_ENVIRONMENT=Production dotnet ef database update 0 --context DataContext
ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations remove --context DataContext
```
マイグレーション
```console
ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```
テーブル生成
```console
ASPNETCORE_ENVIRONMENT=Production dotnet ef database update --context DataContext
```

#### .NET Core アプリを起動する
WebAPI サーバを起動
```console
dotnet run
```

#### Swagger
Swagger で API ドキュメントを確認:   
http://localhost:4000/index.html


### Vue.js
フロントエンドディレクトリに移動
```console
cd ./vue-vuex-jwt-auth
```

モジュールをインストール
```console
yarn
```

フロントエンドサーバを起動
```console
yarn serve
```

動作確認:  
http://localhost:8081/
