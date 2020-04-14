# Process-Runner
This project is a process spawming tool where you can add any 3rd party EXE jobs that will run on their schedule times

#Main Process Runner
  use nssm or any service installation too install the host process runner as a service. check "service.cmd" file for installation details
  Host process accepts the 1st argument as the polling duration of the service which cannot be less then 3000 millis
  
#
---
#Adding Jobs
 Root folder of the process-runner has a folder "Jobs"
 1) Each Job EXE should be in their own folders
 2) Job Folder file system
	* Main executable EXE (abc.exe)
	* Settings File (abc.json)
	* ".lnk" file if refrenced to global applications like dotnet, nodejs, etc
 3) Folder name should be same as that of EXE/LNK inside it
 
 
#Job Settings
  Job settings file (abc.json) contains 2 elements
  1) "args" - Specify the argumers to your job process space separated
  2) "schedule" - Provide the execution schedule. contains 2 property as
      * "type" - can be "Daily, Weekly, Monthly, Yearly"
      * "value" - the schedule timestamp as 
          - Daily - "16:05"
          - Weekly - "Friday 16:05"
          - Monthly - "02 16:05" (02 is the date)
          - Yearly - "01-02 16:05" (01 is month, 02 is date)
          
          
### Sample Job is the part of the project  
---
# ** Scheduling part is still in development. 
    
