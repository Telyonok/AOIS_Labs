while (True):
    formula = input ("Enter formula: ")
    isSKNF = True
    lambdaCodes = []
    splittedFormula = formula.split('*')
    symbols = ''.join(sorted(splittedFormula[0].replace('~','')))
    for i in range(0, len(splittedFormula), 1):
        string = splittedFormula[i]
        if ''.join(sorted(string.replace('~',''))) != symbols:
            isSKNF = False
            break
        currentCode = 0
        for j in range(0, len(string)):
            if (string[j] == '~'):
                currentCode += ord(string[j + 1])
        for code in lambdaCodes:
            if (code == currentCode):
                isSKNF = False
                break
        lambdaCodes.append(currentCode)

    print(isSKNF)