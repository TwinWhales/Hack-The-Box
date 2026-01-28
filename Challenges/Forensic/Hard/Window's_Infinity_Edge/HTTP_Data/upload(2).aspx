
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head><title>
	File Upload Control
</title></head>
<body>
    <form method="post" action="upload.aspx" id="form1" enctype="multipart/form-data">
<div class="aspNetHidden">
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKMTI3ODM5MzQ0Mg9kFgICAw8WAh4HZW5jdHlwZQUTbXVsdGlwYXJ0L2Zvcm0tZGF0YRYCAgUPDxYCHgRUZXh0BRpGaWxlIFN1Y2Nlc3NmdWxseSBVcGxvYWRlZGRkZJs9/Lgcs0glhO3MR9VFCW5J8q8eSGZeb/IXfLLPuUqs" />
</div>

<div class="aspNetHidden">

	<input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="/wEdAALA9PyVQykg4eDUPIApbq9M5vhDofrSSRcWtmHPtJVt4CsK3W2QZBHNSXlIPiJU0D0JzF39zBe7U4BXnEAk7pO0" />
</div>
    <div>
    <input type="file" name="fuSample" id="fuSample" />
    <input type="submit" name="btnUpload" value="Upload" id="btnUpload" />
            <span id="lblMessage">File Successfully Uploaded</span>
    </div>
    </form>
</body>
</html>