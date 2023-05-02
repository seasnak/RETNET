import os
import sys

import random
import math

from enum import Enum

class RhythmType(Enum):
    RANDOM = 0,
    REGULAR = 1,
    SWING = 2,

def generate_rhythm(group_size=5, density=0.5, jump_freq=0.2):
    rhythm = []
    
    return rhythm

if __name__ == "__main__":
    # SAMPLE USAGE
    print("Generating Rhythm Based Level")

    level_filename = "rhythm_lvl.txt"
    level = []

    with open(level_filename, 'w') as level_f:
        for row in level:
            for block in row:
                level_f.write(f"{block} ")