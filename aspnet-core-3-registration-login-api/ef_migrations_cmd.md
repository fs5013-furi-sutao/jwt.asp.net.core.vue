# 本番環境/開発環境
開発環境では SQLite の DB を自動生成（SQL の環境構築不要）
本番環境では SQL Server の利用を想定（事前に環境構築が必要）

本番環境の DB 接続設定は、appsettings.json の ConnectionStrings.WebApiDatabase に接続先文字列を記載する

# Migration の実行
すべての launch.json, launchSettings.json の以下の環境変数の値を、実施環境の設定に合わせる。

開発環境:  
```json
ASPNETCORE_ENVIRONMENT="Development"
```

本番環境:  
```json
ASPNETCORE_ENVIRONMENT="Production"
```

# Migrations の初期化
## 開発環境:  
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

## 本番環境:  
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