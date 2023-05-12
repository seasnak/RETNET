import os
import sys

from enum import Enum

class Block(Enum):
    EMPTY = 0,
    BLOCK = 1,
    SPIKE = 2,
    PLAYER = 10,
    ENEMY = 11,
    pass

class Level():
    def __init__(self, target_fname: str, shape: tuple(int) = (10, 10)):
        self.level_file = target_fname
        self.shape = shape
        self.contents = [shape[0]][shape[1]]
        pass

    def load_level_from_file(self, level_fname: str): 
        self.level_file = level_fname

        with open(level_fname, 'r') as level_f:
            for i, line in enumerate(level_f.readlines()):
                for j, block in enumerate(line.split(' ')):
                    if block == 'B':
                        self.contents[i][j] == Block.BLOCK
                    elif block == 'S':
                        self.content[i][j] == Block.SPIKE
                    else:
                        self.content[i][j] == Block.EMPTY
                    
                pass
        return