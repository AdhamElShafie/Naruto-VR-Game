from pathlib import Path
import numpy as np
import os
import time

from Recognizer.VRProject_NN import SetUp as NN
from LEAP.Normalizer import norm as Normal



print("Setting up Neural-Network...")
with open('./Recognizer/Models/w1.txt', 'r') as file:
    w1 = [[float(num.strip()) for num in line.split(' ')] for line in file]
file.close()
with open('./Recognizer/Models/w2.txt', 'r') as file:
    w2 = [[float(num.strip()) for num in line.split(' ')] for line in file]
file.close()
setup_NN = NN(w1,w2)

run = 1
last_type = ""
user_combo = []

combos = [["Tiger","Serpent","Boar"],["Horse","Boar","Tiger"],["Serpent","Horse","Serpent"],["Boar","Tiger","Horse"],["Boar","Horse","Serpent"]]

def reset():
    global run, last_type, user_combo
    last_type = ""
    user_combo = []
    file = Path("./data.txt")
    if file.is_file():
        open('./data.txt', 'w').close()
    file = Path("./normData.txt")
    if file.is_file():
        open('./normData.txt', 'w').close()
    run = 1

reset()

print("Ready...")

def main():
    global run, last_type, user_combo
    while (run == 1):
        file = Path("./data.txt")
        if file.is_file():
            if os.stat("./data.txt").st_size == 0:
                print("data missing")
            else:
                Normal("./data.txt").run()
        else:
            print("waiting for data.txt")

        #<-------- GETTING DATA FROM FILE -------->
        file = Path("./normData.txt")
        if file.is_file():
            if os.stat("./normData.txt").st_size == 0:
                print("normData missing")
            else:
                with open('./normData.txt', 'r') as file:
                    user_input=[[num.strip() for num in line.split(' ')] for line in file][0]
                    out = [0]*(len(user_input)-1)
                    
                for s in range(len(user_input)-1):
                    out[s] = float(user_input[s])
        #<-------- GETTING SOLUTION -------->
                sol = setup_NN.run(out)
                if (sol != last_type and sol != "None"):
                    print(sol)
                    user_combo.append(sol)
                    last_type = sol
                    if (len(user_combo) >= 3):
                        hasCombo = 0
                        for c in range(len(combos)):
                            if (combos[c] == user_combo):
                                hasCombo = c+1
                                print("FOUND-COMBO [" + ', '.join(combos[c]) + "]")
                                break
                        with open('moves.txt', 'w+') as file:
                            np.savetxt(file, [hasCombo], "%d")
                            
                        #reset combos
                        run = 0
        else:
            print("waiting for normData.txt")
        #run = 0
        time.sleep(1)

while(True):
    reset()
    main()