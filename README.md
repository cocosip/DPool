# DPool

> 基于Redis的数据缓存池

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/DPool/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/DPool.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/DPool.svg)

| Build Server | Platform | Build Status |
| ------------ | -------- | ------------ |
| Azure Pipelines| Windows |[![Build Status](https://dev.azure.com/cocosip/DPool/_apis/build/status/cocosip.DPool?branchName=master&jobName=Windows)](https://dev.azure.com/cocosip/DPool/_build/latest?definitionId=18&branchName=master)|
| Azure Pipelines| Linux |[![Build Status](https://dev.azure.com/cocosip/DPool/_apis/build/status/cocosip.DPool?branchName=master&jobName=Linux)](https://dev.azure.com/cocosip/DPool/_build/latest?definitionId=18&branchName=master)|

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `DPool` | [![NuGet](https://img.shields.io/nuget/v/DPool.svg)](https://www.nuget.org/packages/Spool) |![NuGet](https://img.shields.io/nuget/dt/DPool.svg)|

## DPool 基于Redis的数据池

> 支持从Redis取数据后,将数据入队到新的链表,避免在取数据后重启应用导致的数据丢失。
