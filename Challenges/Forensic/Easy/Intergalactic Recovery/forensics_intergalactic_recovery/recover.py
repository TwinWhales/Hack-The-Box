d1 = open("./healthy1.img", "rb").read()
d2 = open("./healthy2.img", "rb").read()

data = b"".join([bytes([x^y]) for x,y in zip(d1,d2)])

with open("./recovered.img", "wb") as f:
    f.write(data)