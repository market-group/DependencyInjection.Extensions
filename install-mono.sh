apt-get install -y --no-install-recommends unzip git libc6 libc6-dev libc6-dbg

curl -Ls https://github.com/GitTools/GitVersion/releases/download/v3.6.5/GitVersion.CommandLine.3.6.5.nupkg -o tmp.zip \ 
  && unzip -d /usr/lib/GitVersion tmp.zip \
  && rm tmp.zip

