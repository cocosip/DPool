# DPool

> 基于Redis的数据缓存池

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu) [![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/cocosip/DPool/blob/master/LICENSE) ![GitHub last commit](https://img.shields.io/github/last-commit/cocosip/Spool.svg) ![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/cocosip/DPool.svg)

| Build Server | Platform | Build Status |
| ------------ | -------- | ------------ |
| Azure Pipelines| Windows |[![Build Status](https://dev.azure.com/cocosip/DPool/_apis/build/status/cocosip.Spool?branchName=master&jobName=Windows)](https://dev.azure.com/cocosip/DPool/_build/latest?definitionId=8&branchName=master)|
| Azure Pipelines| Linux |[![Build Status](https://dev.azure.com/cocosip/DPool/_apis/build/status/cocosip.Spool?branchName=master&jobName=Linux)](https://dev.azure.com/cocosip/DPool/_build/latest?definitionId=8&branchName=master)|

| Package  | Version | Downloads|
| -------- | ------- | -------- |
| `Spool` | [![NuGet](https://img.shields.io/nuget/v/DPool.svg)](https://www.nuget.org/packages/DPool) |![NuGet](https://img.shields.io/nuget/dt/DPool.svg)|

## DPool是基于Redis的数据池

> 从数据池中取数据时,将会自动将数据保存到另外的临时集合中,避免数据丢失。
