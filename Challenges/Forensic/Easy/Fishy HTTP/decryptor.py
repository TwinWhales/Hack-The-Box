import os
import re

def decrypt_files():
    base_path = r"C:\Users\user\Desktop\Pentest\Forensic\Easy\Fishy HTTP\http_list"
    
    # Tag mapping from suspicious file (C# code)
    tag_hex = {
        "cite": "0", "h1": "1", "p": "2", "a": "3",
        "img": "4", "ul": "5", "ol": "6", "button": "7",
        "div": "8", "span": "9", "label": "a", "textarea": "b",
        "nav": "c", "b": "d", "i": "e", "blockquote": "f"
    }

    files_to_process = ["%5c", "%5c(1)", "%5c(2)", "%5c(3)"] # Based on directory listing

    print("[-] Starting decryption...")

    for filename in files_to_process:
        filepath = os.path.join(base_path, filename)
        
        if not os.path.exists(filepath):
            print(f"[!] File not found: {filepath}")
            continue

        try:
            with open(filepath, 'r', encoding='utf-8') as f:
                content = f.read()

            # Extract body content
            body_match = re.search(r'<body>(.*?)</body>', content, re.DOTALL)
            if not body_match:
                print(f"[!] {filename}: No <body> tag found")
                continue
            
            body_content = body_match.group(1)
            
            # Find all tags
            # Regex from C#: <(\\w+)[\\s>]
            tags = re.finditer(r'<(\w+)[\s>]', body_content)
            
            hex_string = ""
            for match in tags:
                tag_name = match.group(1)
                if tag_name == "li":
                    continue
                if tag_name in tag_hex:
                    hex_string += tag_hex[tag_name]
            
            # Convert hex to bytes/string
            try:
                # Iterate by 2 chars
                decoded_chars = []
                for i in range(0, len(hex_string), 2):
                    byte_val = int(hex_string[i:i+2], 16)
                    decoded_chars.append(chr(byte_val))
                
                decoded_text = "".join(decoded_chars)
                print(f"[+] {filename} Decoded Content:")
                print(f"    {decoded_text}")
                
                with open('decrypted_output.txt', 'a', encoding='utf-8') as outfile:
                     outfile.write(f"[+] {filename} Decoded Content:\n{decoded_text}\n\n")
                
            except Exception as e:
                print(f"[!] {filename}: Error converting hex to string - {e}")

        except Exception as e:
            print(f"[!] {filename}: Error reading file - {e}")

if __name__ == "__main__":
    # Clear previous output
    open('decrypted_output.txt', 'w').close()
    decrypt_files()
