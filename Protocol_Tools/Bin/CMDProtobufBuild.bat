del Cmd.cs

del Cmd.dll

del CmdSerializer.dll

del Cmd.bin

del Cmd.proto

rd /s /q temp

md temp

python removepack.py .. temp

type temp\*.proto >> Cmd.proto

findstr /iv "import" Cmd.proto>Cmd1.proto&move Cmd1.proto Cmd.proto

findstr /iv "package" Cmd.proto>Cmd1.proto&move Cmd1.proto Cmd.proto

copy /y Cmd.proto protobuf-net\ProtoGen

protobuf-net\ProtoGen\protoc -I. Cmd.proto -o Cmd.bin

protobuf-net\ProtoGen\protogen -i:Cmd.bin -o:Cmd.cs

"C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe" /target:library /r:protobuf-net.dll Cmd.cs

protobuf-net\Precompile\precompile Cmd.dll -o:CmdSerializer.dll -t:CmdSerializer

copy /y Cmd.dll dll\

copy /y CmdSerializer.dll dll\

del Cmd.dll

del CmdSerializer.dll

pause