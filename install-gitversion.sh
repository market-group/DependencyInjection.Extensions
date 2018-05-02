curl -Ls https://github.com/GitTools/GitVersion/releases/download/v3.6.5/GitVersion.CommandLine.3.6.5.nupkg -o tmp.zip
mkdir ./GitVersion
unzip -d ./GitVersion tmp.zip
rm tmp.zip
