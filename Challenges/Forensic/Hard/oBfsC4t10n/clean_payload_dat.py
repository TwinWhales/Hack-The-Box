
import re

try:
    with open('payload.dat', 'r', encoding='utf-8') as f:
        lines = f.readlines()

    cleaned_content = ""
    for line in lines:
        # Regex to match "   123: " prefix
        # It seems to be spaces, number, colon, space.
        # But wait, looking at the file content in step 442:
        #    0: "<html>...
        # Also there is a leading quote " at the start of the first line and maybe trailing quotes?
        # Let's check line 0: '   0: "<html><head><script language=""vbscript"">'
        # It seems to be a Dump format. Strings might be quoted.
        
        # Let's remove the prefix first.
        match = re.match(r'^\s*\d+:\s*(.*)', line)
        if match:
            content = match.group(1).strip()
            
            # Remove wrapping quotes if present?
            # Step 442 shows: '   0: "<html>...'
            # And inner quotes seem doubled: 'language=""vbscript""' -> This is CSV or VB style escaping.
            
            # Let's see if it starts with ".
            if content.startswith('"'):
                content = content[1:]
            # And ends with "?
            # Line 0: "<html>...<script...>" -> probably doesn't end with quote on same line unless it's a single line string.
            # But here it spans multiple lines.
            # Wait, line 1441: ... AddFromString ""Private ""&""Type ...
            
            # It looks like the user pasted a "Locals" window dump or similar.
            # The content behaves like a VB string literal Dump.
            # Double quotes are escaped as "".
            
            # Let's replace "" with "
            content = content.replace('""', '"')
            
            # Remove trailing quote if it's the last char?
            # Actually, let's just append and let the user decide or try to be smart.
            # Looking at the last line: ... </script></head></html>".
            # It ends with ".
            
            if content.endswith('"'):
                content = content[:-1]
                
            cleaned_content += content + "\n"
    
    with open('payload_extracted.hta', 'w', encoding='utf-8') as f:
        f.write(cleaned_content)
        
    print(f"Success: payload_extracted.hta created (Size: {len(cleaned_content)} bytes)")

except Exception as e:
    print(f"Error: {e}")
