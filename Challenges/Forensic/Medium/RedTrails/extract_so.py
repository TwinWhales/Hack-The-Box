import binascii

def extract():
    with open('x10SPFHN.so', 'r') as f:
        hex_data = f.read().strip()
    
    # Analyze if it's a raw packet capture with headers in between or just a single stream with headers at start
    # Decode all
    try:
        binary_data = binascii.unhexlify(hex_data)
    except Exception as e:
        print(f"Error unhexlify: {e}")
        return

    # Look for ELF header
    elf_start = binary_data.find(b'\x7fELF')
    if elf_start == -1:
        print("ELF header not found")
        return

    print(f"Found ELF header at offset: {elf_start}")
    
    # We saw a RESP header before it: $58928\r\n
    # Let's check if we can verify the size
    # Look back a few bytes
    preamble = binary_data[max(0, elf_start-20):elf_start]
    print(f"P preamble: {preamble}")

    # Extract the ELF
    # If it is a clean stream, we just take from elf_start
    # If it has packet headers in between, we might need to carve them out, but let's assume it's one reassembled stream first.
    # The file size is 14612 chars -> 7306 bytes.
    # Wait, 7306 bytes is too small for a 58KB file ($58928).
    # The view_file output showed "Total Bytes: 14612".
    # But the RESP header says $58928.
    # This implies we only have a truncated part of the file, or the hex is compressed/abbreviated?
    # Or maybe the ViewFile didn't show everything?
    # ViewFile said "Showing lines 1 to 1". "Total Bytes: 14612".
    # And "The above content shows the entire, complete file contents".
    # If the file is only 14KB, it cannot contain a 58KB ELF.
    # This is suspicious.
    
    # Let's write what we have.
    with open('extracted.so', 'wb') as out:
        out.write(binary_data[elf_start:])
    
    print(f"Wrote {len(binary_data[elf_start:])} bytes to extracted.so")

if __name__ == "__main__":
    extract()
