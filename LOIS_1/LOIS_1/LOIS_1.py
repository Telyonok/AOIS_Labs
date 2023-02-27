#//////////////////////////////////////////////
#// Лабораторная работа №1 по дисциплине ЛОИС 
#// Выполнена студентами группы 121701 БГУИР Чвилёвом И.А., Яковиничем Г.И.
#// Определяет принадлежит ли логическая формула к классу СКНФ
#// 20.02.2023
#//
#// https://ru.wikipedia.org/wiki/Совершенная_конъюнктивная_нормальная_форма

while(True):
    try:
        formula = input ("Enter formula: ")
        isSKNF = True
        tildaCodes = []
        strippedFormula = formula.strip('(').strip( ')');
        if len(strippedFormula) != len(strippedFormula.strip('/\\').strip('\\/')):
            isSKNF = False
            continue
        splittedFormula = formula.split('/\\')
        part = splittedFormula[0].replace('!','').replace(' ', '').replace('(', '').replace(')', '').split('\\/')
        if len(part[0]) == 0:
            isSKNF = False
            print(isSKNF)
            continue
        for i in range(0, len(part), 1):
            for j in range(i+1, len(part), 1):
                if part[i] == part[j]:
                    isSKNF = False
                    continue
        symbols = ''.join(sorted(splittedFormula[0].replace('!','').replace(' ', '')))
        for i in range(0, len(splittedFormula), 1):
            string = splittedFormula[i]
            if ''.join(sorted(string.replace('!','').replace(' ', ''))) != symbols:
                isSKNF = False
                break
            currentCode = 0
            for j in range(0, len(string)):
                if (string[j] == '!'):
                    currentCode += ord(string[j + 1])
            for code in tildaCodes:
                if (code == currentCode):
                    isSKNF = False
                    break
            tildaCodes.append(currentCode)

        print(isSKNF)

    except Exception as e:
        print(False)
        continue