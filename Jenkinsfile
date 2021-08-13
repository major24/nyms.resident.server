pipeline {
  agent any

  stages {
    stage('Fetch') {
      steps {
        echo 'Fetching code from git..'
        git 'https://github.com/major24/nyms.resident.server'
        echo 'Fetching - Done Success'
      }
    }

    stage('Build') {
      steps {
        echo 'Building code from....'
        bat 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\msbuild.exe nyms.resident.server.sln'
      }
    }

    stage('Test') {
      steps {
        echo 'Testing now..'
      }
    }

  }
}
