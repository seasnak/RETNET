import os
import sys

import math
import numpy as np
import random

from enum import Enum
from dataclasses import dataclass

class Verb(Enum):
    MOVE = 0,
    Jump = 1,
    pass

class JumpType(Enum):
    SHORT = 2,
    MEDIUM = 3,
    LONG = 5,
    pass

# TODO: convert to dataclass
class Action():
    def __init__(self, act_type: Verb, start_time: int, duration: int):
        self.action_type = act_type
        self.start_time = start_time
        self.duration = duration
    
    def __str__(self):
        return f"{self.action_type}, {self.start_time} - {self.duration}"

class RhythmGroup():
    def __init__(self, duration: int, actions: list[int] = []):
        self.actions = actions
        self.duration = duration
        pass
    
    def add_action(self, action_type: Verb, start_time: int, action_dur: int) -> bool:
        group_duration = self.duration
        if (start_time > group_duration):
            return False
        
        if (start_time + action_dur > group_duration) and (action_type != Verb.JUMP):
            action_dur = group_duration - start_time
        
        self.actions.append(
            Action(action_type, start_time, action_dur)
        )
        return True
    pass

class BeatPattern(Enum):
    RANDOM = 0,
    REGULAR = 1,
    SWING = 2,
    pass

# Generates a series of rhythm groupsk
class RhythmGenerator():
    def __init__(self, min_group_dur: int, max_group_dur: int, pattern_freq: dict[BeatPattern, int], density: int, jump_freq: int, beat_freq: list[int]):
        self.min_group_dur = min_group_dur
        self.max_group_dur = max_group_dur
        self.pattern_freq = pattern_freq
        self.density = density
        self.jump_freq = jump_freq

        normalizer = np.sum(beat_freq)
        patterns = [key.value[0] for key in BeatPattern]

        for i in range(len(patterns)):
            self.pattern_freq[patterns[i]] = beat_freq[i] / normalizer

    def get_beat_times(self, group_dur: int, pattern: BeatPattern):
        out: list[int] = [] # the beat pattern to return
        amount = math.floor(group_dur * self.density)

        short_beat = group_dur / (2 * amount - 1)
        long_beat = 3 * short_beat

        for i in range(amount):
            if (pattern == BeatPattern.REGULAR):
                out.append(i * (group_dur / amount))
            elif (pattern == BeatPattern.RANDOM):
                out.append(random.random() * group_dur)
            else: # SWING PATTERN
                if (i % 2): # short step
                    out.append((i/2) * (long_beat * short_beat))
                else: # long step
                    out.append(((i-1)/2) * (long_beat * short_beat) + long_beat)

        return out

    pass



    def generate_rhythm_group(self) -> RhythmGroup:
        group_dur: int = self.max_group_dur if (self.min_group_dur == self.max_group_dur) else abs(random.random() * (self.max_group_dur - self.min_group_dur) + self.min_group_dur)

        # generate a random beat pattern
        seed = random.random()
        cum_freq = 0
        chosen_pattern: BeatPattern = None
        for key in self.pattern_freq.keys():
            cum_freq += self.pattern_freq[key]
            if (cum_freq > seed):
                chosen_pattern = key
        print(chosen_pattern) # DEBUG

        # generate rhythm group
        group: RhythmGroup = RhythmGroup(group_dur)
        beat_times: list[int] = self.get_beat_times(group_dur, chosen_pattern)

        max_jump_hold = 0 # TODO: figure out how to get this value from godot
        jump_lens = [JumpType.SHORT, JumpType.MEDIUM, JumpType.LONG]

        # populate rhythm group
        last_jump_time = -1
        last_jump_dur = 0
        group.add_action(Verb.MOVE, 0, group_dur)
        for time in beat_times:
            if (time > last_jump_time + last_jump_dur) and (random.random() < self.jump_freq):
                jump_type = math.floor(random.random() * len(jump_lens))
                group.add_action(Verb.JUMP, time, jump_lens[jump_type])

        return group
    pass

if __name__ == "__main__":
    # SAMPLE USAGE
    print("Generating Rhythm Based Level")

    pattern_freq_dict = {
        BeatPattern.RANDOM: 0.1,
        BeatPattern.REGULAR: 0.5,
        BeatPattern.SWING: 0.4,
    }
    beat_freq_list = [3, 8, 4]

    rg = RhythmGenerator(min_group_dur=10, max_group_dur=20, pattern_freq=pattern_freq_dict, density=0.2, jump_freq=0.2, beat_freq=beat_freq_list)

    rg_group = rg.generate_rhythm_group()
    for action in rg_group.actions:
        print(action)

    # level_filename = "rhythm_lvl.txt"
    # level = []

    # with open(level_fi lename, 'w') as level_f:
    #     for row in level:
    #         for block in row:
    #             level_f.write(f"{block} ")