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
        echo 'Building code from..!!'
        cmd_exec('echo "Run by build script is starting..."')
        cmd_exec('echo "mybuild.bat"')
      }
    }

    stage('Test') {
      steps {
        echo 'Testing now..'
      }
    }

  }
}

def cmd_exec(command) {
    // return bat(returnStdout: true, script: "${command}").trim()
  bat "chcp 65001\n${command}"
}
