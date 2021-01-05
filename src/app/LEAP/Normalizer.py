#!/usr/bin/env python
# coding: utf-8

from sklearn.preprocessing import normalize
import numpy as np
import csv

class norm:


    def __init__(self,f):
        self.filename = f

    def run(self):
        X = []
        with open (self.filename) as txtfile:
            for val in csv.reader(txtfile, delimiter=" "):
                l_x = []
                for i in range(0, len(val)):
                    if i < len(val):
                        l_x.append(float(val[i]))
                X.append(l_x)

        Y = normalize(X)

        open('./normData.txt', 'w').close() #clean file
        f = open("./normData.txt", "w+")
        for sample in Y:
            for feature in sample:
                f.write("%f " % feature)
        f.close()
