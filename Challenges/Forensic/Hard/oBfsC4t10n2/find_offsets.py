import struct

def find_offsets(filename):
    with open(filename, 'rb') as f:
        data = f.read()

    print(f"File size: {len(data)} bytes")
    print("-" * 40)

    # 1. Find BOUNDSHEET Record
    # Signature: 85 00
    # Structure: [85 00] [Len 2] [Offset 4] [Hidden 1] [Type 1] [NameLen 1] [UnicodeFlag 1] [NameVar]
    # We look for the Sheet Name "c1zB0vasN"
    
    # Try generic search for the name first
    target_name = "c1zB0vasN"
    # In BIFF8, strings are 1 byte (compressed) or 2 bytes (unicode)
    # Let's try finding the ASCII version first
    name_bytes = target_name.encode('utf-8')
    
    # We look for the name, then check if it looks like a BOUNDSHEET record preceding it
    offsets = []
    start = 0
    while True:
        idx = data.find(name_bytes, start)
        if idx == -1:
            break
        offsets.append(idx)
        start = idx + 1
        
    print(f"Found string '{target_name}' at offsets: {offsets}")
    
    found_boundsheet = False
    for name_offset in offsets:
        # Backtrack to find header
        # [Type] is at NameStart - 2 (Flag) - 1 (NameLen) ? No
        # Structure:
        # +0: 85 00 (ID)
        # +2: Len (2)
        # +4: Stream Offset (4)
        # +8: Visibility (1)
        # +9: Sheet Type (1) <--- TARGET
        # +10: Name Len (1)
        # +11: Unicode Flag (1)
        # +12: Name Start (if not unicode header)
        
        # Assumption: Name found is at +12
        # So Record Start would be NameOffset - 12
        potential_start = name_offset - 12
        
        if potential_start >= 0 and data[potential_start:potential_start+2] == b'\x85\x00':
            print(f"\n[!] Likely BOUNDSHEET Record found at {potential_start}")
            
            type_offset = potential_start + 9
            current_type = data[type_offset]
            print(f" -> Type Byte Offset: {type_offset} (0x{type_offset:X})")
            print(f" -> Current Value: 0x{current_type:02X}")
            
            if current_type == 0x01: # Macro
                print(" -> This is indeed a Macro Sheet (0x01).")
                print(" -> PATCH: Change 01 to 00")
                found_boundsheet = True
            elif current_type == 0x00:
                print(" -> This is a Worksheet (0x00).")
            
    if not found_boundsheet:
        print("\n[Warning] Could not definitely locate the BOUNDSHEET record via string search.")
        print("Trying stricter scan...")

    print("-" * 40)
    
    # 2. Find BOF Record for Macro
    # Signature: 09 08
    # Structure: [09 08] [Len 2] [Ver 2] [Type 2] ...
    # We look for Type = 40 00 (Macro Sheet)
    
    print("Scanning for BOF Records (0x0809) with Type Macro (0x0040)...")
    
    bof_sig = b'\x09\x08'
    start = 0
    while True:
        idx = data.find(bof_sig, start)
        if idx == -1:
            break
            
        # Check length to ensure it's valid BOF (usally generic size)
        # But just check Type at +6
        # +0: 09 08
        # +2: Len (2)
        # +4: Ver (2)
        # +6: Type (2) <--- TARGET
        
        if idx + 8 < len(data):
            type_val = data[idx+6:idx+8]
            if type_val == b'\x40\x00': # Macro Sheet
                print(f"\n[!] Macro Sheet BOF found at {idx} (0x{idx:X})")
                
                target_offset = idx + 6
                print(f" -> Type Word Offset: {target_offset} (0x{target_offset:X})")
                print(f" -> Current Value: 0x40 00")
                print(f" -> PATCH: Change 40 to 10")
        
        start = idx + 2

if __name__ == "__main__":
    find_offsets("oBfsC4t10n2.xls")
