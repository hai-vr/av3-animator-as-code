import re
import tkinter as tk
from tkinter import filedialog
import os
import pyperclip

def remove_group_matches(input_string):
    regex_pattern = r"\| `([^`]+?)\s"
    result = re.sub(regex_pattern, "| `", input_string)
    return result

#read text from file
#root = tk.Tk()
#root.withdraw()

#file_path = filedialog.askopenfilename()

#with open(file_path, 'r') as f:
#    instr = f.read()
#print(remove_group_matches(instr))

#read text from clipboard

#example string - `AacFlTransition AnyTransitionsTo(AacFlState destination)` /// Create a transition from Any to the `destination` state.
def extract_info(input_string):
    type_regex = r"`([^`]+?\s)"
    #should match "AnyTransitionsTo(AacFlState destination)"
    method_regex = r"\s(([A-Za-z].+\))\` )"
    description_regex = r"\/\/\/.(.*)"
    return_type = re.search(type_regex, input_string).group(1)
    method = re.search(method_regex, input_string).group(2)
    description = re.search(description_regex, input_string).group(1)
    return return_type, method, description

instr = pyperclip.paste()
lines = instr.splitlines()
output = ""
for line in lines:
    print(line)
    return_type, method, description = extract_info(line)
    #print("| `" + return_type + "` | `" + method + "` | " + description + " |")
    output += "| `" + method + "` | `" + return_type + "` | " + description + " |\n"

#copy to clipboard
pyperclip.copy(output)
