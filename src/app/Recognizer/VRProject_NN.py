import numpy as np
import matplotlib.pyplot as plt
import random as rand

def sigmoid(x,deriv):
    if deriv==1:
        return x * (1 - x)
    else:
        return 1 / (1 + np.exp(-x))

def bin(x):
    for i in range(len(x)):
        if x[i] >= .5:
            x[i] = 1
        else:
            x[i] = 0
    return x

class NeuralNetwork:
    def __init__(self,in_val,hid_val,out_val):
        np.random.seed(1)
        self.input = np.zeros((in_val,1))
        self.hidden = np.zeros((hid_val,1))
        self.output = np.zeros((out_val,1))
        self.w1 = np.random.rand(in_val,hid_val)
        self.w2 = np.random.rand(hid_val,out_val)

    def feed(self,x,y):
        self.input = x
        self.org_output = y
        self.hidden = sigmoid(np.dot(self.input, self.w1),0)
        self.output = sigmoid(np.dot(self.hidden, self.w2),0)

    def backprop(self):
        d_w2 = np.dot(np.transpose(np.asmatrix(self.hidden)), np.asmatrix(2*(self.org_output - self.output) * sigmoid(self.output,1)))
        d_w1 = (np.dot(np.transpose(np.asmatrix(self.input)),  np.asmatrix((np.dot(2*(self.org_output - self.output) * sigmoid(self.output,1), np.transpose(self.w2)) * sigmoid(self.hidden,1)))))
        self.w1 += d_w1
        self.w2 += d_w2

    def test(self,x):
        self.input = x
        self.hidden = sigmoid(np.dot(self.input, self.w1),0)
        self.output = sigmoid(np.dot(self.hidden, self.w2),0)
        return self.output

    def setNN(self,w1,w2):
        self.w1 = w1
        self.w2 = w2

#<-------- SETTING UP NEURAL-NETWORK -------->

class SetUp:

    def __init__(self,w1,w2):
        self.size_input = 15 # input layer with 15 neurans, relation to finger joints
        self.size_hidden = 11 # one hidden layer with 11 neurans
        self.size_output = 4 # output layer with 4 neurans, relation to number of classes
        self.NN = NeuralNetwork(self.size_input,self.size_hidden,self.size_output)
        self.NN.setNN(w1,w2)
        self.classes = ["Tiger","Serpent","Horse","Boar"]

    def run(self,user_input):
        found = 0
        sol = bin(self.NN.test(user_input))
        for j in range(len(sol)):
            if sol[j] == 1:
                sol = self.classes[j]
                found = 1
                break
        if (found == 1):
            return sol
        else:
            return "None"
        print("SOLUTION: ", sol)
