import re
import base64

def deobfuscate(file_path):
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # 1. Extract Base64 string from the HTML
        # Using simple string splitting to avoid regex escaping headaches
        # Target start: FromBase64String('+[char]34+'
        # Target end: '+[char]34+'))
        
        start_marker = "FromBase64String('+[char]34+'"
        end_marker = "'+[char]34+'))"
        
        if start_marker in content:
            temp = content.split(start_marker)[1]
            b64_str = temp.split(end_marker)[0]
            print(f"[+] Found Base64 string (truncated): {b64_str[:20]}...")
        else:
            print("[-] Could not find Base64 string with expected markers.")
            # Fallback: try to find a large base64-like blob? 
            # Or maybe the markers are slightly different in the file reading.
            pass

        # 2. Decode Base64
        decoded_bytes = base64.b64decode(b64_str)
        decoded_ps1 = decoded_bytes.decode('utf-8')
        
        print("\n[+] Decoded Basic PowerShell Layer:")
        print("-" * 40)
        print(decoded_ps1)
        print("-" * 40)
        
        # 3. Analyze the specific flag construction logic
        # The line looks like: ${f`ile} = ("{7}{1}..."-f'}.exe','B{msDt_4s_A_pr0','E','r...s','3Ms_b4D','l3','toC','HT','0l_h4nD')
        # We can extract the format string and the arguments using regex
        
        # Regex to find the format string and arguments
        # Looking for something like: ("{...}"-f'arg1','arg2'...)
        fmt_match = re.search(r'\("([^"]+)"-f(.*?)\)', decoded_ps1)
        
        if fmt_match:
            fmt_str = fmt_match.group(1)
            args_raw = fmt_match.group(2)
            
            # Simple parsing of arguments (splitting by ',' and stripping quotes)
            # This is a specific parser for this challenge's format
            args = [arg.strip().strip("'") for arg in args_raw.split(',')]
            
            # Construct the result
            # format() in python uses {0}, {1} etc which matches PowerShell's -f operator
            try:
                flag_filename = fmt_str.format(*args)
                print(f"\n[+] Deobfuscated Flag/Filename: {flag_filename}")
            except Exception as e:
                print(f"[-] Error reconstructing flag: {e}")
        else:
            print("[-] Could not find formatting pattern for the flag.")

        # Save decoded script
        with open("deobfuscated_payload.ps1", "w", encoding="utf-8") as out:
            out.write(decoded_ps1)
            print("\n[+] Saved decoded PowerShell to 'deobfuscated_payload.ps1'")

    except Exception as e:
        print(f"[-] An error occurred: {e}")

if __name__ == "__main__":
    target_file = "payload.html"
    deobfuscate(target_file)
