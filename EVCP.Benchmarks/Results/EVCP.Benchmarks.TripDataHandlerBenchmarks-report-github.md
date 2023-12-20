```

BenchmarkDotNet v0.13.11, Windows 11 (10.0.22621.2861/22H2/2022Update/SunValley2)
11th Gen Intel Core i5-1135G7 2.40GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2
  Job-CLZHGK : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method                         | NoOfMessages | NoOfElementsInMessage | Mean        | Error     | StdDev      | Min         | Max         | Median      |
|------------------------------- |------------- |---------------------- |------------:|----------:|------------:|------------:|------------:|------------:|
| Initial                        | 10           | 60                    | 40,030.1 ms | 795.79 ms | 1,307.50 ms | 37,004.7 ms | 42,180.0 ms | 40,121.4 ms |
| WithEdgeIndex                  | 10           | 60                    |    772.5 ms |  15.07 ms |    27.56 ms |    729.4 ms |    842.5 ms |    769.6 ms |
| WithFactPartitions             | 10           | 60                    | 40,617.9 ms | 798.39 ms | 1,009.71 ms | 39,204.1 ms | 42,665.9 ms | 40,432.6 ms |
| WithEdgeIndexAndFactPartitions | 10           | 60                    |    780.4 ms |  13.57 ms |    13.33 ms |    755.7 ms |    803.9 ms |    782.1 ms |
