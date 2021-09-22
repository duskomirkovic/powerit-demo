@echo off

md ExampleFiles
md ExampleFiles\SubFolder
md ExampleFiles\AnotherFolder

fsutil file createnew .\ExampleFiles\file1.demo.blob 268435456
fsutil file createnew .\ExampleFiles\file2.demo.blob 268435456
fsutil file createnew .\ExampleFiles\file3.demo.blob 268435456

fsutil file createnew .\ExampleFiles\SubFolder\file1.demo.blob 268435456
fsutil file createnew .\ExampleFiles\SubFolder\file2.demo.blob 268435456
fsutil file createnew .\ExampleFiles\SubFolder\file3.demo.blob 268435456

fsutil file createnew .\ExampleFiles\AnotherFolder\file1.demo.blob 268435456
fsutil file createnew .\ExampleFiles\AnotherFolder\file2.demo.blob 268435456
fsutil file createnew .\ExampleFiles\AnotherFolder\file3.demo.blob 268435456

