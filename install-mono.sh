apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
apt install apt-transport-https
echo "deb https://download.mono-project.com/repo/ubuntu stable-trusty main" | tee /etc/apt/sources.list.d/mono-official-stable.list
apt-get clean && apt update
apt-get install -y --no-install-recommends mono-devel unzip git libc6 libc6-dev libc6-dbg
rm -rf /var/lib/apt/lists/* /tmp/*

curl -Ls https://github.com/GitTools/GitVersion/releases/download/v3.6.5/GitVersion.CommandLine.3.6.5.nupkg -o tmp.zip \ 
  && unzip -d /usr/lib/GitVersion tmp.zip \
  && rm tmp.zip

