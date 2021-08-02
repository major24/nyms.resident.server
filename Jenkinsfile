pipeline {
  agent any

  stages {
    stage('Fetch') {
      steps {
        echo 'Fetching code from git..'
        git 'https://github.com/major24/nyms.resident.serverxx'
        echo 'Fetching - Done Success'
      }
    }

    stage('Build') {
      steps {
        echo 'Building code..'
      }
    }

    stage('Test') {
      steps {
        echo 'Testing..'
      }
    }
    
  }
}
