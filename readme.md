这是 [FreeSql](https://github.com/2881099/FreeSql) 衍生出来的辅助工具包，内含生成器等功能；

> 作者：[mypeng1985](https://github.com/mypeng1985)　　[FreeSql.Tools-1.0.0.zip](https://github.com/2881099/FreeSql.Tools/files/3955304/FreeSql.Tools-1.0.0.zip)

运行源码请将 dll.ref/* 复制到 bin 目录下

推荐改用 dotnet-tool 命令行工具生成实体类，以便后续一键重复生成复盖：

> dotnet tool install -g FreeSql.Generator

新建目录，在地址栏输入 cmd 快速打开命令窗口，输入命令：

> FreeSql.Generator --help

使用命令行工具生成实体类的极大好处，是后续的重新生成操作（一键完成）。

![image](https://user-images.githubusercontent.com/16286519/58793525-e0cf3300-8628-11e9-8959-d2efed685843.png)

