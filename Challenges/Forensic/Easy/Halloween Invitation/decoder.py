import re

def hex_to_str(hex_str):
    return bytes.fromhex(hex_str).decode('utf-8', 'ignore')

def numbers_to_str(numbers_str):
    numbers = numbers_str.split()
    chars = []
    for n in numbers:
        if n.isdigit():
            chars.append(chr(int(n)))
    return "".join(chars)

with open('result.txt', 'r', encoding='utf-8') as f:
    content = f.read()

okbzichkqtto_match = re.search(r"Private Function okbzichkqtto\(\) As String(.*?)End Function", content, re.DOTALL)

if okbzichkqtto_match:
    okbzichkqtto_code = okbzichkqtto_match.group(1)
    
    hex_strings = re.findall(r'uxdufnkjlialsyp\("([a-fA-F0-9]+)"\)', okbzichkqtto_code)
    
    decoded_string = "".join([hex_to_str(h) for h in hex_strings])
    
    shellcode = numbers_to_str(decoded_string)
    
    with open('shellcode.txt', 'w', encoding='utf-8') as out_file:
        out_file.write(shellcode)
else:
    print("Could not find the target function 'okbzichkqtto' in the input file.")