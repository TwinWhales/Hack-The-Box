import re

def deobfuscate():
    # Read the file
    with open(r"c:\Users\user\Desktop\Hack The Box\Challenges\Forensic\Hard\oBfsC4t10n\payload_extracted.hta", "r", encoding="utf-8") as f:
        content = f.read()

    # Find the AddFromString line
    # The content spans multiple lines with "_" line continuations
    # We'll capture everything inside AddFromString "..."
    
    # 1. Extract the full string argument
    start_marker = 'xlmodule.CodeModule.AddFromString "'
    start_idx = content.find(start_marker)
    if start_idx == -1:
        print("Could not find AddFromString call")
        return

    # Find the end of the script block or just assume it goes until the end of the concatenation
    # The unique thing is that it uses VBScript string concatenation
    
    # Let's manually parse the components from the start_marker
    current_idx = start_idx + len('xlmodule.CodeModule.AddFromString ')
    
    # Extract the raw chunk that corresponds to the string construction
    # It ends before "objExcel.DisplayAlerts = False" (Line 97)
    end_marker = 'objExcel.DisplayAlerts = False'
    end_idx = content.find(end_marker)
    
    raw_code_chunk = content[current_idx:end_idx].strip()
    
    # This chunk contains "string" & Chr(x) & _ \n ...
    # process it to build the actual string
    
    # improved tokenizer essentially
    full_source_code = ""
    
    # Remove line continuations
    clean_chunk = raw_code_chunk.replace("_\n", "").replace("_\r\n", "")
    
    # Split by '&'
    parts = clean_chunk.split('&')
    
    for part in parts:
        part = part.strip()
        if not part:
            continue
            
        if part.startswith('"') and part.endswith('"'):
            # String literal
            full_source_code += part[1:-1]
        elif part.startswith('Chr(') and part.endswith(')'):
            # Char code
            code = int(part[4:-1])
            full_source_code += chr(code)
        else:
            print(f"Warning: Unknown part format: {part}")

    # Now we have the deobfuscated source code string (the content of the macro)
    # Apply Step 2: API Renaming and adjustments
    
    # API Aliasing
    full_source_code = full_source_code.replace('Alias "CreateStuff"', 'Alias "CreateRemoteThread"')
    full_source_code = full_source_code.replace('Function CreateStuff', 'Function CreateRemoteThread')
    
    full_source_code = full_source_code.replace('Alias "AllocStuff"', 'Alias "VirtualAllocEx"')
    full_source_code = full_source_code.replace('Function AllocStuff', 'Function VirtualAllocEx')
    
    full_source_code = full_source_code.replace('Alias "WriteStuff"', 'Alias "WriteProcessMemory"')
    full_source_code = full_source_code.replace('Function WriteStuff', 'Function WriteProcessMemory')
    
    full_source_code = full_source_code.replace('Alias "RunStuff"', 'Alias "CreateProcessA"')
    full_source_code = full_source_code.replace('Function RunStuff', 'Function CreateProcessA')
    
    # Also replace calls in the body
    full_source_code = full_source_code.replace(' CreateStuff(', ' CreateRemoteThread(')
    full_source_code = full_source_code.replace(' AllocStuff(', ' VirtualAllocEx(')
    full_source_code = full_source_code.replace(' WriteStuff(', ' WriteProcessMemory(')
    full_source_code = full_source_code.replace(' RunStuff(', ' CreateProcessA(')

    # Shellcode Array Fix
    # Pattern: Array(-35, -63, ..., 78)
    def array_replacer(match):
        numbers_str = match.group(1)
        numbers = [int(x.strip()) for x in numbers_str.split(',')]
        
        # Convert to unsigned byte then hex
        hex_values = []
        for n in numbers:
            unsigned = n & 0xFF
            hex_values.append(f"&H{unsigned:02X}")
            
        return "Array(" + ", ".join(hex_values) + ")"

    full_source_code = re.sub(r'Array\(([-0-9, \n\r]+)\)', array_replacer, full_source_code)

    # Output the result
    # We want to replace the original file's messy part with this clean string.
    # However, since the user wants the obfuscation REMOVED, we can simple put the Clean String 
    # directly into the AddFromString call, BUT VBScript doesn't support multi-line strings easily without line continuations.
    
    # Strategy: formatting the `full_source_code` back into a VBScript friendly format (with " & _ \n " )
    # but using meaningful chunks (whole lines) instead of random splits.
    
    lines = full_source_code.split('\n')
    formatted_vb_string = ""
    for i, line in enumerate(lines):
        # Escape quotes
        line_esc = line.replace('"', '""')
        # Wrap in quotes and add vbCrLf equivalent
        # If it's the last line, don't add Chr(10)
        suffix = '&Chr(10)' if i < len(lines) - 1 else ''
        
        # We need to construct the VBScript line
        # "line_content" & Chr(10) & _
        vb_line = f'"{line_esc}"{suffix}'
        
        if i < len(lines) - 1:
             vb_line += " & _\n      "
        
        formatted_vb_string += vb_line

    # Reconstruct the file
    new_content = content[:current_idx] + formatted_vb_string + "\n      " + content[end_idx:]
    
    # Save to a new file first to verify
    output_path = r"c:\Users\user\Desktop\Hack The Box\Challenges\Forensic\Hard\oBfsC4t10n\payload_deobfuscated.hta"
    with open(output_path, "w", encoding="utf-8") as f:
        f.write(new_content)
        
    print(f"Created {output_path}")

if __name__ == "__main__":
    deobfuscate()
