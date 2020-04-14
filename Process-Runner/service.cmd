
nssm stop ProcessRunner confirm
nssm remove ProcessRunner confirm
nssm install ProcessRunner "Process-Runner.exe" "5000"
nssm stop ProcessRunner
nssm set ProcessRunner Application Process-Runner.exe
nssm set ProcessRunner AppDirectory "\"
nssm set ProcessRunner AppParameters 5000
nssm set ProcessRunner DisplayName ProcessRunner
nssm set ProcessRunner Description Process-Runner for the dynamic job modules
nssm set ProcessRunner Start SERVICE_AUTO_START
nssm set ProcessRunner ObjectName LocalSystem
nssm set ProcessRunner AppNoConsole 0
nssm set ProcessRunner AppStdout service.log
nssm set ProcessRunner AppStderr service.log
nssm start ProcessRunner