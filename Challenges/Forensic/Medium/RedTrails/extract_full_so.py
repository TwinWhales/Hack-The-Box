def extract():
    with open('full_stream.dat', 'rb') as f:
        content = f.read()

    start_offset = 272
    length = 58928
    
    elf_data = content[start_offset : start_offset + length]
    
    # Verify magic bytes
    if not elf_data.startswith(b'\x7fELF'):
        print(f"Warning: Extracted data does not start with ELF magic. Starts with: {elf_data[:10]}")
    else:
        print("ELF magic verified.")

    with open('module.so', 'wb') as out:
        out.write(elf_data)
    
    print(f"Extracted {len(elf_data)} bytes to module.so")

if __name__ == "__main__":
    extract()
