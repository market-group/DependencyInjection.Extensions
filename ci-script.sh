#!/usr/bin/env bash

SLN_FILE=`find . -name '*.sln'`

echo "checking for sln list"
SLN=$(readlink -f `ls *.sln`)
DCPROJ=`dotnet sln list | grep dcproj || true`
if [ ! -z "$DCPROJ" ]; then
	echo "Found dcproj files and remove them"
	dotnet sln $SLN remove $DCPROJ
fi


echo "Calculating Version...."
mono /usr/lib/GitVersion/tools/GitVersion.exe /output buildserver
export $(awk -F= '{print $0}' gitversion.properties)


echo "Updates assembly version in the csproj file"
PATTERN="<Version>.*</Version>"
CSPROJ_FILES=()
while IFS=  read -r -d $'\0'; do
    CSPROJ_FILES+=("$REPLY")
done < <(find . -name *.csproj -print0)

for CSPROJ_PATH in ${CSPROJ_FILES[*]}; do 
	if grep -q "${PATTERN}" $CSPROJ_PATH; then
        	sed -i -E "s/(<Version>).*(<\/Version>)/\1$GitVersion_NuGetVersionV2\2/" $CSPROJ_PATH
	else
        	CONTENT="<Version>$GitVersion_NuGetVersionV2</Version>"
	        C=$(echo $CONTENT | sed 's/\//\\\//g')
        	sed -i "/<\/PropertyGroup>/ s/.*/\t${C}\n&/" $CSPROJ_PATH
	fi
done

echo "dotnet restore $SLN_FILE -f --no-cache"
dotnet restore $SLN_FILE -f --no-cache

NUPKG_PATH="$PWD/packages"
echo $NUPKG_PATH

echo "dotnet build $SLN_FILE --no-restore --configuration Release /p:DebugSymbols=false /p:DebugType=pdbonly"
dotnet build $SLN_FILE --no-restore --configuration Release /p:DebugSymbols=false /p:DebugType=pdbonly

if [ ! -z "$TESTS_PATH" ]; then
        for TEST_CSPROJ in  `find $TESTS_PATH/* -name *.csproj`; do
                echo "dotnet test $TEST_CSPROJ --no-build --configuration Release"
                dotnet test $TEST_CSPROJ --configuration Release
        done
else
        echo "No tests path was mentioned"
fi

#TAGS=`echo $PROJECTNAME | tr '.' ' '`
echo "dotnet pack $SLN_FILE -o $NUPKG_PATH --include-symbols --no-restore --no-build --configuration Release --verbosity normal /p:PackageProjectUrl=\"$CI_REPOSITORY_URL\" /p:PackageTags=\"$TAGS\" /p:RepositoryUrl=\"$CI_REPOSITORY_URL\" /p:IsPackable=true /p:PackageVersion=\"$GitVersion_NuGetVersionV2\" /p:Authors=\"$GITLAB_USER_NAME\" /p:Description=\"SemVer $GitVersion_SemVer GitLab CI ProjectId $CI_PROJECT_ID\" /p:Copyright=\"c Stuccomedia LTD.\""
dotnet pack $SLN_FILE -o $NUPKG_PATH \ 
                        --include-symbols \
                        --no-restore \
                        --no-build \
                        --configuration Release \
                        --verbosity normal \
#                        /p:PackageProjectUrl="$CI_REPOSITORY_URL" \
#                        /p:PackageTags="$TAGS" \
#                       /p:RepositoryUrl="$CI_REPOSITORY_URL" \
                        /p:IsPackable=true \
                        /p:PackageVersion="$GitVersion_NuGetVersionV2" \
                        /p:Authors="Market Group" \
                        /p:Copyright="c Market Group LTD."

NUPKG_FILES=`ls $NUPKG_PATH | grep -v 'symbols.nupkg$'`
for FILENAME in $NUPKG_FILES; do
	FILENAME="${FILENAME%.*}"
	mv -f $NUPKG_PATH/$FILENAME.symbols.nupkg $NUPKG_PATH/$FILENAME.nupkg
done

#echo "dotnet nuget push $NUPKG_PATH --source $NUGET_FEED --api-key ********** --no-symbols true"
#dotnet nuget push "$NUPKG_PATH/*.nupkg" --source $NUGET_FEED --api-key $NUGET_API_KEY --no-symbols true
