import re

def investigate():
    with open('full_stream.dat', 'rb') as f:
        content = f.read()
    
    # Search for ALL ELF magic occurrences
    elf_offsets = [m.start() for m in re.finditer(b'\x7fELF', content)]
    print(f"ELF Magic found at offsets: {elf_offsets}")

    for offset in elf_offsets:
        start = max(0, offset - 50)
        end = min(len(content), offset + 20)
        # Use repr to see escape chars
        print(f"Context around offset {offset}: {content[start:end]}")

if __name__ == "__main__":
    investigate()
