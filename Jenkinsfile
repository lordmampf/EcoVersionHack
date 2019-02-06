node {
	stage 'Checkout'
		checkout scm

	stage 'Build'
	    bat "nuget restore"
		bat "\"${tool 'MSBuild'}\" EcoVersionHack.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

	stage 'Archive'
		bat "7z a EcoVersionHack.zip Mods/"
		archiveArtifacts 'EcoVersionHack.zip'
		cleanWs()
}