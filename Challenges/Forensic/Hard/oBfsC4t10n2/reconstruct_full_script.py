import xlrd
import re

def cell_to_rowcol(cell_str):
    match = re.match(r"([A-Z]+)([0-9]+)", cell_str)
    if not match: raise ValueError(f"Invalid cell: {cell_str}")
    col_str, row_str = match.groups()
    row = int(row_str) - 1
    col = 0
    for char in col_str: col = col * 26 + (ord(char) - ord('A') + 1)
    col -= 1
    return row, col

def get_cell_value(sheet, cell_str):
    try:
        row, col = cell_to_rowcol(cell_str)
        if row < sheet.nrows and col < sheet.ncols:
            val = sheet.cell_value(row, col)
            return str(val)
        return ""
    except: return ""

def split_args(s):
    args = []
    current = ""
    quote = False
    parens = 0
    for char in s:
        if char == '"': quote = not quote; current += char
        elif char == '(': 
            if not quote: parens += 1; current += char
        elif char == ')':
            if not quote: parens -= 1; current += char
        elif char == ',':
            if not quote and parens == 0:
                args.append(current.strip()); current = ""
            else: current += char
        else: current += char
    args.append(current.strip())
    return args

def resolve_concat_args(sheet, arg_str, visited):
    args = split_args(arg_str)
    result = ""
    for arg in args:
        clean_arg = arg.strip()
        if clean_arg.startswith('"') and clean_arg.endswith('"'):
             result += clean_arg[1:-1].replace('""', '"')
        elif re.match(r"^[A-Z]+[0-9]+$", clean_arg):
             if clean_arg in visited: 
                 result += "[LOOP]"
             else:
                 val = get_cell_value(sheet, clean_arg)
                 visited.add(clean_arg)
                 # Recurse in case the looked-up cell also has CONCATENATE
                 result += resolve_string(sheet, val, visited=visited)
                 visited.remove(clean_arg)
        else: 
             result += clean_arg
    return result

def resolve_string(sheet, text, depth=0, visited=None):
    if depth > 50: return text
    if visited is None: visited = set()
    
    text = str(text)
    
    # Iteratively replace CONCATENATE(...) with resolved value
    # We loop until no CONCATENATE remains or no change happens
    
    while "CONCATENATE(" in text:
        # Finding the *innermost* or *first* valid CONCATENATE might be tricky with simple find.
        # Let's find the first "CONCATENATE("
        start = text.find("CONCATENATE(")
        if start == -1: break
        
        # Find matching paren
        content_start = start + 12
        balance = 1
        content_end = -1
        for i in range(content_start, len(text)):
            if text[i] == '(': balance += 1
            elif text[i] == ')': balance -= 1
            if balance == 0:
                content_end = i
                break
        
        if content_end != -1:
            inner_args = text[content_start:content_end]
            resolved_segment = resolve_concat_args(sheet, inner_args, visited)
            
            # Replace the function call with the result
            # We wrap in quotes if it looks like a string argument for another function?
            # Or just raw? 
            # If it's inside IF(..., HERE, ...), we probably want the raw string value (quoted if needed?).
            # But the user wants to see the *meaning*, so raw string is better readable.
            # Example: IF(..., "http://...", "file...")
            
            replacement = f'"{resolved_segment}"'
            
            # Splice it in
            text = text[:start] + replacement + text[content_end+1:]
        else:
            # Mismatched/Unclosed, break loop to avoid hang
            break
            
    return text

def main():
    xls_path = "oBfsC4t10n2.xls"
    cmd_path = "command.txt"
    
    print(f"Loading {xls_path}...")
    try:
        book = xlrd.open_workbook(xls_path, formatting_info=True)
    except:
        book = xlrd.open_workbook(xls_path)
    
    # Find macro sheet (likely index 1 now that we know)
    macro_sheet = None
    for s in book.sheets():
         # Heuristic from before
         val = get_cell_value(s, "D8")
         if val and ("IF(" in val or "CONCATENATE" in val):
             macro_sheet = s
             break
    
    if not macro_sheet:
        macro_sheet = book.sheet_by_index(0) # Fallback

    print(f"Using sheet: {macro_sheet.name}")
    print("\n--- Deobfuscated Script ---\n")
    
    with open(cmd_path, 'r', encoding='utf-8') as f:
        for line in f:
            line = line.strip()
            # Parse ' c1zB0vasN,D8,"FORMULA"
            match = re.search(r"^\'[^\,]+,([A-Z]+[0-9]+),", line)
            
            cell_addr = "???"
            formula = line
            
            if match:
                cell_addr = match.group(1)
                
                # Extract the formula/value part better
                # Split by comma twice: [Sheet], [Cell], [Rest]
                parts = line.split(',', 2)
                if len(parts) >= 3:
                     rest = parts[2]
                     # Remove trailing ,""
                     if rest.endswith(',""'): rest = rest[:-3]
                     # Remove enclosing quotes if present (standard CSV/olevba output)
                     if rest.startswith('"') and rest.endswith('"'):
                         # Careful not to strip quotes if they are part of the value itself not csv quoting
                         # olevba quotes the field if it contains delimiters.
                         # Simple unescape:
                         rest = rest[1:-1].replace('""', '"')
                     
                     formula = rest

            # Deobfuscate
            decoded = resolve_string(macro_sheet, formula)
            
            print(f"[{cell_addr}] {decoded}")

if __name__ == "__main__":
    main()
