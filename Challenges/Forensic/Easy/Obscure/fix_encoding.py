
try:
    # Read as UTF-16 (or whatever it is, likely utf-16-le with BOM from PowerShell)
    # If it fails, try utf-8
    try:
        with open('keepass.hash', 'r', encoding='utf-16') as f:
            content = f.read()
    except UnicodeError:
        with open('keepass.hash', 'r', encoding='utf-8') as f:
            content = f.read()

    # Clean up: strip whitespace, ensure no BOM in content variable
    content = content.replace('\ufeff', '').strip()

    # Write back as UTF-8 without BOM
    with open('keepass.hash', 'w', encoding='utf-8') as f:
        f.write(content)

    print("[+] Successfully converted 'keepass.hash' to UTF-8.")
    
    # Verify content start
    print(f"[*] Preview: {content[:50]}...")

except Exception as e:
    print(f"[-] Error converting file: {e}")
